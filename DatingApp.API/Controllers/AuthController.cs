using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthRepository _authRepository { get; }
        public AuthController(IAuthRepository authRepository)
        {
            this._authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
            //validate request
            if(!ModelState.IsValid)
            return BadRequest(ModelState);
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
            if (await _authRepository.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _authRepository.Register(userToCreate , userForRegisterDto.Password);

            return StatusCode(201, createdUser);

        }
    }
}