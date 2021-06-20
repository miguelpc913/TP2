using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Persistencia.Models;

namespace WebApi.Controllers
{
    [Route("gastos")]
    [ApiController]
    public class GastosController : ControllerBase
    {
        private readonly GastoDbContext _context;

        public GastosController(GastoDbContext context)
        {
            _context = context;
        }

        // GET: gastos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gasto>>> GetGastos()
        {
             
            return await _context.Gastos.ToListAsync();
        }

        //GET: gastos/categorias
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoria()
        {
             
            return await _context.Categoria.ToListAsync();
        }

        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Gasto>>> GetGastoCategoria(string categoria, string description)
        {
            if (!String.IsNullOrEmpty(description) && !String.IsNullOrEmpty(categoria))
            {
                return await _context.Gastos.Where(gasto => gasto.categoria.Tipo.ToLower().Equals(categoria) && gasto.Description.ToLower().Contains(description)).ToListAsync();
            }
            else if (!String.IsNullOrEmpty(description))
            {
                return await _context.Gastos.Where(gasto => gasto.Description.ToLower().Contains(description.ToLower())).ToListAsync();
            }
            else if (!String.IsNullOrEmpty(categoria))
            {
                return await _context.Gastos.Where(gasto => gasto.categoria.Tipo.ToLower().Equals(categoria)).ToListAsync();
            }
            return await _context.Gastos.ToListAsync();
        }


        // GET: gastos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gasto>> GetGasto(int id)
        {
            Gasto gasto = await _context.Gastos.FindAsync(id);

            if (gasto == null) return NotFound();
            
            return gasto;
        }

        // PUT: gastos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGasto(int id, Gasto gasto)
        {

            if (id != gasto.Id || isInvalidGasto(gasto)) return BadRequest();
            gasto.categoria.Tipo = gasto.categoria.Tipo.ToLower();
            //Check if Categoria is new - unused 
            CategoriaUpdate(gasto);
            _context.Entry(gasto).State = EntityState.Modified;
            try
            {
                Gasto OldGasto =  await _context.Gastos.AsNoTracking().Where(gastoDB => gastoDB.Id == gasto.Id).Include(gasto => gasto.categoria).AsNoTracking().FirstAsync();
                await _context.SaveChangesAsync();
                if (!OldGasto.categoria.Id.Equals(gasto.categoria.Id))
                {
                    if (removeUnusedCountingGasto(OldGasto)) await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GastoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return NoContent();
        }

        // POST: gastos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gasto>> PostGasto(Gasto gasto)
        {
            gasto.categoria.Tipo = gasto.categoria.Tipo.ToLower();
            if(isInvalidGasto(gasto)) return BadRequest();

            CategoriaUpdate(gasto);
            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGasto", new { id = gasto.Id }, gasto);
        }

        // DELETE: gastos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGasto(int id)
        {
            Gasto gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null)  return NotFound();
            Categoria categoria = gasto.categoria;

            removeUnusedNotCoutingGasto(gasto);
            _context.Gastos.Remove(gasto);
            
            await _context.SaveChangesAsync();
        

            return NoContent();
        }
        
        private void CategoriaUpdate(Gasto gasto){
            //Check if Categoria is new - used
            if (!CategoriaIsUsed(gasto.categoria.Tipo))
            {
                createCategoria(gasto);
            }
            else
            {
                gasto.categoria =  _context.Categoria.First(categoria => categoria.Tipo == gasto.categoria.Tipo);
            }
        }
        private void createCategoria(Gasto gasto)
        {
            gasto.categoria = _context.Categoria.Add(gasto.categoria).Entity;
        }
        private bool removeUnusedNotCoutingGasto(Gasto gasto)
        {
            if (!CategoriaIsUsed(gasto.categoria.Tipo, gasto.Id))
            {
                _context.Categoria.Remove(gasto.categoria); 
                return true;
            }
            return false;
        }
        private bool removeUnusedCountingGasto(Gasto gasto)
        {
            if (!CategoriaIsUsed(gasto.categoria.Tipo))
            {
                _context.Categoria.Remove(gasto.categoria);
                return true;
            }
            return false;
        }

        private bool isInvalidGasto(Gasto gasto){
            return (gasto.Price < 1 || gasto.CompartidoEntre < 0 || (gasto.Compartido && gasto.CompartidoEntre < 1) ||  (!gasto.Compartido && gasto.CompartidoEntre > 0 ) );
        }
        private bool CategoriaIsUsed(string tipo)
        {
            return _context.Gastos.Any(gasto => gasto.categoria.Tipo == tipo);
        }
        private bool CategoriaIsUsed(string tipo , int gastoId = 0)
        {
            return _context.Gastos.Any(gasto => gasto.categoria.Tipo == tipo && gasto.Id != gastoId);
        }
        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.Id == id);
        }
    }
}
