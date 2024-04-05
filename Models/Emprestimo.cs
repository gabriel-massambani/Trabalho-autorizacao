using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Emprestimo
    {
        [Key]
        public long? EmprestimoID { get; set; }

        [Required(ErrorMessage = "O campo Data Empréstimo é obrigatório.")]
        public DateTime? DataEmprestimo { get; set; }

        [Required(ErrorMessage = "O campo Data Devolução é obrigatório.")]
        public DateTime? DataDevolucao { get; set; }

        [Required(ErrorMessage = "O campo Livro é obrigatório.")]
        [ForeignKey("Livro")]
        public long? fk_LivroID { get; set; }
        public Livro? Livro { get; set; }

        [Required(ErrorMessage = "O campo Usuário é obrigatório.")]
        [ForeignKey("Usuario")]
        public long? fk_UsuarioID { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
