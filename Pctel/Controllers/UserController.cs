using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Entities.Dtos;
using Contracts.Services;

namespace Pctel.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        ///<summary> 
        ///To login user
        ///</summary>
        ///<remarks>To create and return session for valid user</remarks> 
        ///<param name="login"></param> 
        ///<response code = "200" >Session type and token returned succesfully</response> 
        ///<response code = "403" >Password wrong</response> 
        ///<response code = "404" >User email not found</response>
        ///<response code = "400" >User email not valid</response> 
        ///<response code="500">Internel server error</response>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login User", Description = "Login user and return session token")]
        [SwaggerResponse(200, "Success", typeof(TokenDto))]
        [SwaggerResponse(403, "Forbidden", typeof(ErrorDto))]
        [SwaggerResponse(400, "BadRequest", typeof(ErrorDto))]
        [SwaggerResponse(404, "NotFound", typeof(ErrorDto))]
        [SwaggerResponse(500, "Internal server error", typeof(ErrorDto))]
        public ActionResult LoginUser([FromBody] LoginDto loginCredentials)
        {
            return Ok(_userService.ValidateUserInputLogin(loginCredentials));
        }

        ///<summary> 
        ///Create New User  
        ///</summary>
        ///<remarks>To create new user</remarks> 
        ///<param name="user"></param> 
        ///<response code = "200" >Id of created user returned successfully</response> 
        ///<response code = "400" >user input not valid</response>
        ///<response code = "409" >email already exist</response>
        ///<response code="500">Internel server error</response>
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Create User", Description = "To create user")]
        [SwaggerResponse(200, "Created", typeof(IdDto))]
        [SwaggerResponse(400, "Bad Request", typeof(ErrorDto))]
        [SwaggerResponse(409, "Conflict", typeof(ErrorDto))]
        [SwaggerResponse(500, "Internal server error", typeof(ErrorDto))]
        public ActionResult CreateUser([FromBody] RegisterDto user)
        {
            return Ok(_userService.ValidateUserInputRegister(user));
        }

        ///<summary> 
        ///Get all user 
        ///</summary>
        ///<remarks>To get all user</remarks> 
        ///<response code = "200" >list of found user</response> 
        ///<response code="500">Internel server error</response>
        [HttpGet("user")]
        [SwaggerOperation(Summary = "Get User", Description = "To get user")]
        [SwaggerResponse(200, "Success", typeof(IdDto))]
        [SwaggerResponse(500, "Internal server error", typeof(ErrorDto))]
        public ActionResult GetUser()
        {
            return Ok(_userService.GetUser());
        }
    }
}