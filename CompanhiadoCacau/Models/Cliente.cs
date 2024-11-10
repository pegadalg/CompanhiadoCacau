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
        [DataNascimentoNaoPodeSerFutura(ErrorMessage = "A data de nascimento não pode ser maior que a data atual.")]
        public DateOnly DataNascimento { get; set; }

        [Required]
        [Cpf]
        public string CPF { get; set; }

        [Required]
        [Email(ErrorMessage = "O e-mail fornecido não é válido.")]
        public string Email { get; set; }

        [Telefone(ErrorMessage = "O número de telefone fornecido não é válido.")]
        public string Telefone { get; set; }

       
        public Endereco? Endereco { get; set; }

        [ForeignKey("Endereco")]
        public int EnderecoId { get; set; }       

        public List<Pedido>? PedidosCliente { get; set; }
    }

}
