using System.ComponentModel.DataAnnotations;

namespace Interface.FiltersInterface.PagingFilterInterface;
public class PagingValidator : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            // If value is null, consider it valid
            return ValidationResult.Success;
        }

        var optionExtension = (IPagingOptionExtension)validationContext.GetService(typeof(IPagingOptionExtension)) ?? throw new InvalidOperationException("IPagingOptionExtension service is not available.");
        var limitOptions = optionExtension.GetStringOptions();
        return value is string durationStr && limitOptions.Contains(durationStr)
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage ?? "Invalid PageCount.");
    }
}
