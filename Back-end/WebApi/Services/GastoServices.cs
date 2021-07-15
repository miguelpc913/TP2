using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Persistencia.Models;

namespace WebApi.Services 
{
    public class GastoService
    {
        private readonly GastoDbContext _context;
        public GastoService(GastoDbContext context){
            _context = context;
        }

        public bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.Id == id);
        }

        public bool isInvalidGasto(Gasto gasto){
            return (gasto.Price < 1 || gasto.CompartidoEntre < 0 || (gasto.Compartido && gasto.CompartidoEntre < 1) ||  (!gasto.Compartido && gasto.CompartidoEntre > 0 ) );
        }

        public Gasto SearchForOldGasto(Gasto gasto){
          Gasto OldGasto = _context.Gastos.AsNoTracking().Where(gastoDB => gastoDB.Id == gasto.Id).Include(gasto => gasto.categoria).AsNoTracking().First();
          return OldGasto;
        }

        public async Task<ActionResult<IEnumerable<Gasto>>> SearchCategoryAndDescription(string categoria, string description){
            formatSearch(categoria, description);
            if (!String.IsNullOrEmpty(description) && !String.IsNullOrEmpty(categoria))
            {
                return await _context.Gastos.Where(
                    gasto => gasto.categoria.Tipo.Equals(categoria) && gasto.Description.Contains(description)
                    ).ToListAsync();
            }
            else if (!String.IsNullOrEmpty(description))
            {
                return await _context.Gastos.Where(gasto => gasto.Description.Contains(description)).ToListAsync();
            }
            else if (!String.IsNullOrEmpty(categoria))
            {
                return await _context.Gastos.Where(gasto => gasto.categoria.Tipo.Equals(categoria)).ToListAsync();
            }
            return await _context.Gastos.ToListAsync();
        }

        private void formatSearch(string categoria, string description){
            if(!String.IsNullOrEmpty(categoria)){ categoria =  categoria.ToLower().Trim(); };
            if(!String.IsNullOrEmpty(description)){ description = description.ToLower().Trim(); };
        }
    }
}