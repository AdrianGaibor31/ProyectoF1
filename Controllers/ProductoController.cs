using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Models;
using proyectoFinal.Models.Entidades;
using proyectoFinal.Services;




namespace proyectoFinal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductoController : Controller
    {

        private readonly IServicioUsuarios _servicioUsuario;
        private readonly IServicioImagenP _servicioImagenP;
        private readonly IServicioLista _servicioLista;
        private readonly SMedicoContext _context;
        private readonly IServicioProducto _servicioProducto;
        public ProductoController(IServicioUsuarios servicioUsuario, IServicioImagenP servicioImagenP,
            IServicioLista servicioLista, SMedicoContext context, IServicioProducto servicioProducto)
        {
            _servicioUsuario = servicioUsuario;
            _servicioImagenP = servicioImagenP;
            _servicioLista = servicioLista;
            _context = context;
            _servicioProducto = servicioProducto;
        }

        [HttpGet]
        public async Task<IActionResult> ListadoProducto()
        {
            var categoriaList = await _context.Productos
                .Include(c => c.Categorias)
                .ToListAsync();

            return View(categoriaList);
        }



        public async Task<IActionResult> Crear()
        {
            Producto producto = new Producto

            {
                Categoria = await _servicioLista.GetListaCategoria(),

            };

            // Asigna las listas al ViewData para que estén disponibles en la vista
            ViewData["Categoria"] = producto.Categoria;


            return View(producto);
        }

        [HttpPost]

        public async Task<IActionResult> Crear(Producto producto, IFormFile Imagen)
        {

            producto.Categoria = await _servicioLista.GetListaCategoria();

            // Proceder con la carga de la imagen
            Stream image = Imagen.OpenReadStream();

            string urlImagen = await _servicioImagenP.SubirImagen(image, Imagen.FileName);

            producto.URLFoto = urlImagen;

            Producto productoCreado = await _servicioProducto.SaveProducto(producto);

            if (productoCreado.IdProducto > 0)
            {
                return RedirectToAction("ListadoProducto", "Producto");
            }

            ViewData["Mensaje"] = "No se pudo crear el Producto";
            return View();


        }



        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.Categoria = await _servicioLista.GetListaCategoria();

            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Producto producto, IFormFile Imagen)
        {

            try
            {
                var productoexistente = await _context.Productos.FindAsync(producto.IdProducto);

                if (productoexistente == null)
                {
                    return NotFound();
                }

                if (Imagen != null)
                {
                    Stream image = Imagen.OpenReadStream();
                    productoexistente.URLFoto = await _servicioImagenP.SubirImagen(image, Imagen.FileName);
                }

                productoexistente.Nombre = producto.Nombre;
                productoexistente.Descripcion = producto.Descripcion;
                productoexistente.Precio = producto.Precio;
                productoexistente.CantidadEnStock = producto.CantidadEnStock;
                productoexistente.FechaCreacion = producto.FechaCreacion;
                productoexistente.Activo = producto.Activo;
                productoexistente.CodigoProducto = producto.CodigoProducto;



                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Producto Actualizado exitosamente";

                return RedirectToAction("ListadoProducto");
            }
            catch (Exception ex)
            {
                TempData["alertMessage"] = ex;
            }



            producto.Categoria = await _servicioLista.GetListaCategoria();

            return View(producto);
        }
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            try
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Producto eliminado exitosamente";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Ocurrió un error, no se pudo eliminar el Producto");
            }

            return RedirectToAction(nameof(ListadoProducto));
        }







    }
}
