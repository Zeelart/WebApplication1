using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebApplication1.Models
{
    [Table("cities")]
    public class City : BaseModel
    {
        [PrimaryKey("id", true)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }
}