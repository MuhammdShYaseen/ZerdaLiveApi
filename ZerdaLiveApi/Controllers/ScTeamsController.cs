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
    public class ScTeamsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public ScTeamsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/ScTeams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScTeam>>> GetScTeams()
        {
            return await _context.ScTeams.ToListAsync();
        }

        // GET: api/ScTeams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScTeam>> GetScTeam(int id)
        {
            var scTeam = await _context.ScTeams.FindAsync(id);

            if (scTeam == null)
            {
                return StatusCode(404);
            }

            return scTeam;
        }

      
        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutScTeam(int ID, string UserName, string Password, ScTeam scTeam)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (ID != scTeam.TeamId)
            {
                return BadRequest();
            }

            using (_context)
            {
                var ss = _context.ScTeams.First(a => a.TeamId == scTeam.TeamId);
                ss.TeamName = scTeam.TeamName;
                ss.TeamLogo = scTeam.TeamLogo;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScTeamExists(ID))
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
        public async Task<ActionResult<ScTeam>> PostScTeam(string UserName, string Password, ScTeam scTeam)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }

            using (var ZContext = _context)
            {
                object SS = new ScTeam { TeamName = scTeam.TeamName, TeamLogo = scTeam.TeamLogo };
                ZContext.Add(SS);
                await ZContext.SaveChangesAsync();
            }

            return CreatedAtAction("GetScTeam", new { id = scTeam.TeamId }, scTeam);
        }

        // DELETE: api/ScTeams/5
        [HttpDelete("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteScTeam(int ID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return Forbid();
            }
            var scTeam = await _context.ScTeams.FindAsync(ID);
            if (scTeam == null)
            {
                return StatusCode(404);
            }

            _context.ScTeams.Remove(scTeam);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool ScTeamExists(int id)
        {
            return _context.ScTeams.Any(e => e.TeamId == id);
        }
    }
}
