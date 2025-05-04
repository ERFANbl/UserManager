using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usermanager.Interfaces;
using Usermanager.Model.DTO;

namespace Usermanager.Controllers;

[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<RoleDto>> CreateRole(RoleCreateDto dto)
    {
        var role = await _roleService.CreateRoleAsync(dto);
        if (role == null)
        {
            return BadRequest();
        }
        return Ok();
    }

    [HttpDelete("{roleId}/delete")]
    public async Task<ActionResult> DeleteRole(int roleId)
    {
        await _roleService.DeleteRoleAsync(roleId);
        return Ok();
    }
}

