using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ZerdaLiveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTopicController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public NotificationTopicController(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration = (JsonConfigurationExtensions.AddJsonFile(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()), "appsettings.json").Build());
        }

        [HttpGet]
        public ActionResult<string> GetTopic()
        {
            return Configuration["FirebaseTopic:Topic"];
        }
    }
}
