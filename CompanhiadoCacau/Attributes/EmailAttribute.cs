using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class EmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var email = value as string;

        if (string.IsNullOrWhiteSpace(email))
        {
            return new ValidationResult("E-mail é obrigatório.");
        }

        // Expressão regular para validar o formato do e-mail
        var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

        if (!emailRegex.IsMatch(email))
        {
            return new ValidationResult("E-mail inválido.");
        }

        return ValidationResult.Success;
    }
}
