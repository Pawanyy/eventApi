using eventApi.Models;

namespace eventApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);

    }
}