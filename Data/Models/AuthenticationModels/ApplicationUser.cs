using Microsoft.AspNetCore.Identity;

namespace Data.Models.AuthenticationModels;

public class ApplicationUser : IdentityUser
{
    public Guid CompanyId { get; set; }

    public string Custom { get; set; } = string.Empty;
}
