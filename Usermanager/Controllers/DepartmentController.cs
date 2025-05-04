using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usermanager.Interfaces;
using Usermanager.Model.DTO;


namespace Usermanager.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentCreateDto dto)
    {
        var department = await _departmentService.CreateDepartmentAsync(dto);
        return Ok(department);
    }

    [HttpPut("{departmentId}/update")]
    public async Task<ActionResult> UpdateDepartment(int departmentId, DepartmentCreateDto dto)
    {
        await _departmentService.UpdateDepartmentAsync(departmentId, dto);
        return Ok();
    }

    [HttpDelete("{departmentId}/delete")]
    public async Task<ActionResult> DeleteDepartment(int departmentId)
    {
        await _departmentService.DeleteDepartmentAsync(departmentId);
        return Ok();
    }
}

