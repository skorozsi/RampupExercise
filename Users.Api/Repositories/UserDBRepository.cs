using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Api.Models;
using Users.Api.Providers;

namespace Users.Api.Repositories
{
    /// <summary>
    /// Repository for in memory database of Users
    /// </summary>
    public class UserDBRepository : IUserRepository
    {
        private readonly ApiContext _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository" /> class.
        /// </summary>
        /// <param name="database">The database context.</param>
        public UserDBRepository(ApiContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Create(User user)
        {
            _database.Users.Add(user);
            _database.SaveChanges();
        }

        /// <summary>
        /// Gets the specified user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public User Get(int id)
        {
            var user = _database.Users.Find(id);
            return user;
        }
        /// <summary>
        /// Gets all Users.
        /// </summary>
        /// <returns></returns>
        public IList<User> Get()
        {
            var users = _database.Users.ToList();
            return users;
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The user.</param>
        public void Update(int id, User user)
        {
            var existinguser = _database.Users.Find(id);
            if (existinguser != null)
            {
                existinguser.Name = user.Name;
                existinguser.DoB = user.DoB;
                _database.Users.Update(existinguser);
                _database.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(int id)
        {
            var user = _database.Users.Find(id);
            if (user != null)
            {
                _database.Users.Remove(user);
                _database.SaveChanges();
            }
        }

    }
}
