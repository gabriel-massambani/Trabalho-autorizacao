using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class LivroController : Controller
    {
        private readonly IESContext _context;

        public LivroController(IESContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var livros = await _context.Livro
                .Include(i => i.Autor)
                .Include(i => i.Genero)
                .OrderBy(d => d.Titulo)
                .ToListAsync();
            return View(livros);
        }

        //GET: Livro/CREATE
        public IActionResult Create()
        {
            var autores = _context.Autor.OrderBy(i => i.Nome).ToList();
            autores.Insert(0, new Autor()
            {
                AutorID = 0,
                Nome = "Selecione o autor(a)"
            });

            ViewBag.Autores = autores;

            var generos = _context.Genero.OrderBy(i => i.Nome).ToList();
            generos.Insert(0, new Genero()
            {
                GeneroID = 0,
                Nome = "Selecione o gênero"
            });

            ViewBag.Generos = generos;

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Livro livro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(livro);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "Não foi possível inserir os dados.");
            }
            return View(livro);
        }

        //GET: Livro/UPDATE

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro.SingleOrDefaultAsync(m => m.LivroID == id);

            if (livro == null)
            {
                return NotFound();
            }

            ViewBag.Autores = new SelectList(_context.Autor.OrderBy(b => b.Nome),
                "AutorID", "Nome", livro.fk_AutorID);

            ViewBag.Generos = new SelectList(_context.Genero.OrderBy(b => b.Nome),
                "GeneroID", "Nome", livro.fk_GeneroID);

            return View(livro);
        }

        private bool LivroExists(long? id)
        {
            return _context.Livro.Any(e => e.LivroID == id);
        }

        //POST: Livro/UPDATE
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(long? id, Livro livro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!LivroExists(livro.LivroID))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livro);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro.SingleOrDefaultAsync(m => m.LivroID == id);

            _context.Autor.Where(i => livro.fk_AutorID == i.AutorID).Load();
            _context.Genero.Where(i => livro.fk_GeneroID == i.GeneroID).Load();

            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        //GET: Livro/DELETE
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro.SingleOrDefaultAsync(d => d.LivroID == id);

            _context.Autor.Where(i => livro.fk_AutorID == i.AutorID).Load();
            _context.Genero.Where(i => livro.fk_GeneroID == i.GeneroID).Load();

            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        //POST: Livro/DELETE

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var livro = await _context.Livro.SingleOrDefaultAsync(
                m => m.LivroID == id);

            _context.Livro.Remove(livro);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Livro {livro.Titulo.ToUpper()} foi removido";

            return RedirectToAction(nameof(Index));
        }


    }
}
