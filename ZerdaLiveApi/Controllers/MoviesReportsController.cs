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
    public class MoviesReportsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public MoviesReportsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/MoviesReports
        [HttpGet("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<IEnumerable<MoviesReport>>> GetMoviesReports(string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 4)
            {
                return await _context.MoviesReports.ToListAsync();
            }
            return StatusCode(403);
        }

        // GET: api/MoviesReports/5
        [HttpGet("RID={RID}&UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<MoviesReport>> GetMoviesReport(int RID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 4)
            {
                var moviesReport = await _context.MoviesReports.FindAsync(RID);
                if (moviesReport == null)
                {
                    return StatusCode(404);
                }

                return moviesReport;
            }
            return StatusCode(403);
        }


        // POST: api/MoviesReports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("DeviceToken={DeviceToken}")]
        public async Task<ActionResult<MoviesReport>> PostMoviesReport(string DeviceToken,MoviesReport moviesReport)
        {
            var FindToken = await _context.DeviceTokens.FirstOrDefaultAsync(c => c.DeviceToken1 == DeviceToken);
            var IsReportExist = await _context.MoviesReports.Where(c => c.MovieId == c.MovieId && c.SenderTokenId == FindToken.DeviceTokenId).ToListAsync();
            if (IsReportExist.Any())
            {
                return StatusCode(403);
            }
            if (FindToken != null)
            {
                using (var ZContext = _context)
                {
                    object SS = new MoviesReport { MovieId = moviesReport.MovieId, ReportDis = moviesReport.ReportDis, SenderTokenId = FindToken.DeviceTokenId };
                    ZContext.Add(SS);
                    await ZContext.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(400);
        }

        // DELETE: api/MoviesReports/5
        [HttpDelete("RID={RID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteMoviesReport(int RID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 4)
            {
                var moviesReport = await _context.MoviesReports.FindAsync(RID);
                if (moviesReport == null)
                {
                    return StatusCode(404);
                }

                _context.MoviesReports.Remove(moviesReport);
                await _context.SaveChangesAsync();

                return StatusCode(204);
            }
            return StatusCode(403);
        }

    }
}
