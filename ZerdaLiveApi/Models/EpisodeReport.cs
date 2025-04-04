using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class EpisodeReport
    {
        public int EpisodeReportId { get; set; }
        public int? EpisodeId { get; set; }
        public string ReportDis { get; set; }
        public int? SenderTokenId { get; set; }

        public virtual Episode Episode { get; set; }
        public virtual DeviceToken SenderToken { get; set; }
    }
}
