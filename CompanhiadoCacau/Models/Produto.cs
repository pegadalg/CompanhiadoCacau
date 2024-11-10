using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanhiadoCacau.Models
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProdutoId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do produto deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
        [MaxLength(50, ErrorMessage = "O nome da categoria não pode ter mais de 50 caracteres.")]
        public string Categoria { get; set; }


        public List<PedidoProduto>? PedidoProdutos { get; set; }
    }
}
