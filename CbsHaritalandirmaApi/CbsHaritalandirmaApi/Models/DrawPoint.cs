using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace CbsHaritalandirmaApi.Models
{
    [Table("table_point")]
    public class DrawPoint
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("wkt")]
        public string Wkt { get; set; } = string.Empty;

        [JsonIgnore]
        [Column("geom")]
        public Point Geom { get; set; } = null!;

        [Column("color")]
        public string? Color { get; set; } = "#2563eb";

        [Column("description")]
        public string? Description { get; set; }

        [Column("object_count")]
        public int ObjectCount { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("is_delete")]
        public bool IsDelete { get; set; } = false;
    }
}