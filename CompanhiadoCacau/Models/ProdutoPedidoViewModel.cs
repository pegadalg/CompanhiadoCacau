using System.ComponentModel.DataAnnotations;

namespace CompanhiadoCacau.Models
{
    public class ProdutoPedidoViewModel
    {
        public int ProdutoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }
    }
}
