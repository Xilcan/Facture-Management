using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Interface.FiltersInterface.PagingFilterInterface;

public class PagingDtoOption
{
    [JsonPropertyName("page")]
    [Range(0, int.MaxValue, ErrorMessage = "Page Must be greater then 0")]
    public int? Page { get; set; }

    [JsonPropertyName("pageCount")]
    [PagingValidator]
    public string? PageCount { get; set; }
}
