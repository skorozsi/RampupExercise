using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Users.Api.Models;
using Users.Api.Providers;

namespace Users.Api.Repositories
{
    public interface IUserRepository
    {
        void Create(User user);
        User Get(int id);
        IList<User> Get();
        void Update(int id, User user);
        void Delete(int id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IDictionary<int, User> _database;
        public UserRepository(IDBProvider dbProvider)
        {
            _database = dbProvider.GetDB();
        }

        public void Create(User user)
        {
            _database.Add(user.Id, user);
        }

        public User Get(int id)
        {
            User user;
            _database.TryGetValue(id, out user);
            return user;
        }
        public IList<User> Get()
        {
            return _database.Values.ToList();
        }

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
