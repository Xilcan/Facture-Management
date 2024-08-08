using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class CompanyAddress : Address
{
    [Required]
    public Guid CompanyId { get; set; }

    public virtual Company Company { get; set; }
}
