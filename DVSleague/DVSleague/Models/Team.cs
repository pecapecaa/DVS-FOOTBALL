using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Models
{
    public class Team
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public List<Player> Squad { get; set; }
        public League League { get; set; }

    }
}