using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using proyectoFinal.Models;
using proyectoFinal.Models.Entidades;
using proyectoFinal.Services;
using System.Security.Claims;

namespace proyectoFinal.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]

    
    public class UsuariosController : Controller
    {
        private readonly SMedicoContext _context;

        private readonly IServicioUsuarios _servicioUsuario;
        private readonly IServicioImagen _servicioImagen;

        public UsuariosController(IServicioUsuarios servicioUsuario,
         IServicioImagen servicioImagen, SMedicoContext context)
        {
            _servicioUsuario = servicioUsuario;
            _servicioImagen = servicioImagen;
            _context = context;
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListadoUsuario()
        {
            //  _context.Usuarios
            return View(await _context.Usuarios.ToListAsync());
        }




        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> EditarPerfil(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }



        [HttpPost]
        public async Task<IActionResult> Editar(Usuarios usuario, IFormFile Imagen)
        {
            try
            {
                var usuarioexistente = await _context.Usuarios.FindAsync(usuario.UsuarioID);
                if (usuarioexistente != null)
                {
                    if (Imagen != null)
                    {
                        Stream image = Imagen.OpenReadStream();
                        usuarioexistente.URLFotoPerfil = await _servicioImagen.SubirImagen(image, Imagen.FileName);
                    }

                    usuarioexistente.Nombre = usuario.Nombre;
                    usuarioexistente.Apellido = usuario.Apellido;
                    usuarioexistente.correo = usuario.correo;
                    usuarioexistente.Cedula = usuario.Cedula;
                    usuarioexistente.Clave = Utilitarios.EncriptarClave(usuario.Clave);
                    usuarioexistente.Roles = usuario.Roles;

                    _context.Entry(usuarioexistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    TempData["AlertMessage"] = "Usuario actualizado exitosamente!!!";

                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListadoUsuario");
                    }
                    else
                    {
                        return RedirectToAction("Perfil");
                    }



                }
                else
                {
                    return NotFound(); // Manejar el caso donde el usuario no se encuentra
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Ocurrió un error al actualizar");
            }

            return View(usuario);
        }



        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioID == id);

            if (usuario == null)
            {
                return NotFound();
            }

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Usuario eliminado exitosamente!!!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Ocurrió un error, no se pudo eliminar el registro");
            }

            return RedirectToAction(nameof(ListadoUsuario));
        }

        // USUARIO 
      public async Task<IActionResult> Perfil()
    {
        // Obtener el UsuarioID del usuario autenticado
        int usuarioId = ObtenerUsuarioId();

        // Obtener la información del perfil del usuario
        Usuarios usuario = await _servicioUsuario.ObtenerUsuarioPorId(usuarioId);

        // Puedes pasar el objeto usuario a la vista y mostrar la información del perfil
        return View(usuario);
    }

    // Otras acciones relacionadas con perfiles...

    private int ObtenerUsuarioId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }

        // Devuelve algún valor predeterminado o lanza una excepción según tu lógica
        return 0;
    }

  



    }

    



}