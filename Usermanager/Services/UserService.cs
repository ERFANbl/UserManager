using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Usermanager.Model.DBContext;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;
using Usermanager.Interfaces;

namespace Usermanager.Services
{
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
                .Include(u => u.Province)
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
                    Province = u.Province.Name,
                    City = u.City.Name
                })
                .ToList();
        }

        public bool CreateUser(User user)
        {
            _dbcontext.Users.Add(user);
            _dbcontext.SaveChanges();

            var u = _dbcontext.Users
                   .Include(x => x.Province)
                   .Include(x => x.City)
                   .First(x => x.Id == user.Id);

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
                .Include(u => u.Province)
                .Include(u => u.City)
                .Where(u => u.ProvinceId == provinceId && u.CityId == cityId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Province = u.Province.Name,
                    City = u.City.Name
                })
                .ToList();
        }
    }
}
