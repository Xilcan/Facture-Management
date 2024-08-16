using System.ComponentModel.DataAnnotations;

namespace Bussines.Dto.AutenticationDto;

public class LoginVM
{
    [Required(ErrorMessage = "EmailAddress cannot be null")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Password cannot be null")]
    public string Password { get; set; }
}
