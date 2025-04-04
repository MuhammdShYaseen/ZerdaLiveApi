using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class ScEvent
    {
        public ScEvent()
        {
            ScMatches = new HashSet<ScMatch>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventLogo { get; set; }

        public virtual ICollection<ScMatch> ScMatches { get; set; }
    }
}
