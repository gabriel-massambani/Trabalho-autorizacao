using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Autor
    {
        [Key]
        public long? AutorID { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo Nacionalidade é obrigatório.")]
        public string? Nacionalidade { get; set; }

        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo Biografia é obrigatório.")]
        public string? Biografia { get; set; }

    }
}
