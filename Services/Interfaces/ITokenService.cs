namespace Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userName);
    }
}
