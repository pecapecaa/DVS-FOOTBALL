using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVSleague.Models;
using DVSleague.Repository;
using DVSleague.Services;

namespace DVSleague.Controllers
{
    public class PlayerController : Controller
    {
        private TeamRepository TeamRepository = new TeamRepository();
        private PlayerRepository PlayerRepository = new PlayerRepository();
        private PlayerService PlayerService = new PlayerService();
        
        // GET: Player
        public ActionResult Index()
        {
            return View();
        }

        [Route("player/{id}", Name = "player_details")]
        public ActionResult PlayerDetails(int id) //     view/home/Player/PlayerDetails
        {
            Player player = PlayerRepository.GetPlayerById(id);
            return View(player);
        }

        [Route("team/{teamId}/players", Name = "show_players")]
        public ActionResult ShowPlayersByTeam(int teamId)        //  view/home/Player/ShowPlayersByTeam
        {
            List<Player> players = PlayerRepository.GetPlayersByTeamId(teamId);
            ViewBag.TeamId= teamId;
            return View(players);
        }

        [Route("team/{teamId}/add-new-player", Name = "add_new_player")]
        public ActionResult AddNewPlayer(int teamId)
        {
            ViewBag.TeamId = teamId;
            return View();
        }

        [Route("add-player")]
        public ActionResult AddPlayer(Player player, int teamId)
        {
            Team team = TeamRepository.GetTeamById(teamId);
            player.Team = team;
            
            PlayerRepository.AddNewPlayer(player, teamId);
            return RedirectToAction("PlayerDetails", new { id = player.Id });
        }
        [Route("delete-player/{id}", Name= "delete_player")]
        public ActionResult DeletePlayer(int id,int tId)
        {
            PlayerService.DeletePlayerById(id);
           
            return RedirectToAction("ShowPlayersByTeam", new { teamId = tId });
        }

    }
}