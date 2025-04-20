using Usermanager.Model.DTO;

namespace Usermanager.Interfaces;

public interface ILocationService
{
    List<ProvinceDto> GetProvinces();
    List<CityDto> GetCitiesByProvinceId(int provinceId);
}

