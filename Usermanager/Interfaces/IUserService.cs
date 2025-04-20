using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Interfaces;

public interface IUserService
{
    public ICollection<UserDto> GetUsers(int pageNumber, int pageSize);
    public bool CreateUser(User user);
    public bool DeleteUsers(IEnumerable<UserDto> users);
    public ICollection<UserDto> GetFilteredUsers(int provinceId, int cityId);
}
