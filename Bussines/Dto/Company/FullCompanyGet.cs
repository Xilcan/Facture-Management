using System.ComponentModel.DataAnnotations;
using Bussines.Dto.Adress;

namespace Bussines.Dto.Company;
public class FullCompanyGet
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public long NIP
    {
        get; set;
    }

    public AddressGet Address { get; set; }
}
