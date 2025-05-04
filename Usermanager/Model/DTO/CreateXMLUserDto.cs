namespace Usermanager.Model.DTO;

public class CreateXMLUserDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PersonnelNumber { get; set; }

    public int CityId { get; set; }

    public int? GroupId { get; set; }
}
