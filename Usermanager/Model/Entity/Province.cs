namespace Usermanager.Model.Entity;

public class Province
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<City> Cities { get; set; } = new List<City>();
}
