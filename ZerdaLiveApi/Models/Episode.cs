using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class Episode
    {
        public Episode()
        {
            EpisodeReports = new HashSet<EpisodeReport>();
        }

        public int EpisodesId { get; set; }
        public string EpisodesName { get; set; }
        public string EpisodesUrl { get; set; }
        public int? SeasoneId { get; set; }

        public virtual Season Seasone { get; set; }
        public virtual ICollection<EpisodeReport> EpisodeReports { get; set; }
    }
}
