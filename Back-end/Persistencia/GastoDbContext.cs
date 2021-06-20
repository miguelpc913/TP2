using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistencia.Models;



namespace Persistencia
{
    public class GastoDbContext : DbContext
    {

        public virtual DbSet<Gasto> Gastos {get;set;}
        public virtual DbSet<Categoria> Categoria {get;set;}

        public GastoDbContext(){}

        public GastoDbContext(DbContextOptions<GastoDbContext> options):base(options){}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder){


        }
    }    
}
