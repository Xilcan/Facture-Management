using System.ComponentModel.DataAnnotations;

namespace Bussines.Dto.Adress;
public class AddressPost
{
    [Required]
    public string Country { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Street { get; set; }

    [Required]
    public string HouseNumber { get; set; }

    public string? LocalNumber { get; set; }

    [Required]
    public string PosteCode { get; set; }
}
