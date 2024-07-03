using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectoFinal.Models.Entidades;
using proyectoFinal.Models;
using proyectoFinal.Services;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Models.Entidades.ViewModelAccion;
using Rotativa.AspNetCore;

namespace proyectoFinal.Controllers
{
   
    public class FacturaController : Controller
    {

        private readonly IServicioUsuarios _servicioUsuario;
        private readonly IServicioImagenP _servicioImagenP;
        private readonly IServicioLista _servicioLista;
        private readonly SMedicoContext _context;
        private readonly IServicioProducto _servicioProducto;
        private readonly ILogger<FacturaController> _logger;

        public FacturaController(IServicioUsuarios servicioUsuario, IServicioImagenP servicioImagenP,
            IServicioLista servicioLista, SMedicoContext context, IServicioProducto servicioProducto, ILogger<FacturaController> logger)
        {
            _servicioUsuario = servicioUsuario;
            _servicioImagenP = servicioImagenP;
            _servicioLista = servicioLista;
            _context = context;
            _servicioProducto = servicioProducto;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListadoFactura()
        {
            var facturaslist = await _context.Facturas
         .Include(f => f.Cliente)

         .ToListAsync();

            return View(facturaslist);
        }



        public async Task<IActionResult> Crear()
        {
            // Aquí debes inicializar una nueva instancia de Factura y asignarla a 'factura'
            var factura = new Factura();

            // Puedes inicializar otros campos de factura si es necesario
            factura.Fecha = DateTime.Now;  // Por ejemplo, asignar la fecha actual

            // Asegúrate de incluir la lista de detalles de factura si es necesario
            factura.DetallesFactura = new List<DetalleFacturas>();

            ViewData["Productos"] = await _context.Productos.ToListAsync();

            // Devuelve la vista 'Crear' con el objeto 'factura'
            return View(factura);
        }


        [HttpPost]

        public async Task<IActionResult> Crear(Factura factura, List<DetalleFacturas> detalleFacturas)
        {
            if (factura.IdFactura != null)
            {
                // Validar si se ingresaron detalles de factura
                if (detalleFacturas != null )
                {
                    // Obtener el cliente seleccionado
                    var clienteSeleccionado = await _context.Clientes.FindAsync(factura.IdCliente);
                    if (clienteSeleccionado == null)
                    {
                        ModelState.AddModelError("", "El cliente seleccionado no existe.");
                        ViewData["Productos"] = await _context.Productos.ToListAsync(); // Obtener lista de productos para el formulario
                        return View(factura);
                    }

                    // Asignar el cliente a la factura
                    factura.Cliente = clienteSeleccionado;

                    // Asociar detalles de factura con la factura principal
                    factura.DetallesFactura = detalleFacturas;

                    // Guardar la factura y sus detalles en la base de datos
                    _context.Facturas.Add(factura);
                    await _context.SaveChangesAsync();

                    // Redirigir al listado de facturas
                    return RedirectToAction("ListadoFactura");
                }
                else
                {
                    ModelState.AddModelError("", "Debe agregar al menos un detalle de factura.");
                }
            }

            // Si hay errores de validación, recargar la vista con los datos y los mensajes de error
            ViewData["Productos"] = await _context.Productos.ToListAsync(); // Obtener lista de productos para el formulario
            return View(factura);
        }







        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            // Obtener la lista de clientes y productos para rellenar los selects en la vista
            ViewData["Clientes"] = await _context.Clientes.ToListAsync();
            ViewData["Productos"] = await _context.Productos.ToListAsync();

            return View(factura);
        }




        [HttpPost]
        public async Task<IActionResult> Editar(Factura factura)
        {
            try
            {
                var facturaExistente = await _context.Facturas.FindAsync(factura.IdFactura);

                if (facturaExistente == null)
                {
                    return NotFound();
                }

                // Actualizar campos de la factura con los datos del formulario
                facturaExistente.NombreFactura = factura.NombreFactura;
                facturaExistente.Fecha = factura.Fecha;
                facturaExistente.Total = factura.Total;
                facturaExistente.Iva = factura.Iva;
                facturaExistente.SubTotal = factura.SubTotal;
                facturaExistente.IdCliente = factura.IdCliente;


                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                TempData["AlertMessage"] = "Factura actualizada exitosamente";

                return RedirectToAction("ListadoFactura");
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = $"Error al actualizar la factura: {ex.Message}";
            }

            // Obtener la lista de clientes para rellenar el select en caso de error
            ViewData["Clientes"] = await _context.Clientes.ToListAsync();

            return View(factura);
        }



        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            try
            {
                _context.Facturas.Remove(factura);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Factura eliminada exitosamente";
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = $"Ocurrió un error, no se pudo eliminar la factura: {ex.Message}";
            }

            return RedirectToAction(nameof(ListadoFactura));
        }

        [HttpGet]
        public async Task<IActionResult> BuscarCliente(string cedula)
        {
            // Realiza la lógica para buscar el usuario por cédula en tu base de datos
            var cliente = await _servicioLista.BuscarClientePorCedula(cedula);

            if (cliente != null)
            {
                return Json(new
                {
                    nombre = cliente.Nombres,
                    apellido = cliente.Apellidos,
                    telefono = cliente.Telefono,
                    direccion = cliente.Direccion,
                    idcliente = cliente.IdCliente
                });
            }
            else
            {
                return Json(null);
            }
        }
        public async Task<IActionResult> BuscarProducto(string codigopro)
        {
            // Realiza la lógica para buscar el usuario por cédula en tu base de datos
            var producto = await _servicioLista.BuscarProductoPorCodigo(codigopro);

            if (producto != null)
            {
                return Json(new
                {
                    nombre = producto.Nombre,
                    cantidadStock = producto.CantidadEnStock,
                    categoria = producto.Categoria,
                    precio = producto.Precio,
                    idproducto = producto.IdProducto

                });
            }
            else
            {
                return Json(null);
            }
        }

        public async Task<IActionResult> ImprimirFactura(int clienteId)
        {
            _logger.LogInformation($"Valor de clienteId: {clienteId}");

            ViewModelAccion modelo = await _context.Facturas
                .Where(v => v.IdCliente == clienteId)
                .Select(v => new ViewModelAccion()
                {
                    IdFactura = v.IdFactura.ToString(),


                    NombreFactura= v.NombreFactura.ToString(),



                    NombreCliente=v.NombreCliente,

                  ApellidoCliente= v.ApellidoCliente,

                    CedulaCliente=v.CedulaCliente,

                     DireccionCliente = v.DireccionCliente,


                      Fecha = v.Fecha.ToString(),


                    Total = v.Total.ToString(),


                      Iva=v.Iva.ToString(), 


                    SubTotal=v.SubTotal.ToString(),

                    IdCliente=v.IdCliente.ToString(),

                    DescuentoTotal = v.DescuentoTotal.ToString(),


                })
                .FirstOrDefaultAsync();





            if (modelo == null)
            {
                return NotFound("La Factura no fue encontrada.");
            }

            return new ViewAsPdf("ImprimirFactura", modelo)
            {
                FileName = $"Factura {modelo.CedulaCliente + " " + modelo.Fecha}.pdf",

                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,

            };
        }





    }
}
