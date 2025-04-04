using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class Channel
    {
        public Channel()
        {
            ChannelReports = new HashSet<ChannelReport>();
        }

        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelUrl { get; set; }
        public string UserAgent { get; set; }
        public int? Category { get; set; }
        public int? Country { get; set; }
        public int? LanguageId { get; set; }
        public string ChannalIcon { get; set; }
        public bool? IsTop { get; set; }
        public bool? IsNew { get; set; }

        public virtual Category CategoryNavigation { get; set; }
        public virtual Country CountryNavigation { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<ChannelReport> ChannelReports { get; set; }
    }
}
