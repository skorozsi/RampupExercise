using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Users.Api.Models;
using Users.Api.Providers;

namespace Users.Api.Repositories
{
    /// <summary>
    /// Implements repository pattern for CRUD operation for Users.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Create(User user);
        /// <summary>
        /// Gets the specified user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        User Get(int id);
        /// <summary>
        /// Gets all Users.
        /// </summary>
        /// <returns></returns>
        IList<User> Get();
        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The user.</param>
        void Update(int id, User user);
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Users.Api.Repositories.IUserRepository" />
    public class UserRepository : IUserRepository
    {
        private readonly IDictionary<int, User> _database;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbProvider">The database provider.</param>
        public UserRepository(IDBProvider dbProvider)
        {
            _database = dbProvider.GetDB();
        }

        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Create(User user)
        {
            _database.Add(user.Id, user);
        }

        /// <summary>
        /// Gets the specified user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public User Get(int id)
        {
            User user;
            _database.TryGetValue(id, out user);
            return user;
        }
        /// <summary>
        /// Gets all Users.
        /// </summary>
        /// <returns></returns>
        public IList<User> Get()
        {
            return _database.Values.ToList();
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The user.</param>
        public void Update(int id, User user)
        {
            User existingUser;
            _database.TryGetValue(id, out existingUser);
            if (existingUser == null)
            {
                Create(user);
            }
            else
            {
                _database[id] = user;
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id)
        {
            User existingUser;
            _database.TryGetValue(id, out existingUser);
            if (existingUser == null)
            {
                _database.Remove(id);
            }

        }
    }
}
