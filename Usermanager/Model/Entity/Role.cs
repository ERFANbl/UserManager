using System.ComponentModel.DataAnnotations;

namespace Usermanager.Model.Entity;

public enum UserAccess
{
    Guest = 0,
    User = 1,
    Moderator = 2,
    Admin = 3,
}

public class Role
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public List<UserAccess> Accessibility { get; set; } = new List<UserAccess>();

    public ICollection<Group> Groups { get; set; } = new List<Group>();
}
