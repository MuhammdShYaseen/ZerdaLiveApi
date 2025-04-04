using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class Category
    {
        public Category()
        {
            Channels = new HashSet<Channel>();
            ScMatches = new HashSet<ScMatch>();
        }

        public int CatgoryId { get; set; }
        public string CatgoryName { get; set; }

        public virtual ICollection<Channel> Channels { get; set; }
        public virtual ICollection<ScMatch> ScMatches { get; set; }
    }
}
