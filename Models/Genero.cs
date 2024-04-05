using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Genero
    {
        [Key]
        public long? GeneroID { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O campo Estilo Narrativo é obrigatório.")]
        public string? EstiloNarrativo { get; set; }

        [Required(ErrorMessage = "O campo Público Alvo é obrigatório.")]
        public string? PublicoAlvo { get; set; }

    }
}
