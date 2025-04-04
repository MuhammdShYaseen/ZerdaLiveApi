using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class ScMatch
    {
        public int MatchId { get; set; }
        public int? EventId { get; set; }
        public int? FirstTeam { get; set; }
        public int? SecondTeam { get; set; }
        public DateTime? MatchDate { get; set; }
        public int? FirstTeamGoals { get; set; }
        public int? SecondTeamGoals { get; set; }
        public int? ChannelCategory { get; set; }
        public string Commentator { get; set; }

        public virtual Category ChannelCategoryNavigation { get; set; }
        public virtual ScEvent Event { get; set; }
        public virtual ScTeam FirstTeamNavigation { get; set; }
        public virtual ScTeam SecondTeamNavigation { get; set; }
    }
}
