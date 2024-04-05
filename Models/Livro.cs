using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Livro
    {
        [Key]
        public long? LivroID { get; set; }

        [Required(ErrorMessage = "O campo Titulo é obrigatório.")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "O campo Editora é obrigatório.")]
        public string? Editora { get; set; }

        [Required(ErrorMessage = "O campo Ano de Puclicação é obrigatório.")]
        public long? AnoPublicacao { get; set; }

        [Required(ErrorMessage = "O campo Resumo é obrigatório.")]
        public string? Resumo { get; set; }

        [Required(ErrorMessage = "O campo Autor é obrigatório.")]
        [ForeignKey("Autor")]
        public long? fk_AutorID { get; set; }
        public Autor? Autor { get; set; }

        [Required(ErrorMessage = "O campo Genero é obrigatório.")]
        [ForeignKey("Genero")]
        public long? fk_GeneroID { get; set; }
        public Genero? Genero { get; set; }


    }
}
