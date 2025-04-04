using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class Season
    {
        public Season()
        {
            Episodes = new HashSet<Episode>();
        }

        public int SeasonsId { get; set; }
        public string SeasonsName { get; set; }
        public string SeasonsImage { get; set; }
        public string SeasonsDis { get; set; }
        public int? SeriesId { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsTob { get; set; }

        public virtual Series Series { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
    }
}
