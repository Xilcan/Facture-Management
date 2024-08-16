using System.ComponentModel.DataAnnotations;

namespace Bussines.Dto.Company;

public class BriefCompanyGet
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public long NIP
    {
        get; set;
    }
}
