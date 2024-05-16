using Libra.Dtos;
using Libra.Models;
using Libra.Models.ResponseModels;
using Libra.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Libra.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;

        }

        [HttpPost("SignUp"),AllowAnonymous]
        public async Task<ActionResult> RegistrateUser([FromBody]UserDto user)
        {
            try
            {
                var response = new BaseResponse<string>();
                var userData = new UserData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    HashedPassword = user.Password
                };
                string token = _userService.Registrate(userData);
                if (token != null)
                {
                    response.Message = "Authorized";
                    response.Data = token;
                    return Ok(response);
                }
                else
                {
                    return BadRequest("User with this email already exists.");
                }
            } 
            catch{
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("SignIn"), AllowAnonymous]
        public async Task<ActionResult> LoginUser([FromBody] AuthPayloadDto authDto)
        {
            var response = new BaseResponse<string>();
            try
            {
                var token = _userService.Login(authDto);

                if (token == null)
                {
                    return Unauthorized();
                }
                response.Message = "Authorized";
                response.Data = token;
                return Ok(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
