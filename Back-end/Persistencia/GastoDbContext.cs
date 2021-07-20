using Microsoft.EntityFrameworkCore;
using Persistencia.Models;

namespace Persistencia
{
    public class GastoDbContext : DbContext
    {
        public virtual DbSet<Gasto> Gastos {get;set;}
        public virtual DbSet<Categoria> Categoria {get;set;}
        public virtual DbSet<User> Users {get;set;}
        public GastoDbContext(){}
        public GastoDbContext(DbContextOptions<GastoDbContext> options):base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder){
        }
    }    
}
