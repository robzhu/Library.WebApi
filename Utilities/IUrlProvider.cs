using System;
using System.Linq.Expressions;

namespace Library.WebApi
{
    public interface IUrlProvider
    {
        Uri UriFor<TController>( Expression<Action<TController>> action );
        string UriStringFor<TController>( Expression<Action<TController>> action );
    }
}
