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
    public class SeriesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public SeriesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Series
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Series>>> GetSeries()
        {
            return await _context.Series.Include(c => c.Seasons).ThenInclude(y => y.Episodes).ToListAsync();
        }

        // GET: api/Series/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Series>> GetSeries(int id)
        {
            var series = await _context.Series.FindAsync(id);

            if (series == null)
            {
                return StatusCode(404);
            }

            return series;
        }

        // PUT: api/Series/5
        [HttpPut("SID={SID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutSeries(int SID, string UserName, string Password, Series series)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                if (SID != series.SeriesId)
                {
                    return StatusCode(400);
                }
                using (var ZContext = _context)
                {
                    var ss = ZContext.Series.First(a => a.SeriesId == series.SeriesId);
                    ss.SeriesName = series.SeriesName;
                    ss.SeriesCat = series.SeriesCat;
                    ss.SeriesCountry = series.SeriesCountry;
                    ss.SeriesLang = series.SeriesLang;
                    ss.SeriesImage = series.SeriesImage;
                    ss.SeriesDis = series.SeriesDis;
                    ss.IsNew = series.IsNew;
                    ss.IsTob = series.IsTob;
                    try
                    {
                        await ZContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SeriesExists(SID))
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

        // POST: api/Series
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Series>> PostSeries(string UserName, string Password, Series series)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {

                using (var ZContext = _context)
                {
                    object SS = new Series { SeriesName = series.SeriesName, SeriesCat = series.SeriesCat, SeriesCountry = series.SeriesCountry, SeriesDis = series.SeriesDis, SeriesImage = series.SeriesImage, SeriesLang = series.SeriesLang, IsTob = series.IsTob, IsNew = series.IsNew };
                    ZContext.Add(SS);
                    await ZContext.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(403);

        }

        // DELETE: api/Series/5
        [HttpDelete("SID={SID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteSeries(int SID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                var Ser = await _context.Series.FindAsync(SID);
                if (Ser == null)
                {
                    return StatusCode(404);
                }
                _context.Series.Remove(Ser);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }

        private bool SeriesExists(int id)
        {
            return _context.Series.Any(e => e.SeriesId == id);
        }
    }
}
