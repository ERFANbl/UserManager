using System;
using Usermanager.Interfaces;
using Usermanager.Model;
using Usermanager.Model.DBContext;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Services;
public class DepartmentService : IDepartmentService
{
    private readonly ApplicationDbContext _context;

    public DepartmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentCreateDto dto)
    {
        DateTime currentTime = DateTime.Now;
        var department = new Department { Name = dto.Name, CreatedDate = currentTime};
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return new DepartmentDto { Id = department.Id, Name = department.Name };
    }

    public async Task UpdateDepartmentAsync(int departmentId, DepartmentCreateDto dto)
    {
        var department = await _context.Departments.FindAsync(departmentId);
        if (department == null)
            throw new NotFoundException("Department not found");

        department.Name = dto.Name;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDepartmentAsync(int departmentId)
    {
        var department = await _context.Departments.FindAsync(departmentId);
        if (department == null)
            throw new NotFoundException("Department not found");

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }
}