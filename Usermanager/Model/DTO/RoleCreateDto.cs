using Usermanager.Model.Entity;

namespace Usermanager.Model.DTO;

public class RoleCreateDto
{
    public string Name { get; set; }
    public List<UserAccess> Accessibilities { get; set; }
}

