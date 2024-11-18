using TodoList.Model;
using TodoList.Services;
using TodoList.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace TodoList.Controllers
{
    [Route("api/v1/users/[controller]")]
    [ApiController]
    public class UserController(IUserServices userService) : ControllerBase
    {
        private readonly IUserServices _userService = userService;

        [HttpPost("SingIn")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request) 
        {
            var response = await _userService.Authenticate(request);

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(response);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] bool? isActive = null)
        {
            return Ok(await _userService.GetUsers(isActive));
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] AddUpdateUser userObj)
        {
            var user = await _userService.CreateUser(userObj);
            if (user == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                message = "New user created succesfully",
                id = user!.Id
            });
        }

        [HttpPut]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] AddUpdateUser userObj)
        {
            var user = await _userService.UpdateUser(id, userObj);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                message = "User updated succesfully"
            });
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!await _userService.DeleteUserById(id))
            {
                return NotFound();
            }
            return Ok(new
            {
                message = "User deleted succesfully",
                id
            });
        }
    };
}
