using Microsoft.AspNetCore.Mvc;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;
using Usermanager.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace Usermanager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowMyFrontend")] // Use the policy name you defined
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        { _userService = userService; }

        [HttpGet]
        public IActionResult GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var list = _userService.GetUsers(pageNumber, pageSize);
            return Ok(list);
        }

        [HttpGet]
        [Route("filter")]
        public IActionResult GetFilteredUsers([FromQuery] int provinceId, [FromQuery] int cityId)
        {
            var list = _userService.GetFilteredUsers(provinceId, cityId);
            return Ok(list);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateUser([FromBody] User user, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ok = _userService.CreateUser(user);
            if (!ok)
                return StatusCode(500, "Failed to create user.");

            var list = _userService.GetUsers(pageNumber, pageSize);
            return Ok(list);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteUsers([FromBody] IEnumerable<UserDto> users, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1)
        {
            var ok = _userService.DeleteUsers(users);
            if (!ok)
                return StatusCode(500, "Failed to delete users.");

            var list = _userService.GetUsers(pageNumber, pageSize);
            return Ok(list);
        }
    }
}
