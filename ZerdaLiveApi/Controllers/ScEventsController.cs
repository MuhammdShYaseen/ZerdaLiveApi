using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZerdaLiveApi.Helpper;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScEventsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public ScEventsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/ScEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScEvent>>> GetScEvents()
        {
            return await _context.ScEvents.ToListAsync();
        }

        // GET: api/ScEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScEvent>> GetScEvent(int id)
        {
            var scEvent = await _context.ScEvents.FindAsync(id);

            if (scEvent == null)
            {
                return NotFound();
            }

            return scEvent;
        }


        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutScEvent(int ID, string UserName, string Password, ScEvent scEvent)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            
            if (ID != scEvent.EventId)
            {
                return StatusCode(400);
            }

            using (_context)
            {
                var ss = _context.ScEvents.First(a => a.EventId == scEvent.EventId);
                ss.EventName = scEvent.EventName;
                ss.EventLogo = scEvent.EventLogo;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScEventExists(ID))
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return StatusCode(204);
        }


        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<ScEvent>> PostScEvent(string UserName, string Password, ScEvent scEvent)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            using (var ZContext = _context)
            {
                object SS = new ScEvent {EventName = scEvent.EventName, EventLogo = scEvent.EventLogo };
                ZContext.Add(SS);
                await ZContext.SaveChangesAsync();
            }
            return CreatedAtAction("GetScEvent", new { id = scEvent.EventId }, scEvent);
        }

        // DELETE: api/ScEvents/5
        [HttpDelete("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteScEvent(int ID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var scEvent = await _context.ScEvents.FindAsync(ID);
            if (scEvent == null)
            {
                return StatusCode(404);
            }

            _context.ScEvents.Remove(scEvent);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool ScEventExists(int id)
        {
            return _context.ScEvents.Any(e => e.EventId == id);
        }
    }
}
