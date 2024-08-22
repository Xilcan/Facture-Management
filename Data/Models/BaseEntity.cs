using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
