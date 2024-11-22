using Shopping.DAL;

namespace Shopping.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(User customer);
}
