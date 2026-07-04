using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CbsHaritalandirmaApi.Models
{
    [Table("map_locations")]
    public class MapLocation
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        [Column("wkt")]
        public string? Wkt { get; set; }

        [Column("geom")]
        [JsonIgnore]
        public Point? Geom { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}