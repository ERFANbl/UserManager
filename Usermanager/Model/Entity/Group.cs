using System.ComponentModel.DataAnnotations;

namespace Usermanager.Model.Entity;

public class Group
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public DateTime CreatedDate { get; set; }

    public int DepartmentID { get; set; }

    public Department Department { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();

    public ICollection<Role> Roles { get; set; } = new List<Role>();

}
