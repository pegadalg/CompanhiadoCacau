using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace CompanhiadoCacau.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        public DateOnly DataNascimento { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string CPF { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Telefone { get; set; }

        [Required]
        public Endereco? Endereco { get; set; }

        [ForeignKey("Endereco")]
        public int EnderecoId { get; set; }       

        public List<Pedido>? PedidosCliente { get; set; }

        public bool IsCpfValid()
        {
            if (string.IsNullOrWhiteSpace(CPF))
            {
                return false; // CPF é obrigatório
            }

            // Remove caracteres não numéricos
            string cpf = Regex.Replace(CPF, @"[^\d]", "");

            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11 || !long.TryParse(cpf, out _))
            {
                return false;
            }

            // Validação dos dígitos verificadores
            return ValidateCpfDigits(cpf);
        }

        private bool ValidateCpfDigits(string cpf)
        {
            int[] cpfArray = new int[11];
            for (int i = 0; i < 11; i++)
            {
                cpfArray[i] = int.Parse(cpf[i].ToString());
            }

            // Valida o primeiro dígito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += cpfArray[i] * (10 - i);
            }
            int remainder = (sum * 10) % 11;
            if (remainder == 10 || remainder == 11)
                remainder = 0;

            if (remainder != cpfArray[9])
                return false;

            // Valida o segundo dígito verificador
            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += cpfArray[i] * (11 - i);
            }
            remainder = (sum * 10) % 11;
            if (remainder == 10 || remainder == 11)
                remainder = 0;

            return remainder == cpfArray[10];
        }



    }

}
