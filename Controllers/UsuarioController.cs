using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {


        private readonly IESContext _context;

        public UsuarioController(IESContext context)
        {
            this._context = context;
        }


        // GET LIST
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuario
                .OrderBy(d => d.Nome)
                .ToListAsync();
            return View(usuarios);

        }

        //Get: Usuario/Create
        public IActionResult Create()
        {
            var usuarios = _context.Usuario.OrderBy(i => i.Nome).ToList();
            usuarios.Insert(0, new Usuario()
            {
                UsuarioID = 0,
                Nome = "Nome Usuario",
                Email = "Desconhecida",
                DataNascimento = null,
                Telefone = null,
            });

            ViewBag.Usuarios = usuarios;
            return View();
        }


        //POST : Usuario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "não foi possivel inserir os dados.");
            }
            return View(usuario);
        }



        //GET: Usuario/Update

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.SingleOrDefaultAsync
                (m => m.UsuarioID == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        private bool UsuarioExists(long? id)
        {
            return _context.Usuario.Any(e => e.UsuarioID == id);
        }

        //POST: Usuario/Update
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(long? id, Usuario usuario)

        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!UsuarioExists(usuario.UsuarioID))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            return View(usuario);


        }




        //GET: Usuario/Details

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.SingleOrDefaultAsync
                (m => m.UsuarioID == id);


            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        //GET: Usuario/Delete

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.
                SingleOrDefaultAsync(d => d.UsuarioID == id);


            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        //POST: Usuario/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            try
            {
                _context.Usuario.Remove(usuario);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Usuario {usuario.Nome.ToUpper()} foi removido";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Adicione manipulação específica de exceção aqui, se necessário
                // Por exemplo, você pode querer adicionar um erro ao ModelState.
                ModelState.AddModelError("Erro", "Não foi possível remover o usuario. Verifique se não há dependências.");

                // Pode ser útil para depurar o erro
                Console.WriteLine(ex.Message);
                return View("Delete", usuario);
            }
        }



    }
}
