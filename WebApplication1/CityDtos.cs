using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class CityResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static CityResponse FromModel(City city) => new()
        {
            Id = city.Id,
            Name = city.Name
        };
    }

    public class CreateCityRequest
    {
        public string Name { get; set; } = string.Empty;

        public City ToModel() => new()
        {
            Name = Name
        };
    }

    public class UpdateCityRequest
    {
        public string Name { get; set; } = string.Empty;

        public void ApplyTo(City city)
        {
            city.Name = Name;
        }
    }
}