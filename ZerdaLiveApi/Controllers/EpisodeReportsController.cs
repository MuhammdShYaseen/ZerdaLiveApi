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
    public class EpisodeReportsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public EpisodeReportsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/EpisodeReports
        [HttpGet("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<IEnumerable<EpisodeReport>>> GetEpisodeReports(string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            { 
                return await _context.EpisodeReports.ToListAsync();
            }
            return StatusCode(403);

        }

        // GET: api/EpisodeReports/5
        [HttpGet("RID={RID}&UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<EpisodeReport>> GetEpisodeReport(int RID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                var episodeReport = await _context.EpisodeReports.FindAsync(RID);

                if (episodeReport == null)
                {
                    return NotFound();
                }

                return episodeReport;
            }
            return StatusCode(403);
        }

        

        // POST: api/EpisodeReports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("DeviceToken={DeviceToken}")]
        public async Task<ActionResult<EpisodeReport>> PostEpisodeReport(string DeviceToken, EpisodeReport episodeReport)
        {
            var FindToken = await _context.DeviceTokens.FirstOrDefaultAsync(c => c.DeviceToken1 == DeviceToken);
            var IsReportExist = await _context.EpisodeReports.Where(c => c.EpisodeId == c.EpisodeId && c.SenderTokenId == FindToken.DeviceTokenId).ToListAsync();
            if (IsReportExist.Any())
            {
                return StatusCode(403);
            }
            if (FindToken != null)
            {
                using (var ZContext = _context)
                {
                    object SS = new EpisodeReport { EpisodeId = episodeReport.EpisodeId, ReportDis = episodeReport.ReportDis, SenderTokenId = FindToken.DeviceTokenId };
                    ZContext.Add(SS);
                    await ZContext.SaveChangesAsync();
                }
                return Ok();
            }
             return StatusCode(400);
            
        }

        // DELETE: api/EpisodeReports/5
        [HttpDelete("RID={RID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteEpisodeReport(int RID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 3)
            {
                var episodeReport = await _context.EpisodeReports.FindAsync(RID);
                if (episodeReport == null)
                {
                    return StatusCode(404);
                }

                _context.EpisodeReports.Remove(episodeReport);
                await _context.SaveChangesAsync();

                return StatusCode(204);
            }
            return StatusCode(403);

        }

        
    }
}
