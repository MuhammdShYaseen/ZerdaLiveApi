using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZerdaLiveApi.Attributes;
using ZerdaLiveApi.FirebaseNotification;
using ZerdaLiveApi.Helpper;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendNotificationController :  ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        private readonly SendCloudMessage SendFCMessage;
        public IConfiguration Configuration { get; }

        public SendNotificationController(UserRole URolee, SendCloudMessage SendMessage, ZerdaLiveContext context, IConfiguration configuration)
        {
            URole = URolee;
            SendFCMessage = SendMessage;
            _context = context;
            Configuration = configuration;
            Configuration = (JsonConfigurationExtensions.AddJsonFile(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()), "appsettings.json").Build());
        }
        [HttpPost("DeviceID={DeviceID}&UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<NotificationFC>> SendToOneDevice(int DeviceID, string UserName, string Password,  NotificationFC notification)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var DeviceToken = await _context.DeviceTokens.FindAsync(DeviceID);
            var TokenID = DeviceToken.DeviceToken1;
            SendFCMessage.SendToOneUser(TokenID, new Notification { Title = notification.Title, Body = notification.Body, ImageUrl = notification.ImageUrl }, notification.Dataa);
            return Ok();
        }

        [HttpPost("UserName={UserName}&Password={Password}")]
        public ActionResult<NotificationFC> SendToAllDevice(string UserName, string Password, NotificationFC notification)
        {
            string Topic = Configuration["FirebaseTopic:Topic"];
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            SendFCMessage.SendToAllUser(new Notification {Title = notification.Title, Body = notification.Body, ImageUrl = notification.ImageUrl}, notification.Dataa,Topic);
            return Ok();
        }


    }
}
