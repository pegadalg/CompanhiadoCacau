using CompanhiadoCacau.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public StatusPedido Status { get; set; }  // Enum aqui

        public DateTime DataPedido { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string ResponsavelAtendimento { get; set; }

        public List<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();
    }

    public enum StatusPedido
    {
        Pendente = 0,
        EmAndamento = 1,
        Finalizado = 2,
        Cancelado = 3
    }

}