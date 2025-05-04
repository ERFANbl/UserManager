using Microsoft.EntityFrameworkCore;
using System;
using Usermanager.Interfaces;
using Usermanager.Model;
using Usermanager.Model.DBContext;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Services;

public class GroupService : IGroupService
{
    private readonly ApplicationDbContext _dbContext;

    public GroupService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<GroupDto> CreateGroupAsync(int departmentId, GroupCreateDto dto)
    {
        var department = await _dbContext.Departments.FindAsync(departmentId);
        if (department == null)
            throw new NotFoundException("Department not found");

        DateTime currentTime = DateTime.Now;
        var group = new Group { Name = dto.Name, DepartmentID = departmentId, CreatedDate = currentTime ,Department = department };
        _dbContext.Groups.Add(group);
        await _dbContext.SaveChangesAsync();
        return new GroupDto { Id = group.Id, Name = group.Name };
    }

    public async Task UpdateGroupAsync(int departmentId, int groupId, GroupCreateDto dto)
    {
        var group = await _dbContext.Groups.FindAsync(groupId);
        if (group == null || group.DepartmentID  != departmentId)
            throw new NotFoundException("Group not found in the specified department");

        group.Name = dto.Name;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteGroupAsync(int departmentId, int groupId)
    {
        var group = await _dbContext.Groups.FindAsync(groupId);
        if (group == null || group.DepartmentID != departmentId)
            throw new NotFoundException("Group not found in the specified department");

        _dbContext.Groups.Remove(group);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AssignRole(int departmentId, int groupId, int roleId)
    {
        var group = await _dbContext.Groups.FindAsync(groupId);
        if (group == null || group.DepartmentID != departmentId)
            throw new NotFoundException("Group not found in the specified department");

        var role = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId);

        if (role == null)
            throw new NotFoundException("Role not found");

        if (!group.Roles.Any(r => r.Id == role.Id))
        {
            group.Roles.Add(role);
        }

        await _dbContext.SaveChangesAsync();

    }
}

