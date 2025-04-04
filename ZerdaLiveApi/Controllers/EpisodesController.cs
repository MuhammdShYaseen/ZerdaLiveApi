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
    public class EpisodesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public EpisodesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Episodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Episode>>> GetEpisodes()
        {
            return await _context.Episodes.ToListAsync();
        }

        // GET: api/Episodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Episode>> GetEpisode(int id)
        {
            var episode = await _context.Episodes.FindAsync(id);

            if (episode == null)
            {
                return StatusCode(404);
            }

            return episode;
        }

        // PUT: api/Episodes/5
        [HttpPut("EID={EID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutEpisode(int EID, string UserName, string Password, Episode episode)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole ==3)
            {
                if (EID != episode.EpisodesId)
                {
                    return StatusCode(400);
                }
                using (var ZContext = _context)
                {
                    var ss = ZContext.Episodes.First(a => a.EpisodesId == episode.EpisodesId);
                    ss.EpisodesName = episode.EpisodesName;
                    ss.EpisodesUrl = episode.EpisodesUrl;
                    try
                    {
                        await ZContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EpisodeExists(EID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return Ok();
            }
            return StatusCode(403);
        }


        // POST: api/Episodes
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Episode>> PostEpisode(string UserName, string Password, Episode episode)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                using (var ZContext = _context)
                {
                    object SS = new Episode { EpisodesName = episode.EpisodesName, SeasoneId = episode.SeasoneId, EpisodesUrl = episode.EpisodesUrl };
                    ZContext.Add(SS);
                    await ZContext.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(403);
        }

        // DELETE: api/Episodes/5
        [HttpDelete("EID={EID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteEpisode(int EID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                var Ep = await _context.Episodes.FindAsync(EID);
                if (Ep == null)
                {
                    return StatusCode(404);
                }
                _context.Episodes.Remove(Ep);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }

        private bool EpisodeExists(int id)
        {
            return _context.Episodes.Any(e => e.EpisodesId == id);
        }
    }
}
