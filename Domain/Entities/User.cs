namespace Domain.Entities;

public class User
{
    public int Id { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }

    public User(string username, string passwordHash, string role)
    {
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }
}
