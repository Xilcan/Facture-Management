using System.ComponentModel.DataAnnotations;
using Bussines.Dto.Company;

namespace Bussines.Dto.AutenticationDto;

public class RegisterVM
{
    [Required(ErrorMessage = "Company is required")]
    public CompanyPost Company { get; set; }

    [Required(ErrorMessage = "EmailAddress cannot be null")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "UserName cannot be null")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password cannot be null")]
    public string Password { get; set; }
}
