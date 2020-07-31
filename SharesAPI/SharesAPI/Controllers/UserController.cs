using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharesAPI.DatabaseAccess;
using SharesAPI.Models;
using SharesAPI.Currency;

namespace SharesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
    private readonly IUsersRepository _userRepository;
        public UserController(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpGet()]
        public IActionResult Get(){
            string username = Request.Headers["username"];
            string password = Request.Headers["password"];
            User user = _userRepository.GetUser(username);
            if(user == null){
                return NotFound("Username and password combination not found.");
            }
            User validUser = _userRepository.Authenticate(username, password);
            if(validUser == null){
                return Forbid("it is forbidden.");
            }
            return Ok(validUser);
        }

        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost()]
        public IActionResult Summon([FromBody] CreateUser user)
        {
            User CreateUser = _userRepository.Add(user);
            if(CreateUser == null){
                return BadRequest("Request is bad");
            }
            return Ok(CreateUser);
        }

    }
}
