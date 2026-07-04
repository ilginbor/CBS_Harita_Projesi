namespace CbsHaritalandirmaApi.Dtos
{
    public class DrawGeometryResponseDto
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string? Name { get; set; }

        public string? Color { get; set; }

        public string? Description { get; set; }

        public int ObjectCount { get; set; }

        public string GeometryType { get; set; } = string.Empty;

        public string Wkt { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }
    }
}