using System.ComponentModel.DataAnnotations;
using Bussines.Dto.Adress;

namespace Bussines.Dto.Company;

public class CompanyPost
{
    [Required]
    public string Name { get; set; }

    [Required]
    public long NIP
    {
        get; set;
    }

    public AddressPost Address { get; set; }
}
