using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class FilmSnCategory
    {
        public FilmSnCategory()
        {
            Films = new HashSet<Film>();
            Series = new HashSet<Series>();
        }

        public int CatId { get; set; }
        public string CatName { get; set; }

        public virtual ICollection<Film> Films { get; set; }
        public virtual ICollection<Series> Series { get; set; }
    }
}
