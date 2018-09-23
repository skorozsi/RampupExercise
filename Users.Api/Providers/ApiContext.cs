using Microsoft.EntityFrameworkCore;
using Users.Api.Models;

namespace Users.Api.Providers
{
    /// <summary>
    /// Database context for Users in memory storage
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class ApiContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public DbSet<User> Users { get; set; }
    }
}