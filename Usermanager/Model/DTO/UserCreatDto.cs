namespace Usermanager.Model.DTO;

public class UserCreateDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public String? PhoneNumber { get; set; }

    public int CityId { get; set; } = 1;

    public string? PersonnelNumber { get; set; }
}
