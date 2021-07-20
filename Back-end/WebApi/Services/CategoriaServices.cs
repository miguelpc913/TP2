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

        public void CategoriaCreateOrFind(Gasto gasto , User user){
            gasto.categoria.Tipo = gasto.categoria.Tipo.ToLower();
            //Check if Categoria is new - used
            if (!CategoriaIsUsed(gasto.categoria.Tipo , user))
            {
                gasto.categoria = _context.Categoria.Add(gasto.categoria).Entity;
                user.Categorias.Add(gasto.categoria);
            }
            else
            {
                gasto.categoria =  _context.Categoria.First(categoria => categoria.Tipo == gasto.categoria.Tipo && categoria.UserId == user.Id);
            }
        }

        public Boolean isCategoriaUnusedAfterUpdate (Gasto OldGasto , Gasto gasto , User user){
            return (!OldGasto.categoria.Id.Equals(gasto.categoria.Id) && !CategoriaIsUsed(OldGasto.categoria.Tipo , user));
        }

        public Boolean removeCategoriaIfUnusedAfterUpdate(Gasto OldGasto , Gasto gasto , User user){
            if(isCategoriaUnusedAfterUpdate(OldGasto , gasto , user)){
                _context.Categoria.Remove(OldGasto.categoria);
                return true;
            }
            return false;
        }

        public void deleteIfUnused(Gasto gasto , User user){
            if (!CategoriaIsUsed(gasto.categoria.Tipo, user , gasto.Id)) { 
                    _context.Categoria.Remove(gasto.categoria); 
                    user.Categorias.Remove(gasto.categoria);
            }
        }

        public bool CategoriaIsUsed(string tipo)
        {
            return _context.Gastos.Any(gasto => gasto.categoria.Tipo == tipo);
        }
        public bool CategoriaIsUsed(string tipo , User user)
        {
            return user.Gastos.Any(gasto => gasto.categoria.Tipo == tipo);
        }
        public bool CategoriaIsUsed(string tipo , int gastoId = 0)
        {
            return _context.Gastos.Any(gasto => gasto.categoria.Tipo == tipo && gasto.Id != gastoId);
        }
        public bool CategoriaIsUsed(string tipo , User user , int gastoId = 0 )
        {
            return user.Gastos.Any(gasto => gasto.categoria.Tipo == tipo && gasto.Id != gastoId);
        }
        public bool CategoriaIsUsedUserId(string tipo , int userId)
        {
            return _context.Gastos.Any(gasto => gasto.categoria.Tipo == tipo && gasto.UserId == userId);
        }
        
    }
}