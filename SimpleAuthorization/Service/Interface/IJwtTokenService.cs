namespace SimpleAuthorization.Service.Interface
{
    public interface IJwtTokenService
    {
        string GenerateToken(string email, string role);
    }
}
