using System.ComponentModel.DataAnnotations;

namespace Interface.FiltersInterface.FactureFilterInterface;

public class SortingFactureValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string[] sortArray)
        {
            return new ValidationResult("The sort parameter must be a string array.");
        }

        var optionExtension = (ISortingFactureExtension)validationContext.GetService(typeof(ISortingFactureExtension)) ?? throw new ValidationException("ISortingFactureExtension service is not available.");
        var sortingOpritons = optionExtension.GetSortingFactureOptions();
        foreach (var sortValue in sortArray)
        {
            if (!sortingOpritons.Contains(sortValue, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult($"Invalid sort value: {sortValue}. Allowed values are: {string.Join(", ", sortingOpritons)}");
            }
        }

        return ValidationResult.Success;
    }
}
