using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserCreationLogService _userCreationLogService;
        private readonly IUserRepository _userRepository;

        public UsersController(IUserCreationLogService userCreationLogService, IUserRepository userRepository)
        {
            _userCreationLogService = userCreationLogService;
            _userRepository = userRepository;
        }

        // GET api/users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(User))]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_userRepository.Get());
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var user = _userRepository.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody] User user)
        {
            var existingUser = _userRepository.Get(user.Id);
            if (existingUser != null)
            {
                throw new ArgumentException("user already exists");
            }
            else
            {
                _userCreationLogService.Log(user);
                _userRepository.Create(user);
            }
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User user)
        {
            _userRepository.Update(id, user);
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }
    }
}
