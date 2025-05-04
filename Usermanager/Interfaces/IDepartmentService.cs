using Usermanager.Model.DTO;

namespace Usermanager.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentDto> CreateDepartmentAsync(DepartmentCreateDto dto);
    Task UpdateDepartmentAsync(int departmentId, DepartmentCreateDto dto);
    Task DeleteDepartmentAsync(int departmentId);
}
