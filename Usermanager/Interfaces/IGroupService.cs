using Usermanager.Model.DTO;

namespace Usermanager.Interfaces;

public interface IGroupService
{
    Task<GroupDto> CreateGroupAsync(int departmentId, GroupCreateDto dto);
    Task UpdateGroupAsync(int departmentId, int groupId, GroupCreateDto dto);
    Task DeleteGroupAsync(int departmentId, int groupId);
    Task AssignRole(int departmentId, int groupId, int roleId);
}
