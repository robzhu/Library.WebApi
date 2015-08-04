using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HttpEx;
using HttpEx.REST;

namespace Library.WebApi
{
    /// <summary>
    /// Resources assemblers a responsible for creating resource representations of business domain entities
    /// </summary>
    public abstract class ResourceAssembler
    {
        public string DefaultRouteName = "api";

        //the length of the string "Controller"
        public const int LengthOfControllerSuffix = 10;

        public string GetPrefix<TController>() where TController : ApiController
        {
            Type controllerType = typeof( TController );
            var controllerTypeName = controllerType.Name;
            var prefixLength = controllerTypeName.Length - LengthOfControllerSuffix;
            var prefix = controllerTypeName.Substring( 0, prefixLength );
            return prefix.ToLowerInvariant();
        }
    }

    public abstract class ResourceAssembler<TModel, TResource> : ResourceAssembler where TResource : IResource
    {
        public async Task<IEnumerable<TResource>> ConvertToResourcesAsync( ApiController controller, IEnumerable<TModel> models )
        {
            return await ConvertToResourcesAsync( controller, models, ExpandQuery.Default );
        }
        public virtual async Task<IEnumerable<TResource>> ConvertToResourcesAsync( ApiController controller, IEnumerable<TModel> models, ExpandQuery expand )
        {
            List<TResource> resources = new List<TResource>();
            foreach( var model in models )
            {
                resources.Add( await ConvertToResourceAsync( controller, model, expand ) );
            }
            return resources;
        }

        public async Task<ResourceCollection<TResource>> ConvertToResourceCollectionAsync( ApiController controller, IEnumerable<TModel> models )
        {
            return await ConvertToResourceCollectionAsync( controller, models, ExpandQuery.Default );
        }
        public virtual async Task<ResourceCollection<TResource>> ConvertToResourceCollectionAsync( ApiController controller, IEnumerable<TModel> models, ExpandQuery expand )
        {
            return new ResourceCollection<TResource>( await ConvertToResourcesAsync( controller, models, expand ) )
            {
                Href = controller.GetRequestUriString()
            };
        }

        public async Task<TResource> ConvertToResourceAsync( ApiController controller, TModel model )
        {
            return await ConvertToResourceAsync( controller, model, ExpandQuery.Default );
        }
        public abstract Task<TResource> ConvertToResourceAsync( ApiController controller, TModel model, ExpandQuery expand );

        public async Task<IEnumerable<Hyperlink<TResource>>> ConvertToResourceEnumerableAsync( ApiController controller, IEnumerable<TModel> models )
        {
            var resourceList = new List<Hyperlink<TResource>>();
            foreach( var model in models )
            {
                resourceList.Add( await ConvertToResourceAsync( controller, model ) );
            }

            return resourceList;
        }
    }
}
