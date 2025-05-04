using Usermanager.Model.Entity;

namespace Usermanager.Model.DTO;


public class AccessibilityDto
{
    public int Id { get; set; }
    public List<UserAccess> ?userAccesses { get; set; }
}
