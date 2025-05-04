using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usermanager.Interfaces;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Controllers;

[ApiController]
[Route("api/departments/{departmentId}/groups")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<GroupDto>> CreateGroup(int departmentId, GroupCreateDto dto)
    {
        var group = await _groupService.CreateGroupAsync(departmentId, dto);

        if (group == null) {
            return BadRequest();
        }

        return Ok(group);
    }

    [HttpPut("{groupId}/update")]
    public async Task<ActionResult> UpdateGroup(int departmentId,int groupId, GroupCreateDto dto)
    {
        await _groupService.UpdateGroupAsync(departmentId, groupId, dto);
        return Ok();
    }

    [HttpPut("{groupId}/role")]
    public async Task<ActionResult> AssignRole(int departmentId, int groupId, int roleId)
    {
        await _groupService.AssignRole(departmentId, groupId, roleId);
        return Ok();
    }

    [HttpDelete("{groupId}/delete")]
    public async Task<ActionResult> DeleteGroup(int departmentId, int groupId)
    {
        await _groupService.DeleteGroupAsync(departmentId, groupId);
        return Ok();
    }
}