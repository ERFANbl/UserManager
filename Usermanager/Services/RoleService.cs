using Microsoft.EntityFrameworkCore;
using System;
using Usermanager.Interfaces;
using Usermanager.Model;
using Usermanager.Model.DBContext;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Services;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _dbcontext;

    public RoleService(ApplicationDbContext context)
    {
        _dbcontext = context;
    }

    public async Task<RoleDto> CreateRoleAsync(RoleCreateDto dto)
    {


        var role = new Role { Name = dto.Name, Accessibility = dto.Accessibilities };
        _dbcontext.Roles.Add(role);
        await _dbcontext.SaveChangesAsync();

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
        };

    }
    public async Task DeleteRoleAsync(int roleId)
    {

        var role = await _dbcontext.Roles.FindAsync(roleId);
        if (role == null)
            throw new NotFoundException("Role not found");

        _dbcontext.Roles.Remove(role);
        await _dbcontext.SaveChangesAsync();
    }
}
