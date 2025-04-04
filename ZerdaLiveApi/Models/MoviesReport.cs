using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class MoviesReport
    {
        public int MovieReportId { get; set; }
        public int? MovieId { get; set; }
        public string ReportDis { get; set; }
        public int? SenderTokenId { get; set; }

        public virtual Film Movie { get; set; }
        public virtual DeviceToken SenderToken { get; set; }
    }
}
