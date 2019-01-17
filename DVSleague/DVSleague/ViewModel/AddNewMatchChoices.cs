using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVSleague.Models;

namespace DVSleague.ViewModel
{
    public class AddNewMatchChoices
    {
        public Match Match { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }
        public IEnumerable<SelectListItem> Players { get; set; }
        public List<int> Scorers { get; set; }
        public List<int> Assistants { get; set; }
    }
}