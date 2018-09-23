using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Users.Api.Models;

namespace Users.Api.Providers
{
    /// <summary>
    /// Interface for get the database
    /// </summary>
    public interface IDBProvider
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns></returns>
        Dictionary<int, User> GetDB();
    }

    /// <summary>
    /// InMemory DB provider
    /// </summary>
    /// <seealso cref="Users.Api.Providers.IDBProvider" />
    public class InMemoryDbProvider : IDBProvider
    {
        private readonly HttpContext _httpContext;
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryDbProvider"/> class.
        /// </summary>
        /// <param name="httpContextFactory">The HTTP context factory.</param>
        public InMemoryDbProvider(IHttpContextFactory httpContextFactory)
        {
            _httpContext = httpContextFactory.Create(new FeatureCollection());            
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <returns>Returns a Dictionary containing the users</returns>
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
