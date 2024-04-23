using Equilobe.Core.Shared.SeedWork;

namespace Equilobe.Core.Features.Users;

public class User : Entity
{
    public string Username { get; private set; }
    public UserRole Role { get; private set; }

    public User(string username, UserRole role) : base()
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Role = role;
    }
}
