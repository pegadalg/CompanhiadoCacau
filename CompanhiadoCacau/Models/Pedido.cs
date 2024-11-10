using CompanhiadoCacau.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
        public StatusPedido Status { get; set; } = StatusPedido.Pendente;  // Enum aqui

        [DisplayName("Data do Pedido")]
        public DateOnly DataPedido { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        [DisplayName("Responsável Atendimento")]
        public string ResponsavelAtendimento { get; set; }

        public List<PedidoProduto>? PedidoProdutos { get; set; } = new List<PedidoProduto>();
    }

    public enum StatusPedido
    {
        Pendente = 0,
        EmAndamento = 1,
        Finalizado = 2,
        Cancelado = 3
    }

}