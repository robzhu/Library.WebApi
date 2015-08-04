using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace HttpEx.REST
{
    public class ResourceConverter : JsonConverter
    {
        public const string LinksElementName = "_links";

        private static JsonSerializer DefaultSerializer;

        static ResourceConverter()
        {
            DefaultSerializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            DefaultSerializer.Converters.Add( new StringEnumConverter { CamelCaseText = true } );
            DefaultSerializer.Converters.Add( new HyperlinkConverter() );
        }

        public override bool CanConvert( Type objectType )
        {
            return typeof( IResource ).IsAssignableFrom( objectType );
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            //we might receive json like so:
            //{
            //    "_links":{
            //        "delete": { 
            //            "href": "http://api/delete",
            //            "rel": "delete"
            //        },
            //        "next": { 
            //            "href": "http://api/next",
            //            "rel": "next"
            //        }
            //    }
            //}
            // since the resource doesn't have a "links" property bag on it, we want to populate the "delete" and "next" action link properties

            if( reader.TokenType == JsonToken.None ) return null;
            JObject jo = JObject.Load( reader );
            JObject linksElement = jo.GetValue( LinksElementName ) as JObject;

            if( linksElement != null )
            {
                jo.Remove( LinksElementName );

                foreach( JProperty prop in linksElement.Properties() )
                {
                    jo.Add( prop.Name, prop.Value );
                }
            }

            return jo.ToObject( objectType );
        }

        private static bool IsPropertyActionLink( Type type, string propertyName )
        {
            PropertyInfo objectProperty = type.GetProperty( propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance );
            return ( objectProperty.PropertyType == typeof( Actionlink ) );
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            var jtw = new JTokenWriter();
            DefaultSerializer.Serialize( jtw, value );

            JToken token = jtw.Token;
            JObject obj = (JObject)token;
            JObject linksElement = new JObject();

            foreach( var prop in obj.Properties().ToList() )
            {
                if( IsPropertyActionLink( value.GetType(), prop.Name ) )
                {
                    //this property is an ActionLink, strip it from the root object and add to the "_links" element.
                    obj.Remove( prop.Name );
                    linksElement.Add( prop.Name, prop.Value );
                }
            }

            if( linksElement.Children().Count() > 0 )
            {
                obj.Add( LinksElementName, linksElement );
            }
            obj.WriteTo( writer, serializer.Converters.ToArray() );
        }
    }
}
