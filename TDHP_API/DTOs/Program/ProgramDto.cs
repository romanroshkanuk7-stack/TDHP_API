namespace TDHP_API.DTOs.Program
{
    public class ProgramDto
    {
        public Guid Id { get; set; }
        public string DateLine1 { get; set; } = string.Empty;
        public string DateLine2 { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string? Time { get; set; }
        public string? Location { get; set; }
        public string? ButtonLink { get; set; }
        public int SortIndex { get; set; }
    }
}
