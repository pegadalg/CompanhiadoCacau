using System;
using System.ComponentModel.DataAnnotations;

public class DataNascimentoNaoPodeSerFuturaAttribute : ValidationAttribute
{
    public DataNascimentoNaoPodeSerFuturaAttribute() 
        : base("A data de nascimento não pode ser maior que a data atual.")
    {
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateOnly dataNascimento)
        {
            // Compara a data de nascimento com a data de hoje
            if (dataNascimento > DateOnly.FromDateTime(DateTime.Now))
            {
                return new ValidationResult("A data de nascimento não pode ser maior que a data atual.");
            }
            return ValidationResult.Success;
        }
        
        return new ValidationResult("Data de nascimento inválida.");
    }
}
