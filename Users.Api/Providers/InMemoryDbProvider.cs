using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Users.Api.Models;

namespace Users.Api.Providers
{
    public interface IDBProvider
    {
        Dictionary<int, User> GetDB();
    }

    public class InMemoryDbProvider : IDBProvider
    {
        private readonly HttpContext _httpContext;
        public InMemoryDbProvider(IHttpContextFactory httpContextFactory)
        {
            _httpContext = httpContextFactory.Create(new FeatureCollection());            
        }

        public Dictionary<int, User> GetDB()
        {
            if (_httpContext.Items["db"] == null)
            {
                _httpContext.Items["db"] = new Dictionary<int, User>();
            }
            return _httpContext.Items["db"] as Dictionary<int, User>;
        }
    }
}
