using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Models
{
    public class League
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Country { get; set; }
        public int NumberOfTeams { get; set; }
        public List<Team> Teams { get; set; }
        public Player TopScorer { get; set; }
        public Player TopAssistant { get; set; }
        public Player MVP { get; set; }
    }
}