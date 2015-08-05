using System;
using System.Linq.Expressions;
using System.Net.Http;
using Drum;

namespace Library.WebApi
{
    public class DrumUrlProvider : IUrlProvider
    {
        private readonly UriMakerContext _context;
        private readonly HttpRequestMessage _request;

        public DrumUrlProvider( UriMakerContext context, HttpRequestMessage request )
        {
            _context = context;
            _request = request;
        }

        public Uri UriFor<TController>( Expression<Action<TController>> action )
        {
            HttpRequestMessage message = _request;
            var maker = new UriMaker<TController>( _context, message );
            return maker.UriFor( action );
        }

        public string UriStringFor<TController>( Expression<Action<TController>> action )
        {
            return UriFor<TController>( action ).ToString();
        }
    }
}
