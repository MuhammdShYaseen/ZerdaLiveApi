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
    public class ScMatchesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public ScMatchesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/ScMatches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScMatch>>> GetScMatches()
        {
            return await _context.ScMatches.ToListAsync();
        }

        // GET: api/ScMatches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScMatch>> GetScMatch(int id)
        {
            var scMatch = await _context.ScMatches.FindAsync(id);

            if (scMatch == null)
            {
                return StatusCode(404);
            }

            return scMatch;
        }


        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutScMatch(int ID, string UserName, string Password, ScMatch scMatch)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (ID != scMatch.MatchId)
            {
                return StatusCode(400);
            }

            using (_context)
            {
                var ss = _context.ScMatches.First(a => a.MatchId == scMatch.MatchId);
                ss.EventId = scMatch.EventId;
                ss.FirstTeam = scMatch.FirstTeam;
                ss.FirstTeamGoals = scMatch.FirstTeamGoals;
                ss.MatchDate = scMatch.MatchDate;
                //ss.MatchTime = scMatch.MatchTime;
                ss.SecondTeam = scMatch.SecondTeam;
                ss.SecondTeamGoals = scMatch.SecondTeamGoals;
                ss.ChannelCategory = scMatch.ChannelCategory;
                ss.Commentator = scMatch.Commentator;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScMatchExists(ID))
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
        public async Task<ActionResult<ScMatch>> PostScMatch(string UserName, string Password, ScMatch scMatch)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            using (var ZContext = _context)
            {
                object SS = new ScMatch {FirstTeam = scMatch.FirstTeam ,FirstTeamGoals = scMatch.FirstTeamGoals ,MatchDate = scMatch.MatchDate ,SecondTeam = scMatch.SecondTeam ,SecondTeamGoals = scMatch.SecondTeamGoals ,ChannelCategory = scMatch.ChannelCategory, EventId = scMatch.EventId,Commentator = scMatch.Commentator };
                ZContext.Add(SS);
                await ZContext.SaveChangesAsync();
            }

            return CreatedAtAction("GetScMatch", new { id = scMatch.MatchId }, scMatch);
        }

        // DELETE: api/ScMatches/5
        [HttpDelete("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteScMatch(int ID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var scMatch = await _context.ScMatches.FindAsync(ID);
            if (scMatch == null)
            {
                return StatusCode(404);
            }

            _context.ScMatches.Remove(scMatch);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool ScMatchExists(int id)
        {
            return _context.ScMatches.Any(e => e.MatchId == id);
        }
    }
}
