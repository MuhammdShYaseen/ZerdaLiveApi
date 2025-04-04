using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class ScTeam
    {
        public ScTeam()
        {
            ScMatchFirstTeamNavigations = new HashSet<ScMatch>();
            ScMatchSecondTeamNavigations = new HashSet<ScMatch>();
        }

        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }

        public virtual ICollection<ScMatch> ScMatchFirstTeamNavigations { get; set; }
        public virtual ICollection<ScMatch> ScMatchSecondTeamNavigations { get; set; }
    }
}
