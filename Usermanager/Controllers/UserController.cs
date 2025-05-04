using Microsoft.AspNetCore.Mvc;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;
using Usermanager.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace Usermanager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowMyFrontend")] 
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
        public IActionResult CreateUser([FromBody] UserCreateDto user, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ok = _userService.CreateUser(user);
            if (!ok)
                return StatusCode(500, "Failed to create user.");

            var list = _userService.GetUsers(pageNumber, pageSize);
            return Ok(list);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteUsers([FromBody] IEnumerable<UserDto> users, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var ok = _userService.DeleteUsers(users);
            if (!ok)
                return StatusCode(500, "Failed to delete users.");

            var list = _userService.GetUsers(pageNumber, pageSize);
            return Ok(list);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file recived");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".xls" && ext != ".xlsx")
                return BadRequest("File shuld be in Excel format");

            using var stream = file.OpenReadStream();
            var count = await _userService.ImportFromExcelAsync(stream);
            return Ok(new { Created = count });
        }

        [HttpPost("{userId}/assign")]
        public async Task<ActionResult> AssignUserToGroup(int userId, [FromBody] AssignUserToGroupDto dto)
        {
            await _userService.AssignUserToGroupAsync(userId, dto.DepartmentId, dto.GroupId);
            return Ok();
        }

        [HttpGet("{userId}/permissions")]
        public async Task<ActionResult<AccessibilityDto>> GetUserPermissions(int userId)
        {
            var permissions = await _userService.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportUsersToExcel()
        {
            var ms = await _userService.ExportAllToExcelAsync();
            return File(ms.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Users.xlsx");
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportUsersToPdf()
        {
            var pdfBytes = await _userService.ExportAllToPdfAsync();
            return File(pdfBytes, "application/pdf", "Users.pdf");
        }
    }
}
