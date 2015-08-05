using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HttpEx;
using HttpEx.REST;

namespace HttpEx
{
    /// <summary>
    /// Resources assemblers a responsible for creating resource representations of business domain entities
    /// </summary>
    public abstract class ResourceAssembler
    {
    }

    public abstract class ResourceAssembler<TModel, TResource> : ResourceAssembler where TResource : IResource
    {
        protected IUrlProvider UrlProvider { get; private set; }

        protected ResourceAssembler( IUrlProvider urlProvider )
        {
            UrlProvider = urlProvider;
        }
        
        public async Task<IEnumerable<TResource>> ConvertToResourcesAsync( IEnumerable<TModel> models )
        {
            return await ConvertToResourcesAsync( models, ExpandQuery.Default );
        }
        public virtual async Task<IEnumerable<TResource>> ConvertToResourcesAsync( IEnumerable<TModel> models, ExpandQuery expand )
        {
            List<TResource> resources = new List<TResource>();
            foreach( var model in models )
            {
                resources.Add( await ConvertToResourceAsync( model, expand ) );
            }
            return resources;
        }

        public async Task<ResourceCollection<TResource>> ConvertToResourceCollectionAsync( IEnumerable<TModel> models )
        {
            return await ConvertToResourceCollectionAsync( models, ExpandQuery.Default );
        }
        public virtual async Task<ResourceCollection<TResource>> ConvertToResourceCollectionAsync( IEnumerable<TModel> models, ExpandQuery expand )
        {
            return new ResourceCollection<TResource>( await ConvertToResourcesAsync( models, expand ) )
            {
                Href = UrlProvider.GetRequestUri()
            };
        }

        public async Task<TResource> ConvertToResourceAsync( TModel model )
        {
            return await ConvertToResourceAsync( model, ExpandQuery.Default );
        }
        public abstract Task<TResource> ConvertToResourceAsync( TModel model, ExpandQuery expand );

        public async Task<IEnumerable<Hyperlink<TResource>>> ConvertToResourceEnumerableAsync( IEnumerable<TModel> models )
        {
            var resourceList = new List<Hyperlink<TResource>>();
            foreach( var model in models )
            {
                resourceList.Add( await ConvertToResourceAsync( model ) );
            }

            return resourceList;
        }
    }
}
