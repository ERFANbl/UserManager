using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Interfaces;

public interface IAccessibilityService
{
    List<string> GetAllAccessibilitiesAsync();
}
