using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api.Controllers
{
    /// <summary>
    /// Provides an API to handle CRUD operations of Users.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserCreationLogService _userCreationLogService;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userCreationLogService">The user creation log service.</param>
        /// <param name="userRepository">The user repository.</param>
        public UsersController(IUserCreationLogService userCreationLogService, IUserRepository userRepository)
        {
            _userCreationLogService = userCreationLogService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets all the users.
        /// </summary>
        /// <remarks>
        /// usage:
        /// GET api/users
        /// </remarks>
        /// <returns>Return the users</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(User))]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_userRepository.Get());
        }

        /// <summary>
        /// Gets the specified user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <remarks>
        /// usage:
        /// GET api/users/5
        /// </remarks>
        /// <returns>Return the user if it was found otherwise returns HttpErrorCode 404</returns>
        /// <response code="200">Returns the User</response>
        /// <response code="404">If the User was not found</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public ActionResult<string> Get(int id)
        {
            var user = _userRepository.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Posts the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">user already exists</exception>
        /// <remarks>
        /// Sample request:
        /// PUT api/users
        /// {
        /// "id": 1,
        /// "name": "Béla",
        /// "dob": "01-12-1971"
        /// }
        /// </remarks>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="409">Conflit, user already exists + returns the conflicting user</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(409, Type = typeof(User))]
        public ActionResult<User> Post([FromBody] User user)
        {
            var existingUser = _userRepository.Get(user.Id);
            if (existingUser != null)
            {
                return StatusCode((int)HttpStatusCode.Conflict, user);
            }
            else
            {
                _userCreationLogService.Log(user);
                _userRepository.Create(user);
            }
            return Created(String.Format("/api/users/{0}", user.Id), user);
        }

        /// <summary>
        /// Updates the user specified by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The user.</param>
        /// <remarks>        
        /// Sample request:
        ///     PUT api/users/1
        ///     {
        ///        "id": 1,
        ///        "name": "Ottó",
        ///        "dob": "01-12-1971"
        ///     }
        /// </remarks>
        /// <response code="200">Returns the User</response>
        /// <response code="404">If the User was not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404, Type = typeof(int))]
        public ActionResult Put(int id, [FromBody] User user)
        {
            if (user == null || user.Id != id)
                return BadRequest();
            
            var existingUser = _userRepository.Get(id);
            if (existingUser == null)
            {
                return NotFound(id);
            }
            _userRepository.Update(id, user);
            return Ok(user);
        }

        /// <summary>
        /// Deletes User specified by Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <remarks>
        /// usage:
        /// DELETE api/users/5
        /// </remarks>
        /// <response code="200">User was deleted</response>
        /// <response code="404">User was not found on the server</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult Delete(int id)
        {
            var existingUser = _userRepository.Get(id);
            if (existingUser == null)
            {
                return NotFound(id);
            }

            _userRepository.Delete(id);
            return Ok();
        }
    }
}
