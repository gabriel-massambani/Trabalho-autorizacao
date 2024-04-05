using Biblioteca.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data
{
    public class IESContext : IdentityDbContext
    {
        public IESContext(DbContextOptions<IESContext> options) : base(options) 
        { 
        }

        public DbSet<Autor> Autor { get; set; }
        public DbSet<Emprestimo> Emprestimo { get; set; }
        public DbSet<Genero> Genero { get; set; }
        public DbSet<Livro> Livro { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<ApplicationUser> users { get; set; }

    }
}
