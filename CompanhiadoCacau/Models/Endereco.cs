using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.RegularExpressions;

namespace CompanhiadoCacau.Models
{
    public class Endereco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEndereco { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "O formato do CEP deve ser 12345-678 ou 12345678.")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        public string Logradouro { get; set; }

        public string? Complemento { get; set; }

        [MaxLength(100, ErrorMessage = "O complemento não pode ter mais de 100 caracteres.")]
        public string? ComplementoValidado
        {
            get { return Complemento; }
            set { Complemento = value?.Length > 100 ? value.Substring(0, 100) : value; }
        }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A localidade é obrigatória.")]
        public string Localidade { get; set; }

        [Required(ErrorMessage = "A UF é obrigatória.")]
        [StringLength(2, ErrorMessage = "A UF deve ter 2 caracteres.")]
        public string UF { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public int Numero { get; set; }
    }
}
