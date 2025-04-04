using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class DeviceToken
    {
        public DeviceToken()
        {
            ChannelReports = new HashSet<ChannelReport>();
            DeviceLogos = new HashSet<DeviceLogo>();
            EpisodeReports = new HashSet<EpisodeReport>();
            MoviesReports = new HashSet<MoviesReport>();
        }

        public int DeviceTokenId { get; set; }
        public string DeviceToken1 { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual ICollection<ChannelReport> ChannelReports { get; set; }
        public virtual ICollection<DeviceLogo> DeviceLogos { get; set; }
        public virtual ICollection<EpisodeReport> EpisodeReports { get; set; }
        public virtual ICollection<MoviesReport> MoviesReports { get; set; }
    }
}
