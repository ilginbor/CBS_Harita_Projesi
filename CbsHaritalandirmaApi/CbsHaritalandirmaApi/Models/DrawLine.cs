using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace CbsHaritalandirmaApi.Models
{
    [Table("table_line")]
    public class DrawLine
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
        public LineString Geom { get; set; } = null!;

        [Column("color")]
        public string? Color { get; set; } = "#16a34a";

        [Column("description")]
        public string? Description { get; set; }

        [Column("object_count")]
        public int ObjectCount { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("is_delete")]
        public bool IsDelete { get; set; } = false;
    }
}