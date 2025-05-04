using System;
using Usermanager.Interfaces;
using Usermanager.Model.DBContext;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Services;

public class AccessibilityService : IAccessibilityService
{
    private readonly ApplicationDbContext _context;

    public AccessibilityService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<string> GetAllAccessibilitiesAsync()
    {
        return new List<string> {"Guest" , "User" , "Moderator", "Admin"};
    }
}

