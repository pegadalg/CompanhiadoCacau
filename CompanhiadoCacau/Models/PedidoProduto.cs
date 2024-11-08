using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanhiadoCacau.Models
{
    public class PedidoProduto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PedidoProdutoId { get; set; } 

        [Required]
        public int PedidoId { get; set; } 

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; } 

        [Required]
        public int ProdutoId { get; set; } 

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; } 

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }
    }
}
