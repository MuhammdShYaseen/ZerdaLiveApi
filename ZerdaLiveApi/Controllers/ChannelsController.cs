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
    public class ChannelsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public ChannelsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Channels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Channel>>> GetChannels()
        {
            return await _context.Channels.ToListAsync();
        }

        // GET: api/Channels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Channel>> GetChannel(int id)
        {
            var channel = await _context.Channels.FindAsync(id);

            if (channel == null)
            {
                return StatusCode(404);
            }

            return channel;
        }

        // PUT: api/Channels/5
        [HttpPut("CID={CID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutChannel(int CID, string UserName, string Password, Channel channel)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2)
            {
                if (CID != channel.ChannelId)
                {
                    return StatusCode(400);
                }
                using (var HR = _context)
                {
                    var ss = HR.Channels.First(a => a.ChannelId == channel.ChannelId);
                    ss.ChannelName = channel.ChannelName;
                    ss.Category = channel.Category;
                    ss.Country = channel.Country;
                    ss.ChannelUrl = channel.ChannelUrl;
                    ss.UserAgent = channel.UserAgent;
                    ss.ChannalIcon = channel.ChannalIcon;
                    ss.LanguageId = channel.LanguageId;
                    ss.IsNew = channel.IsNew;
                    ss.IsTop = channel.IsTop;
                    try
                    {
                        await HR.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ChannelExists(CID))
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

        // POST: api/Channels
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Channel>> PostChannel(string UserName, string Password, Channel channel)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2)
            {

                using (var HR = _context)
                {
                    object SS = new Channel { ChannelName = channel.ChannelName, Category = channel.Category, Country = channel.Country, ChannelUrl = channel.ChannelUrl, UserAgent = channel.UserAgent, ChannalIcon = channel.ChannalIcon,LanguageId = channel.LanguageId, IsNew = channel.IsNew, IsTop = channel.IsTop };
                    HR.Add(SS);
                    await HR.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(403);
        }

        // DELETE: api/Channels/5
        [HttpDelete("CID={CID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteChannel(int CID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2)
            {
                var Chann = await _context.Channels.FindAsync(CID);
                if (Chann == null)
                {
                    return StatusCode(404);
                }
                _context.Channels.Remove(Chann);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }

        private bool ChannelExists(int id)
        {
            return _context.Channels.Any(e => e.ChannelId == id);
        }
    }
}
