using Usermanager.Model.DTO;

namespace Usermanager.Interfaces;

public interface IRoleService
{
    Task<RoleDto> CreateRoleAsync(RoleCreateDto dto);
    Task DeleteRoleAsync(int roleId);
}
