public interface IAuthService
{
    string GenerateJwtToken(Guid Id, string role);
}