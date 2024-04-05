using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class GeneroController : Controller
    {


        private readonly IESContext _context;

        public GeneroController(IESContext context)
        {
            this._context = context;
        }

        // GET LIST
        public async Task<IActionResult> Index()
        {
            var generos = await _context.Genero
                .OrderBy(d => d.Nome)
                .ToListAsync();
            return View(generos);

        }

        //Get: Genero/Create
        public IActionResult Create()
        {
            var generos = _context.Genero.OrderBy(i => i.Nome).ToList();
            generos.Insert(0, new Genero()
            {
                GeneroID = 0,
                Nome = "Nome genero",
                Descricao = "Desconhecida",
                EstiloNarrativo = "Desconhecida",
                PublicoAlvo = "Sem informação"
            });

            ViewBag.Generos = generos;
            return View();
        }


        //POST : Genero/Create

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Genero genero)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(genero);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "não foi possivel inserir os dados.");
            }
            return View(genero);
        }



        //GET: Genero/Update

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Genero.SingleOrDefaultAsync
                (m => m.GeneroID == id);

            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }


        private bool GeneroExists(long? id)
        {
            return _context.Genero.Any(e => e.GeneroID == id);
        }

        //POST: Genero/Update
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(long? id, Genero genero)

        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!GeneroExists(genero.GeneroID))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            return View(genero);


        }

        //GET: Genero/Details

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Genero.SingleOrDefaultAsync
                (m => m.GeneroID == id);


            if (genero == null)
            {
                return NotFound();
            }
            return View(genero);
        }

        //GET: Genero/Delete

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Genero.
                SingleOrDefaultAsync(d => d.GeneroID == id);


            if (genero == null)
            {
                return NotFound();
            }
            return View(genero);
        }

        //POST: Genero/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genero = await _context.Genero.FindAsync(id);

            if (genero == null)
            {
                return NotFound();
            }

            try
            {
                _context.Genero.Remove(genero);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Genero {genero.Nome.ToUpper()} foi removido";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Adicione manipulação específica de exceção aqui, se necessário
                // Por exemplo, você pode querer adicionar um erro ao ModelState.
                ModelState.AddModelError("Erro", "Não foi possível remover o genero. Verifique se não há dependências.");

                // Pode ser útil para depurar o erro
                Console.WriteLine(ex.Message);
                return View("Delete", genero);
            }
        }



    }
}
