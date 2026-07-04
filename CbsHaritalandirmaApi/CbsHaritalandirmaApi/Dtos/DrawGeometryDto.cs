namespace CbsHaritalandirmaApi.Dtos
{
    public class DrawGeometryDto
    {
        public int? UserId { get; set; }

        public string? Name { get; set; }

        public string? Color { get; set; }

        public string? Description { get; set; }

        public string GeometryType { get; set; } = string.Empty;

        public string Wkt { get; set; } = string.Empty;
    }
}