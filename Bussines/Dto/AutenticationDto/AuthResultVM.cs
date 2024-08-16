namespace Bussines.Dto.AutenticationDto;

public class AuthResultVM
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public Guid CompanyId { get; set; }

    public DateTime ExpiresAt { get; set; }
}
