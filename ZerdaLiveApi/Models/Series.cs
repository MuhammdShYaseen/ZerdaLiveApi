using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class Series
    {
        public Series()
        {
            Seasons = new HashSet<Season>();
        }

        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public string SeriesImage { get; set; }
        public string SeriesDis { get; set; }
        public int? SeriesCat { get; set; }
        public int? SeriesCountry { get; set; }
        public int? SeriesLang { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsTob { get; set; }

        public virtual FilmSnCategory SeriesCatNavigation { get; set; }
        public virtual Language SeriesLangNavigation { get; set; }
        public virtual ICollection<Season> Seasons { get; set; }
    }
}
