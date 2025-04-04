using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class Film
    {
        public Film()
        {
            MoviesReports = new HashSet<MoviesReport>();
        }

        public int FilmId { get; set; }
        public string FilmeName { get; set; }
        public string FilmDis { get; set; }
        public string FilmDuration { get; set; }
        public string FilmImage { get; set; }
        public string FilmUrl { get; set; }
        public int? FilmLang { get; set; }
        public int? FilmCat { get; set; }
        public int? FilmCountry { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsTop { get; set; }

        public virtual FilmSnCategory FilmCatNavigation { get; set; }
        public virtual Language FilmLangNavigation { get; set; }
        public virtual ICollection<MoviesReport> MoviesReports { get; set; }
    }
}
