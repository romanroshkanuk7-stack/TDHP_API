namespace TDHP_API.DTOs.Program
{
    public class ProgramDto
    {
        public Guid Id { get; set; }
        public string DateLine1 { get; set; } = string.Empty;
        public string DateLine2 { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int SortIndex { get; set; }
    }
}
