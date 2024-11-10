using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class TelefoneAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var telefone = value as string;

        if (string.IsNullOrWhiteSpace(telefone))
        {
            return new ValidationResult("Telefone é obrigatório.");
        }

        // Expressão regular para validar o formato exato: 11987654321
        var telefoneRegex = new Regex(@"^\d{11}$");

        if (!telefoneRegex.IsMatch(telefone))
        {
            return new ValidationResult("Telefone inválido. O formato correto é 11987654321.");
        }

        return ValidationResult.Success;
    }
}
