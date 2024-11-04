namespace CompanhiadoCacau.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PedidoId { get; set; }

        [Required]
        public Cliente? Cliente { get; set; }

        [Required]
        public List<Produto>? Produtos { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        [Required]
        public Endereco EnderecoEntrega { get; set; }

        [Required]
        public bool Status { get; set; }

        public List<PedidoProduto>? PedidoProdutos { get; set; }
        public int EnderecoId { get; set; }
    }

}
