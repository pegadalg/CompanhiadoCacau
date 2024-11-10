using CompanhiadoCacau.Controllers;
using CompanhiadoCacau.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Xunit;
using System.Threading.Tasks;
using CompanhiadoCacau.Data;

namespace CompanhiadoCacauTest;


public class ClassesTest
{
    [Fact]
    public void Criar_Cliente_Valido()
    {
        // Arrange - Criando um cliente válido
        var cliente = new Cliente
        {
            Nome = "João da Silva",
            //DataNascimento = new DateOnly(1990, 5, 20),
            DataNascimento = new DateOnly(2024, 11, 10),
            CPF = "14264273790",  // CPF válido
            Email = "joao.silva@email.com", // E-mail válido
            Telefone = "11987654321",  // Telefone válido
            EnderecoId = 1
        };

        // Act - Validando o cliente com as anotações de dados
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(cliente, serviceProvider: null, items: null);
        var isValid = Validator.TryValidateObject(cliente, validationContext, validationResults, true);

        // Assert - Verificando que o cliente é válido
        Assert.True(isValid);  // O cliente deve ser válido

        // Se o teste falhar, vamos imprimir os erros de validação
        if (!isValid)
        {
            Console.WriteLine("Erros de validação:");
            foreach (var validationResult in validationResults)
            {
                Console.WriteLine(validationResult.ErrorMessage);  // Imprime as mensagens de erro
            }
        }
    }


    [Fact]
    public void CriarCliente_NomeInvalido_ErroValidacao()
    {
        // Arrange - Criando um cliente com nome inválido (nulo ou vazio)
        var cliente = new Cliente
        {
            Nome = "",  // Nome vazio
            DataNascimento = new DateOnly(1990, 5, 20),
            CPF = "123.456.789-00",  // CPF válido
            Email = "joao.silva@email.com",
            Telefone = "(11) 98765-4321",
            EnderecoId = 1
        };

        // Act - Validando o cliente com as anotações de dados
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(cliente, serviceProvider: null, items: null);
        var isValid = Validator.TryValidateObject(cliente, validationContext, validationResults, true);

        // Assert - Verificando que o cliente não é válido devido ao nome
        Assert.False(isValid);  // O cliente não deve ser válido
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Nome"));  // Erro relacionado ao campo Nome

    }

    [Fact]
    public void CriarCliente_CPFInvalido_ErroValidacao()
    {
        // Arrange - Criando um cliente com CPF inválido
        var cliente = new Cliente
        {
            Nome = "João da Silva",
            DataNascimento = new DateOnly(1990, 5, 20),
            CPF = "000.000.000-00",  // CPF inválido
            Email = "joao.silva@email.com",
            Telefone = "(11) 98765-4321",
            EnderecoId = 1
        };

        // Act - Validando o cliente com as anotações de dados
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(cliente, serviceProvider: null, items: null);
        var isValid = Validator.TryValidateObject(cliente, validationContext, validationResults, true);

        // Assert - Verificando que o CPF é inválido
        Assert.False(isValid);  // O cliente não deve ser válido
                                // Verifique diretamente se a mensagem de erro de CPF está presente
        Assert.Contains(validationResults, vr => vr.ErrorMessage.Contains("CPF inválido"));
    }

    [Fact]
    public void CriarCliente_EmailInvalido_ErroValidacao()
    {
        // Arrange - Criando um cliente com e-mail inválido
        var cliente = new Cliente
        {
            Nome = "João da Silva",
            DataNascimento = new DateOnly(1990, 5, 20),
            CPF = "123.456.789-00",  // CPF válido
            Email = "invalidemail.com", // E-mail inválido
            Telefone = "(11) 98765-4321",  // Telefone válido
            EnderecoId = 1
        };

        // Act - Validando o cliente com as anotações de dados
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(cliente, serviceProvider: null, items: null);
        var isValid = Validator.TryValidateObject(cliente, validationContext, validationResults, true);

        // Assert - Verificando que o e-mail é inválido
        Assert.False(isValid);  // O cliente não deve ser válido
                                // Verifique diretamente a mensagem de erro do Email, ajustando para a mensagem correta
        Assert.Contains(validationResults, vr => vr.ErrorMessage.Contains("E-mail inválido."));
    }

    // Outros testes podem ser adicionados conforme necessário, como para Telefone, DataNascimento, etc.
}