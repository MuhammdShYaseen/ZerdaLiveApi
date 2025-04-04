using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class Country
    {
        public Country()
        {
            Channels = new HashSet<Channel>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryFlag { get; set; }

        public virtual ICollection<Channel> Channels { get; set; }
    }
}
