using proyectoFinal.Models.Entidades;
using proyectoFinal.Models;
using Microsoft.EntityFrameworkCore;
using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace proyectoFinal.Services
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly SMedicoContext _context; //informacion de la libreria context de entidades

        public ServicioUsuarios(SMedicoContext context)
        {
            _context = context;
        }


        public async Task<Usuarios> GetUsuarios(string Nombre)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(c => c.Nombre == Nombre);
        }

        public async Task<Usuarios> GetUsuarios(string correo, string clave)
        {
            Usuarios usuario = await _context.Usuarios.Where(u => u.correo == correo && u.Clave == clave).FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<Usuarios> SaveUsuarios(Usuarios usuarios)
        {
            _context.Usuarios.Add(usuarios);
            await _context.SaveChangesAsync();
            return usuarios;
        }

        public async Task<Usuarios> ObtenerUsuarioPorId(int usuarioId)
        {
            // Aquí llamamos al método de tu repositorio de usuarios para obtener el usuario por su ID.
            // Este es un ejemplo y debes adaptarlo a tu lógica y estructura de datos.

            return await _context.Usuarios.FirstOrDefaultAsync(c => c.UsuarioID == usuarioId);
        }


    }
}