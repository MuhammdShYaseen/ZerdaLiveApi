using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class ChannelReport
    {
        public int ChannalReportId { get; set; }
        public int? ChannalId { get; set; }
        public string ChannalReportDis { get; set; }
        public int? SenderToken { get; set; }

        public virtual Channel Channal { get; set; }
        public virtual DeviceToken SenderTokenNavigation { get; set; }
    }
}
