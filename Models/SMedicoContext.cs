using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using proyectoFinal.Models.Entidades;

namespace proyectoFinal.Models
{
    public class SMedicoContext : DbContext
    {

        public SMedicoContext()
        {

        }

        public SMedicoContext(DbContextOptions<SMedicoContext> options) : base(options) { }


        public DbSet<Producto> Productos { get; set; }

        public DbSet<Usuarios> Usuarios { get; set; }
    
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Factura> Facturas { get; set; }

      






        protected override void OnModelCreating(ModelBuilder modelBuilder)  //Este metodo q va ayudar a conectar con la BDD
        {
            

            base.OnModelCreating(modelBuilder);
        }


    }
}

