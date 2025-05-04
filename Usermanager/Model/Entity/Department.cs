using System.ComponentModel.DataAnnotations;

namespace Usermanager.Model.Entity;

public class Department
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public ICollection<Group> Groups { get; set; } = new List<Group>();
}
