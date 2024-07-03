using proyectoFinal.Models.Entidades;
using proyectoFinal.Models;
using Microsoft.EntityFrameworkCore;
using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace proyectoFinal.Services
{
    public class ServicioProducto : IServicioProducto
    {
        private readonly SMedicoContext _context; //informacion de la libreria context de entidades

        public ServicioProducto(SMedicoContext context)
        {
            _context = context;
        }

        
        public async Task<Producto> SaveProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }
    }
}
