namespace TDHP_API.DTOs.Group
{
    public class GroupDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
    public class CreateGroupDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
