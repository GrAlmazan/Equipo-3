namespace Project.Domain.Entities;

public class User
{
    public long UserID { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public long UserRolID { get; set; }
}