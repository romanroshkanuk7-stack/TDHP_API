using System.ComponentModel.DataAnnotations;

namespace TDHP_API.DTOs.Program
{
    public class CreateProgramDto
    {
        [Required] public string DateLine1 { get; set; } = string.Empty;
        [Required] public string DateLine2 { get; set; } = string.Empty;
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Image { get; set; } = string.Empty;
        public int? SortIndex { get; set; }
    }
}
