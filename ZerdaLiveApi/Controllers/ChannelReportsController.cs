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
    public class ChannelReportsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public ChannelReportsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/ChannelReports
        [HttpGet("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<IEnumerable<ChannelReport>>> GetChannelReports(string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2) 
            { 
                return await _context.ChannelReports.ToListAsync();
            }
            return StatusCode(403);
        }

        // GET: api/ChannelReports/5
        [HttpGet("RID={RID}&UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<ChannelReport>> GetChannelReport(int RID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2)
            {
                var channelReport = await _context.ChannelReports.FindAsync(RID);

                if (channelReport == null)
                {
                    return StatusCode(404);
                }

                return channelReport;
            }
            return StatusCode(403);
        }

       
        // POST: api/ChannelReports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("DeviceToken={DeviceToken}")]
        public async Task<ActionResult<ChannelReport>> PostChannelReport(string DeviceToken, ChannelReport channelReport)
        {
            var FindToken = await _context.DeviceTokens.FirstOrDefaultAsync(c => c.DeviceToken1 == DeviceToken);
            var IsReportExist = await _context.ChannelReports.Where(c => c.ChannalId == c.ChannalId && c.SenderToken == FindToken.DeviceTokenId).ToListAsync();
            if (IsReportExist.Any())
            {
                return StatusCode(403);
            }
            if (FindToken != null)
            {
                using (var ZContext = _context)
                {
                    object SS = new ChannelReport { ChannalId = channelReport.ChannalId, ChannalReportDis = channelReport.ChannalReportDis, SenderToken = FindToken.DeviceTokenId };
                    ZContext.Add(SS);
                    await ZContext.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(400);
        }

        // DELETE: api/ChannelReports/5
        [HttpDelete("RID={RID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteChannelReport(int RID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2)
            {
                var channelReport = await _context.ChannelReports.FindAsync(RID);
                if (channelReport == null)
                {
                    return StatusCode(404);
                }

                _context.ChannelReports.Remove(channelReport);
                await _context.SaveChangesAsync();

                return StatusCode(204);
            }
            return StatusCode(403);
        }

    }
}
