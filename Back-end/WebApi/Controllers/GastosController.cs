using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Persistencia.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("gastos")]
    [ApiController]
    public class GastosController : ControllerBase
    {
        private readonly GastoDbContext _context;
        private GastoService gastoService;
        private CategoriaService categoriaService;

        public GastosController(GastoDbContext context)
        {
            _context = context;
            gastoService = new GastoService(context);
            categoriaService  = new CategoriaService(context);
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
            return await gastoService.SearchCategoryAndDescription(categoria, description);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGasto(int id, Gasto gasto)
        {
            if (gastoService.isInvalidGasto(gasto)) return BadRequest();
            categoriaService.CategoriaCreateOrFind(gasto);
            _context.Entry(gasto).State = EntityState.Modified;
            try
            {
                Gasto OldGasto =  gastoService.SearchForOldGasto(gasto);
                await _context.SaveChangesAsync();
                if(categoriaService.removeCategoriaIfUnusedAfterUpdate(OldGasto , gasto)) await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!gastoService.GastoExists(id))
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
        [HttpPost]
        public async Task<ActionResult<Gasto>> PostGasto(Gasto gasto)
        {
            if(gastoService.isInvalidGasto(gasto)) return BadRequest();
            categoriaService.CategoriaCreateOrFind(gasto);

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
            
            categoriaService.deleteIfUnused(gasto);
            _context.Gastos.Remove(gasto);
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
