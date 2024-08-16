using System.ComponentModel.DataAnnotations;

namespace Bussines.Dto.AutenticationDto;

public class TokenRequestVM
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }
}
