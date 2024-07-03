using Microsoft.EntityFrameworkCore;
using proyectoFinal.Models;
using proyectoFinal.Models.Entidades;

namespace proyectoFinal.Services
{
    public class ServicioFactura : IServicioFactura

    {


        private readonly SMedicoContext _context; //informacion de la libreria context de entidades

        public ServicioFactura(SMedicoContext context)
        {
            _context = context;
        }


        public async Task<List<Factura>> ObtenerAccionesPersonalesPorCliente(int clienteId)
        {
            return await _context.Facturas.Where(c => c.IdCliente == clienteId).ToListAsync();
        }
    }
}
