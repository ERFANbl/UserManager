using Usermanager.Interfaces;
using Usermanager.Model.DBContext;
using Usermanager.Model.DTO;

namespace Usermanager.Services;

public class LocationService : ILocationService
{
    private readonly ApplicationDbContext _context;

    public LocationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ProvinceDto> GetProvinces()
    {
        return _context.Provinces
                       .Select(p => new ProvinceDto { Id = p.Id, Name = p.Name })
                       .ToList();
    }

    public List<CityDto> GetCitiesByProvinceId(int provinceId)
    {
        return _context.Cities
                       .Where(c => c.ProvinceId == provinceId)
                       .Select(c => new CityDto { Id = c.Id, Name = c.Name })
                       .ToList();
    }
}

