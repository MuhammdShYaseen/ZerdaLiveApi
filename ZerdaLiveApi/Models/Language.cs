using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class Language
    {
        public Language()
        {
            Channels = new HashSet<Channel>();
            Films = new HashSet<Film>();
            Series = new HashSet<Series>();
        }

        public int LangId { get; set; }
        public string LangName { get; set; }

        public virtual ICollection<Channel> Channels { get; set; }
        public virtual ICollection<Film> Films { get; set; }
        public virtual ICollection<Series> Series { get; set; }
    }
}
