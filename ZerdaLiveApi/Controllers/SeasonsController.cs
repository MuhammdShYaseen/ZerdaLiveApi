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
    public class SeasonsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public SeasonsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Seasons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Season>>> GetSeasons()
        {
            return await _context.Seasons.ToListAsync();
        }

        // GET: api/Seasons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Season>> GetSeason(int id)
        {
            var season = await _context.Seasons.FindAsync(id);

            if (season == null)
            {
                return StatusCode(404);
            }

            return season;
        }

        // PUT: api/Seasons/5
        [HttpPut("SeID={SeID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutSeason(int SeID, string UserName, string Password, Season season)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                if (SeID != season.SeasonsId)
                {
                    return StatusCode(400);
                }

                using (var ZContext = _context)
                {
                    var ss = ZContext.Seasons.First(a => a.SeasonsId == season.SeasonsId);
                    ss.SeasonsName = season.SeasonsName;
                    ss.SeasonsDis = season.SeasonsDis;
                    ss.SeasonsImage = season.SeasonsImage;
                    ss.IsNew = season.IsNew;
                    ss.IsTob = season.IsTob;
                    try
                    {
                        await ZContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SeasonExists(SeID))
                        {
                            return StatusCode(404);
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

        // POST: api/Seasons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Season>> PostSeason(string UserName, string Password, Season season)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                using (var ZContext = _context)
                {
                    object SS = new Season { SeasonsName = season.SeasonsName, SeasonsDis = season.SeasonsDis, SeasonsImage = season.SeasonsImage, SeriesId = season.SeriesId, IsTob = season.IsTob, IsNew = season.IsNew };
                    ZContext.Add(SS);
                    await ZContext.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(403);
        }

        // DELETE: api/Seasons/5
        [HttpDelete("SeID={SeID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteSeason(int SeID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                var Seas = await _context.Seasons.FindAsync(SeID);
                if (Seas == null)
                {
                    return StatusCode(404);
                }
                _context.Seasons.Remove(Seas);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.SeasonsId == id);
        }
    }
}
