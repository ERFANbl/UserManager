﻿namespace Usermanager.Model.Entity;

public class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PersonnelNumber { get; set; }

    public int CityId { get; set; }
    public City ?City { get; set; }

    public int? GroupID { get; set; }
    public Group? Group { get; set; }
}

