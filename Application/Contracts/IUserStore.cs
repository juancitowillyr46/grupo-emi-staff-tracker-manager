using Domain.Entities;

namespace Application.Contracts;

public interface IUserStore
{
    Task<bool> RegisterAsync(string username, string password, string role);
    Task<User?> ValidateAsync(string username, string password);
}
