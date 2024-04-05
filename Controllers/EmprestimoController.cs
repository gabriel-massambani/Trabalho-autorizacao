using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class EmprestimoController : Controller
    {
        private readonly IESContext _context;

        public EmprestimoController(IESContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var emprestimos = await _context.Emprestimo
                .Include(i => i.Livro)
                .Include(i => i.Usuario)
                .OrderBy(d => d.DataEmprestimo)
                .ToListAsync();
            return View(emprestimos);
        }

        //GET: Emprestimo/CREATE
        public IActionResult Create()
        {
            var livros = _context.Livro.OrderBy(i => i.Titulo).ToList();
            livros.Insert(0, new Livro()
            {
                LivroID = 0,
                Titulo = "Selecione o livro"
            });

            ViewBag.Livros = livros;

            var usuarios = _context.Usuario.OrderBy(i => i.Nome).ToList();
            usuarios.Insert(0, new Usuario()
            {
                UsuarioID = 0,
                Nome = "Selecione o(a) usuário(a)"
            });

            ViewBag.Usuarios = usuarios;

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Emprestimo emprestimo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(emprestimo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "Não foi possível inserir os dados.");
            }
            return View(emprestimo);
        }

        //GET: Emprestimo/UPDATE

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimo.SingleOrDefaultAsync(m => m.EmprestimoID == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            ViewBag.Livros = new SelectList(_context.Livro.OrderBy(b => b.Titulo),
                "LivroID", "Nome", emprestimo.fk_LivroID);

            ViewBag.Usuarios = new SelectList(_context.Usuario.OrderBy(b => b.Nome),
                "UsuarioID", "Nome", emprestimo.fk_UsuarioID);

            return View(emprestimo);
        }

        private bool EmprestimoExists(long? id)
        {
            return _context.Emprestimo.Any(e => e.EmprestimoID == id);
        }

        //POST: Emprestimo/UPDATE
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(long? id, Emprestimo emprestimo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprestimo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!EmprestimoExists(emprestimo.EmprestimoID))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(emprestimo);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimo.SingleOrDefaultAsync(m => m.EmprestimoID == id);

            _context.Livro.Where(i => emprestimo.fk_LivroID == i.LivroID).Load();
            _context.Usuario.Where(i => emprestimo.fk_UsuarioID == i.UsuarioID).Load();

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        //GET: Emprestimo/DELETE
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimo.SingleOrDefaultAsync(d => d.EmprestimoID == id);

            _context.Livro.Where(i => emprestimo.fk_LivroID == i.LivroID).Load();
            _context.Usuario.Where(i => emprestimo.fk_UsuarioID == i.UsuarioID).Load();

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        //POST: Emprestimo/DELETE

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var emprestimo = await _context.Emprestimo.SingleOrDefaultAsync(
                m => m.EmprestimoID == id);

            _context.Emprestimo.Remove(emprestimo);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Emprestimo {emprestimo.DataEmprestimo} foi removido";

            return RedirectToAction(nameof(Index));
        }


    }
}
