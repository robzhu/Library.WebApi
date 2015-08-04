using System.Collections.Generic;

namespace HttpEx.REST
{
    public interface IResourceCollection<T> : IResource where T : IResource
    {
        int Offset { get; }
        int Size { get; }
        List<T> Items { get; }
    }

    public class ResourceCollection<T> : Resource, IResourceCollection<T> where T : IResource
    {
        /// <summary>
        /// If this list is a subset of a larger list, the first item in this list has an index of Offset in that larger list
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// The number of items in this collection
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The contents of the list
        /// </summary>
        public List<T> Items { get; private set; }

        public ResourceCollection()
        {
            Items = new List<T>();
        }

        public ResourceCollection( params T[] items )
        {
            Items = new List<T>( items );
            Size = Items.Count;
        }

        public ResourceCollection( IEnumerable<T> items )
        {
            Items = new List<T>( items );
            Size = Items.Count;
        }
    }
}
