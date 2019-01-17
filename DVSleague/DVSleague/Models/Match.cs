using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime MatchTime { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public List<Player> Scorers { get; set; }
        public List<Player> Assistents { get; set; }
    }
}