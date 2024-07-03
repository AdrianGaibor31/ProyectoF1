using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using proyectoFinal.Models.Entidades;
using proyectoFinal.Models;
using System.Security.Claims;
using proyectoFinal.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;



namespace proyectoFinal.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServicioUsuarios _servicioUsuario;
        private readonly IServicioImagen _servicioImagen;
        private readonly SMedicoContext _context;


        public LoginController(IServicioUsuarios servicioUsuario,
        IServicioImagen servicioImagen, SMedicoContext context)
        {
            _servicioUsuario = servicioUsuario;
            _servicioImagen = servicioImagen;
            _context = context;
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuarios usuario, IFormFile Imagen)
        {
            Stream image = Imagen.OpenReadStream();
            string urlImagen = await _servicioImagen.SubirImagen(image, Imagen.FileName);

            usuario.Clave= Utilitarios.EncriptarClave(usuario.Clave);
            usuario.URLFotoPerfil= urlImagen;

            Usuarios usuarioCreado = await _servicioUsuario.SaveUsuarios(usuario);

            if (usuarioCreado.UsuarioID > 0)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string password)
        {
            Usuarios usuarioEncontrado = await _servicioUsuario.GetUsuarios(correo, Utilitarios.EncriptarClave(password));

            if (usuarioEncontrado == null)
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }

            List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre),
        new Claim(ClaimTypes.NameIdentifier, usuarioEncontrado.UsuarioID.ToString()), // Agrega la claim del UsuarioID
        new Claim("FotoPerfil", usuarioEncontrado.URLFotoPerfil),
    };

            foreach (var rol in usuarioEncontrado.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // Validación por cookies
            var user = HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                // El usuario está autenticado por cookies, puedes realizar acciones adicionales si es necesario.
            }

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Salir()
        {
            //3.- CONFIGURACION DE LA AUTENTICACION
            #region AUTENTICACTION
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            #endregion

            return RedirectToAction("IniciarSesion", "Login");

        }


        public IActionResult VerificarRoles()
        {
            // Obtener la lista de roles del usuario
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            // Convertir la lista de roles en una cadena para mostrarla
            var rolesString = string.Join(", ", roles);

            // Mostrar la información de roles
            return Content($"Roles del usuario: {rolesString}");
        }
        public IActionResult MostrarClaims()
        {
            var claims = ((ClaimsIdentity)User.Identity).Claims;
            foreach (var claim in claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
            return Content("Revisa la consola del navegador o la salida de la aplicación para ver los claims.");
        }


    }

}