using WebApplication1.Models;

namespace WebApplication1.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string Login { get; set; } = string.Empty;
        public string? Mail { get; set; }
        public int CityId { get; set; }

        public static UserResponse FromModel(AppUser user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Age = user.Age,
            Login = user.Login,
            Mail = user.Mail,
            CityId = user.CityId
        };
    }

    public class CreateUserRequest
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Mail { get; set; }
        public int CityId { get; set; }

        public AppUser ToModel() => new()
        {
            Name = Name,
            Age = Age,
            Login = Login,
            Password = Password,
            Mail = Mail,
            CityId = CityId
        };
    }

    public class UpdateUserRequest
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Mail { get; set; }
        public int CityId { get; set; }

        public void ApplyTo(AppUser user)
        {
            user.Name = Name;
            user.Age = Age;
            user.Mail = Mail;
            user.CityId = CityId;
        }
    }
}