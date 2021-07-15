using System;
using System.Linq;
using Persistencia;
using Persistencia.Models;

namespace WebApi.Services 
{
    public class CategoriaService
    {
        private readonly GastoDbContext _context;
        public CategoriaService(GastoDbContext context){
            _context = context;
        }

        public void CategoriaCreateOrFind(Gasto gasto){
            gasto.categoria.Tipo = gasto.categoria.Tipo.ToLower();
            //Check if Categoria is new - used
            if (!CategoriaIsUsed(gasto.categoria.Tipo))
            {
                gasto.categoria = _context.Categoria.Add(gasto.categoria).Entity;
            }
            else
            {
                gasto.categoria =  _context.Categoria.First(categoria => categoria.Tipo == gasto.categoria.Tipo);
            }
        }

        public Boolean isCategoriaUnusedAfterUpdate (Gasto OldGasto , Gasto gasto){
            return (!OldGasto.categoria.Id.Equals(gasto.categoria.Id) && !CategoriaIsUsed(OldGasto.categoria.Tipo));
        }

        public Boolean removeCategoriaIfUnusedAfterUpdate(Gasto OldGasto , Gasto gasto){
            if(isCategoriaUnusedAfterUpdate(OldGasto , gasto)){
                _context.Categoria.Remove(OldGasto.categoria);
                return true;
            }
            return false;
        }

        public void deleteIfUnused(Gasto gasto){
            if (!CategoriaIsUsed(gasto.categoria.Tipo, gasto.Id)) { _context.Categoria.Remove(gasto.categoria); }
        }

        public bool CategoriaIsUsed(string tipo)
        {
            return _context.Gastos.Any(gasto => gasto.categoria.Tipo == tipo);
        }
        public bool CategoriaIsUsed(string tipo , int gastoId = 0)
        {
            return _context.Gastos.Any(gasto => gasto.categoria.Tipo == tipo && gasto.Id != gastoId);
        }
    }
}