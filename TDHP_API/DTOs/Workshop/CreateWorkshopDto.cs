using System.ComponentModel.DataAnnotations;
namespace TDHP_API.DTOs.Workshop
{
    public class CreateWorkshopDto
    {
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Image { get; set; } = string.Empty;
        [Required] public string ToId { get; set; } = string.Empty;
        public bool IsPlaceholder { get; set; } = false;
        public string Description { get; set; } = string.Empty;
    }
}
