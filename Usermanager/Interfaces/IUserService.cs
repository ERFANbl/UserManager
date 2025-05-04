using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Interfaces;

public interface IUserService
{
    public ICollection<UserDto> GetUsers(int pageNumber, int pageSize);
    public bool CreateUser(UserCreateDto user);
    public bool DeleteUsers(IEnumerable<UserDto> users);
    public ICollection<UserDto> GetFilteredUsers(int provinceId, int cityId);
    Task AssignUserToGroupAsync(int userId, int departmentId, int groupId);
    Task<AccessibilityDto> GetUserPermissionsAsync(int userId);
    Task<int> ImportFromExcelAsync(Stream excelStream);
    Task<MemoryStream> ExportAllToExcelAsync();
    Task<byte[]> ExportAllToPdfAsync();
}
