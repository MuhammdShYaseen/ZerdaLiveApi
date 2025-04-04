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
    public class DeviceTokensController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public DeviceTokensController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/DeviceTokens
        [HttpGet("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<IEnumerable<DeviceToken>>> GetDeviceTokens(string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            return await _context.DeviceTokens.ToListAsync();
        }

        // GET: api/DeviceTokens/5
        [HttpGet("DID={DID}&UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<DeviceToken>> GetDeviceToken(int DID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return Forbid();
            }
            var deviceToken = await _context.DeviceTokens.FindAsync(DID);

            if (deviceToken == null)
            {
                return NotFound();
            }

            return deviceToken;
        }

        [HttpPut("Token={Token}")]
        public async Task<IActionResult> PutDeviceToken(string Token, DeviceToken deviceToken)
        {
            if (Token != deviceToken.DeviceToken1)
            {
                return StatusCode(400);
            }
            using (_context)
            {
                var ss = _context.DeviceTokens.First(a => a.DeviceToken1 == deviceToken.DeviceToken1);
                ss.DeviceToken1 = deviceToken.DeviceToken1;
                var ID = deviceToken.DeviceTokenId;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceTokenExists(ID))
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return NoContent();
        }

        
        [HttpPost("OldToken={OldToken}")]
        public async Task<ActionResult<DeviceToken>> PostDeviceToken(string OldToken, DeviceToken deviceToken)
        {
            int ID;
            var searchresult = _context.DeviceTokens.Where(c => c.DeviceToken1 == OldToken).ToList();
            if (!searchresult.Any() || OldToken == "")
            {
                //Add New Token
                using var ZContext = _context;
                object SS = new DeviceToken { DeviceToken1 = deviceToken.DeviceToken1, CreationDate = DateTime.Now.ToUniversalTime() };
                ZContext.Add(SS);
                await ZContext.SaveChangesAsync();
                return Ok();
                //-------------------------------------------------------------------------------------------------------------

            }
            if (searchresult.Any())
            {
                using (_context)
                {
                    //Edit Token
                    var ss = _context.DeviceTokens.First(a => a.DeviceToken1 == OldToken);
                    ss.DeviceToken1 = deviceToken.DeviceToken1;
                    await _context.SaveChangesAsync();
                    //----------------------------------------------------------------------
                    try
                    {
                        
                        //Add Edit Logo
                        ID = _context.DeviceTokens.Where(c => c.DeviceToken1 == deviceToken.DeviceToken1).FirstOrDefault().DeviceTokenId;
                        var IsExistInLogo = _context.DeviceLogos.Where(c => c.DeviceTokenId == ID).Any();

                        //ADD Logo
                        if (!IsExistInLogo)
                        {
                            object AddLogo = new DeviceLogo { DeviceLogoDate = DateTime.Now.ToUniversalTime(), DeviceTokenId = ID };
                            _context.Add(AddLogo);
                            await _context.SaveChangesAsync();
                        }

                        //Edit Logo
                        if (IsExistInLogo)
                        {
                            var EditDeviceLogo = _context.DeviceLogos.First(c => c.DeviceTokenId == ID);
                            EditDeviceLogo.DeviceLogoDate = DateTime.Now.ToUniversalTime();
                            await _context.SaveChangesAsync();
                        }
                        return Ok();
                    }

                    catch (DbUpdateConcurrencyException)
                    {
                        ID = _context.DeviceTokens.Where(c => c.DeviceToken1 == deviceToken.DeviceToken1).FirstOrDefault().DeviceTokenId;
                        if (!DeviceTokenExists(ID))
                        {
                            return StatusCode(404);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return StatusCode(400);
        }

        // DELETE: api/DeviceTokens/5
        [HttpDelete("DID={DID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteDeviceToken(int DID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var deviceToken = await _context.DeviceTokens.FindAsync(DID);
            if (deviceToken == null)
            {
                return StatusCode(404);
            }
            _context.DeviceTokens.Remove(deviceToken);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DeviceTokenExists(int id)
        {
            return _context.DeviceTokens.Any(e => e.DeviceTokenId == id);
        }

    }
}
