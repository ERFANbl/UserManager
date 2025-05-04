namespace Usermanager.Model.DTO;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<AccessibilityDto> Accessibilities { get; set; }
}
