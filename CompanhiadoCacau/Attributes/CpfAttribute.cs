using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class CpfAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var cpf = value as string;

        if (string.IsNullOrWhiteSpace(cpf))
        {
            return new ValidationResult("CPF é obrigatório.");
        }

        // Remove todos os caracteres não numéricos
        cpf = Regex.Replace(cpf, @"[^\d]", "");

        // Verifica se o CPF tem 11 dígitos
        if (cpf.Length != 11 || !long.TryParse(cpf, out _))
        {
            return new ValidationResult("CPF inválido.");
        }

        // Verifica se o CPF é um número "famoso" (como 111.111.111-11, 222.222.222-22, etc.)
        if (cpf.All(c => c == cpf[0]))
        {
            return new ValidationResult("CPF inválido.");
        }

        // Valida os dígitos verificadores
        if (!ValidateCpfDigits(cpf))
        {
            return new ValidationResult("CPF inválido.");
        }

        return ValidationResult.Success;
    }

    private bool ValidateCpfDigits(string cpf)
    {
        int[] cpfArray = new int[11];
        for (int i = 0; i < 11; i++)
        {
            cpfArray[i] = int.Parse(cpf[i].ToString());
        }

        // Validação do primeiro dígito verificador
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += cpfArray[i] * (10 - i);
        }
        int remainder = (sum * 10) % 11;
        if (remainder == 10 || remainder == 11) remainder = 0;

        if (remainder != cpfArray[9]) return false;

        // Validação do segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += cpfArray[i] * (11 - i);
        }
        remainder = (sum * 10) % 11;
        if (remainder == 10 || remainder == 11) remainder = 0;

        return remainder == cpfArray[10];
    }
}
