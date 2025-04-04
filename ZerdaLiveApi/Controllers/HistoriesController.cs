using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZerdaLiveApi.Attributes;
using ZerdaLiveApi.Helpper;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Controllers
{
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoriesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public HistoriesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Histories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<History>>> GetHistories()
        {
            return await _context.Histories.ToListAsync();
        }

        // GET: api/Histories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<History>> GetHistory(int id)
        {
            var history = await _context.Histories.FindAsync(id);

            if (history == null)
            {
                return StatusCode(404);
            }

            return history;
        }

        // PUT: api/Histories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutHistory(int ID, string UserName, string Password, History history)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (ID != history.HistoryId)
            {
                return StatusCode(400);
            }

            _context.Entry(history).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoryExists(ID))
                {
                    return StatusCode(404);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Histories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<History>> PostHistory(string UserName, string Password, History history)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            _context.Histories.Add(history);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistory", new { id = history.HistoryId }, history);
        }

        // DELETE: api/Histories/5
        [HttpDelete("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteHistory(int ID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var history = await _context.Histories.FindAsync(ID);
            if (history == null)
            {
                return StatusCode(404);
            }

            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool HistoryExists(int id)
        {
            return _context.Histories.Any(e => e.HistoryId == id);
        }
    }
}
