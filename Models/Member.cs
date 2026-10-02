namespace LibraryAPI.Models;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RfidValue { get; set; } = string.Empty;
}
