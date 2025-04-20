namespace Usermanager.Model.Entity;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
}
