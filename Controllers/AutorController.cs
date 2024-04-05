using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class AutorController : Controller
    {


        private readonly IESContext _context;

        public AutorController(IESContext context)
        {
            this._context = context;
        }


        // GET LIST
        public async Task<IActionResult> Index()
        {
            var autores = await _context.Autor
                .OrderBy(d => d.Nome)
                .ToListAsync();
            return View(autores);

        }

        //Get: Autor/Create
        public IActionResult Create()
        {
            var autores = _context.Autor.OrderBy(i => i.Nome).ToList();
            autores.Insert(0, new Autor()
            {
                AutorID = 0,
                Nome = "Nome Autor",
                Nacionalidade = "Desconhecida",
                DataNascimento = null,
                Biografia = "Sem informação"
            });

            ViewBag.Autores = autores;
            return View();
        }


        //POST : Autor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Autor autor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(autor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "não foi possivel inserir os dados.");
            }
            return View(autor);
        }



        //GET: Autor/Update

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.SingleOrDefaultAsync
                (m => m.AutorID == id);

            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }


        private bool AutorExists(long? id)
        {
            return _context.Autor.Any(e => e.AutorID == id);
        }






        //POST: Autor/Update
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(long? id, Autor autor)

        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!AutorExists(autor.AutorID))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            return View(autor);


        }




        //GET: Autor/Details

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.SingleOrDefaultAsync
                (m => m.AutorID == id);


            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }






        //GET: Autor/Delete

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.
                SingleOrDefaultAsync(d => d.AutorID == id);


            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        //POST: Autor/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            try
            {
                _context.Autor.Remove(autor);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Autor {autor.Nome.ToUpper()} foi removido";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Adicione manipulação específica de exceção aqui, se necessário
                // Por exemplo, você pode querer adicionar um erro ao ModelState.
                ModelState.AddModelError("Erro", "Não foi possível remover o autor. Verifique se não há dependências.");

                // Pode ser útil para depurar o erro
                Console.WriteLine(ex.Message);
                return View("Delete", autor);
            }
        }



    }
}
