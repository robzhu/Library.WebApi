using System;
using System.Linq.Expressions;

namespace HttpEx
{
    public interface IUrlProvider
    {
        Uri UriFor<TController>( Expression<Action<TController>> action );
        string UriStringFor<TController>( Expression<Action<TController>> action );
        string GetRequestUri();
    }
}
