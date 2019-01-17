using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Models
{
    public class Player
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Position { get; set; }
        public String Nationality { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public Team Team { get; set; }
    }
}