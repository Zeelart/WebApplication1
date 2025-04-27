using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebApplication1.Models
{
    [Table("users")]
    public class AppUser : BaseModel
    {
        [PrimaryKey("id", true)]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("login")]
        public string Login { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("mail")]
        public string? Mail { get; set; }

        [Column("city_id")]
        public int CityId { get; set; }
    }
}