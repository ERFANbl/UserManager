using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Usermanager.Model.DBContext;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;
using Usermanager.Interfaces;
using Usermanager.Model;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace Usermanager.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbcontext;

    public UserService(ApplicationDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public ICollection<UserDto> GetUsers(int pageNumber, int pageSize)
    {
        return _dbcontext.Users
            .Include(u => u.City)
            .OrderBy(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Province = u.City.Province.Name,
                City = u.City.Name
            })
            .ToList();
    }

    public bool CreateUser(UserCreateDto user)
    {
        var city = _dbcontext.Cities.Include(a => a.Province).FirstOrDefault(p => p.Id == user.CityId);
        if (city == null)
            throw new NotFoundException("City not found");
        var userentity = new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            PersonnelNumber = user.PersonnelNumber,
            CityId = user.CityId,
            City = city,
        };

        _dbcontext.Users.Add(userentity);
        _dbcontext.SaveChanges();

        var u = _dbcontext.Users
               .Include(x => x.City)
               .First(x => x.Id == userentity.Id);

        return true;
    }

    public bool DeleteUsers(IEnumerable<UserDto> users)
    {
        var userEntities = users.Select(dto => new User { Id = dto.Id });
        if (!userEntities.Any())
            return false;

        _dbcontext.Users.RemoveRange(userEntities);
        _dbcontext.SaveChanges();
        return true;
    }


    public ICollection<UserDto> GetFilteredUsers(int provinceId, int cityId)
    {
        return _dbcontext.Users
            .Include(u => u.City.Province)
            .Include(u => u.City)
            .Where(u => u.City.ProvinceId == provinceId && u.CityId == cityId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Province = u.City.Province.Name,
                City = u.City.Name
            })
            .ToList();
    }

    public async Task AssignUserToGroupAsync(int userId, int departmentId, int groupId)
    {
        var user = await _dbcontext.Users.FindAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        if (!await _dbcontext.Departments.AnyAsync(d => d.Id == departmentId))
            throw new NotFoundException("Department not found");

        var group = await _dbcontext.Groups.FindAsync(groupId);
        if (group == null || group.DepartmentID != departmentId)
            throw new NotFoundException("Group not found in the specified department");

        user.GroupID = groupId;
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<AccessibilityDto> GetUserPermissionsAsync(int userId)
    {
        var user = await _dbcontext.Users.FindAsync(userId);
        if (user == null)
            throw new NotFoundException("User not found");

        if (user.GroupID == null)
            return new AccessibilityDto { Id = userId, userAccesses = new List<UserAccess>() };

        var group = await _dbcontext.Groups.Include(u => u.Roles).FirstOrDefaultAsync(p => p.Id == user.GroupID);

        var roles = group.Roles.ToList();

        var Accessibilities = new List<UserAccess>();

        foreach (var role in roles.ToList())
            Accessibilities.AddRange(Accessibilities.Concat(role.Accessibility).ToList());

        return new AccessibilityDto { Id = userId, userAccesses = Accessibilities.Distinct().ToList()};
    }

    public async Task<int> ImportFromExcelAsync(Stream excelStream)
    {
        using var workbook = new XLWorkbook(excelStream);
        var ws = workbook.Worksheet(1);
        var rows = ws.RangeUsed().RowsUsed().Skip(1);

        var dtos = rows
            .Where(row => !row.CellsUsed().All(c => string.IsNullOrWhiteSpace(c.GetString())))
            .Select(row =>
            {
                int? groupId = null;
                var groupCell = row.Cell(6);
                if (!groupCell.IsEmpty() && int.TryParse(groupCell.GetValue<string>(), out int parsedGroupId))
                    groupId = parsedGroupId;

                return new CreateXMLUserDto
                {
                    FirstName = row.Cell(1).GetValue<string>(),
                    LastName = row.Cell(2).GetValue<string>(),
                    PhoneNumber = row.Cell(3).GetValue<string>(),
                    PersonnelNumber = row.Cell(4).GetValue<string>(),
                    CityId = row.Cell(5).GetValue<int>(),
                    GroupId = groupId
                };
            }).ToList();


        var userentities = new List<User>();

        foreach (var dto in dtos)
        {
            userentities.Add(new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                PersonnelNumber = dto.PersonnelNumber,
                CityId = dto.CityId,
                GroupID = dto.GroupId,
            });
        }

        await _dbcontext.AddRangeAsync(userentities);

        await _dbcontext.SaveChangesAsync();

        return userentities.Count;
    }
    public async Task<MemoryStream> ExportAllToExcelAsync()
    {
        var users = await _dbcontext.Users.Include(u => u.City).Include(g => g.Group).ToListAsync();
        var workbook = new XLWorkbook();
        var ws = workbook.AddWorksheet("Users");

        ws.Cell(1, 1).Value = "Id";
        ws.Cell(1, 2).Value = "FirstName";
        ws.Cell(1, 3).Value = "LastName";
        ws.Cell(1, 4).Value = "PhoneNumber";
        ws.Cell(1, 5).Value = "PersonnelNumber";
        ws.Cell(1, 6).Value = "CityId";
        ws.Cell(1, 7).Value = "City";
        ws.Cell(1, 8).Value = "GroupId";
        ws.Cell(1, 9).Value = "Group";

        for (int i = 0; i < users.Count; i++)
        {
            var u = users[i];
            ws.Cell(i + 2, 1).Value = u.Id;
            ws.Cell(i + 2, 2).Value = u.FirstName;
            ws.Cell(i + 2, 3).Value = u.LastName;
            ws.Cell(i + 2, 4).Value = u.PhoneNumber;
            ws.Cell(i + 2, 5).Value = u.PersonnelNumber;
            ws.Cell(i + 2, 6).Value = u.CityId;
            ws.Cell(i + 2, 7).Value = u.City.Name;
            if (u.GroupID != null)
            {
                ws.Cell(i + 2, 8).Value = u.GroupID;
                ws.Cell(i + 2, 9).Value = u.Group.Name;
            }
            else
            {
                ws.Cell(i + 2, 8).Value = "";
                ws.Cell(i + 2, 9).Value = "";
            }
                
        }

        var ms = new MemoryStream();
        workbook.SaveAs(ms);
        ms.Position = 0;
        return ms;
    }
    public async Task<byte[]> ExportAllToPdfAsync()
    {
        var users = await _dbcontext.Users.Include(u => u.City).Include(g => g.Group).ToListAsync();

        var doc = new Document();
        var section = doc.AddSection();
        section.PageSetup.Orientation = Orientation.Landscape;
        section.PageSetup.TopMargin = Unit.FromCentimeter(1);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

        var table = section.AddTable();
        table.Borders.Width = 0.75;

        var widths = new[] { "1.5cm", "3cm", "3cm", "3cm", "3cm", "3cm", "3cm", "3cm", "3cm"};
        foreach (var w in widths)
            table.AddColumn(w);

        var headerRow = table.AddRow();
        headerRow.Shading.Color = MigraDoc.DocumentObjectModel.Colors.LightGray;
        headerRow.Format.Font.Bold = true;
        headerRow.Cells[0].AddParagraph("Id");
        headerRow.Cells[1].AddParagraph("FirstName");
        headerRow.Cells[2].AddParagraph("LastName");
        headerRow.Cells[3].AddParagraph("Phone");
        headerRow.Cells[4].AddParagraph("PersonnelNo");
        headerRow.Cells[5].AddParagraph("CityId");
        headerRow.Cells[6].AddParagraph("City");
        headerRow.Cells[7].AddParagraph("GroupId");
        headerRow.Cells[8].AddParagraph("Group");

        foreach (var u in users)
        {
            var row = table.AddRow();
            row.Cells[0].AddParagraph(u.Id.ToString());
            row.Cells[1].AddParagraph(u.FirstName ?? "");
            row.Cells[2].AddParagraph(u.LastName ?? "");
            row.Cells[3].AddParagraph(u.PhoneNumber ?? "");
            row.Cells[4].AddParagraph(u.PersonnelNumber ?? "");
            row.Cells[5].AddParagraph(u.CityId.ToString() ?? "");
            row.Cells[6].AddParagraph(new string(u.City.Name.Reverse().ToArray()) ?? "");
            row.Cells[7].AddParagraph(u.GroupID.ToString() ?? "");
            if (u.GroupID != null)
                row.Cells[8].AddParagraph(new string(u.Group.Name.Reverse().ToArray()) ?? "");
            else
                row.Cells[8].AddParagraph("");
        }

        var pdfRenderer = new PdfDocumentRenderer(unicode: true)
        {
            Document = doc
        };
        pdfRenderer.RenderDocument();

        using var ms = new MemoryStream();
        pdfRenderer.PdfDocument.Save(ms);
        return ms.ToArray();
    }
}
