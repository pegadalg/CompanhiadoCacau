using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanhiadoCacau.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PedidoId { get; set; }

        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor Total deve ser maior que zero.")]
        public decimal ValorTotal { get; set; }

        [Required]
        public bool Status { get; set; }

        // Relacionamento com produtos por meio da tabela intermediária PedidoProduto
        public List<PedidoProduto>? PedidoProdutos { get; set; } = new List<PedidoProduto>();
    }
}
