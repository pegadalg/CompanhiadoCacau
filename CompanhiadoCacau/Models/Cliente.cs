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
        [Cpf]
        public string CPF { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Telefone { get; set; }

       
        public Endereco? Endereco { get; set; }

        [ForeignKey("Endereco")]
        public int EnderecoId { get; set; }       

        public List<Pedido>? PedidosCliente { get; set; }
    }

}
