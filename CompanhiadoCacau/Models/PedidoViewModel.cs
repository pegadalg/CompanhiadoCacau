using System.ComponentModel.DataAnnotations;

namespace CompanhiadoCacau.Models
{
    public class PedidoViewModel
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ResponsavelAtendimento { get; set; }

        // Lista de produtos a serem adicionados ao pedido
        public List<ProdutoPedidoViewModel> Produtos { get; set; }
    }
}
