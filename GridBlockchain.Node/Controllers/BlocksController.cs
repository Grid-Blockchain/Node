using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GridBlockchain.Node.Models;
using TestCore.Models;

namespace GridBlockchain.Node.Controllers
{
    [Produces("application/json")]
    [Route("api/Blocks")]
    public class BlocksController : Controller
    {
        private readonly GridBlockchainNodeContext _context;

        public BlocksController(GridBlockchainNodeContext context)
        {
            _context = context;
        }

        // GET: api/Blocks
        [HttpGet]
        public IEnumerable<Block> GetBlock()
        {
            return _context.Blocks;
        }

        // GET: api/Blocks/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlock([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var block = await _context.Blocks.SingleOrDefaultAsync(m => m.Hash == id);

            if (block == null)
            {
                return NotFound();
            }

            return Ok(block);
        }

        // PUT: api/Blocks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlock([FromRoute] string id, [FromBody] Block block)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != block.Hash)
            {
                return BadRequest();
            }

            _context.Entry(block).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlockExists(id))
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

        // POST: api/Blocks
        [HttpPost]
        public async Task<IActionResult> PostBlock([FromBody] Block block)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Blocks.Add(block);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlock", new { id = block.Hash }, block);
        }

        // DELETE: api/Blocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlock([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var block = await _context.Blocks.SingleOrDefaultAsync(m => m.Hash == id);
            if (block == null)
            {
                return NotFound();
            }

            _context.Blocks.Remove(block);
            await _context.SaveChangesAsync();

            return Ok(block);
        }

        private bool BlockExists(string id)
        {
            return _context.Blocks.Any(e => e.Hash == id);
        }
    }
}