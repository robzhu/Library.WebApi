using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace HttpEx.REST
{
    public class HyperlinkConverter : JsonConverter
    {
        private static JsonSerializer DefaultSerializer;

        static HyperlinkConverter()
        {
            DefaultSerializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            DefaultSerializer.Converters.Add( new StringEnumConverter { CamelCaseText = true } );
            DefaultSerializer.Converters.Add( new ResourceConverter() );
        }

        private const string HrefElementName = "href";

        public override bool CanConvert( Type objectType )
        {
            return typeof( IHyperlink ).IsAssignableFrom( objectType );
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            if( reader.TokenType == JsonToken.None ) return null;

            JObject jo = JObject.Load( reader );
            List<JProperty> properties = jo.Properties().ToList();

            //check to see if we've received a json blob like so:
            //  {
            //      "href": "http://server.com/api/resource/1234"
            //  }
            if( ( properties.Count == 1 ) &&
                ( properties[ 0 ].Name.ToLowerInvariant() == HrefElementName ) )
            {
                var jprop = properties[ 0 ];
                var value = jprop.Value.ToString();
                var instance = (IHyperlink)Activator.CreateInstance( objectType );
                instance.Href = value;
                return instance;
            }
            else
            {
                var resourceType = objectType.GetGenericArguments()[ 0 ];
                var value = jo.ToObject( resourceType );

                return Activator.CreateInstance( objectType, value );
            }
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            IHyperlink link = value as IHyperlink;

            if( link.IsLinkOnly )
            {
                //if the resource is just a link, do not serialize any properties other than Href
                writer.WriteStartObject();

                writer.WritePropertyName( HrefElementName );
                serializer.Serialize( writer, link.Href );

                writer.WriteEndObject();
            }
            else
            {
                JObject.FromObject( link.ObjectValue, DefaultSerializer ).WriteTo( writer );
            }
        }
    }
}
