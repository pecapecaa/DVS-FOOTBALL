using DVSleague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Repository2
{
    public class LeagueRepository
    {
        public League GetLeagueById(int id)
        {
            Player player1 = new Player
            {
                Id = 1,
                FirstName = "Marcus",
                LastName = "Rashford",
                DateOfBirth = new DateTime(1997, 02, 02),
                Position = "SS",
                Nationality = "England",
                Height = 190,
                Weight = 80,
                Goals = 10,
                Assists = 15
            };
            Player player2 = new Player
            {
                Id = 2,
                FirstName = "Romelu",
                LastName = "Lukaku",
                DateOfBirth = new DateTime(1992, 02, 02),
                Position = "ST",
                Nationality = "Belgian",
                Height = 190,
                Weight = 90,
                Goals = 20,
                Assists = 5
            };
            League league1 = new League
            {
                Id = 1,
                Name = "Premier league",
                Country = "England",
                NumberOfTeams = 10,
            };
            Team team1 = new Team
            {
                Id = 1,
                Name = "Monaco",
                City = "Monaco",
                Country = "France",
            };
            List<Player> players = new List<Player>();
            List<Team> teams = new List<Team>();

            player1.Team = team1;
            player2.Team = team1;

            players.Add(player1);
            players.Add(player2);

            team1.Squad = players;
            team1.League = league1;
            teams.Add(team1);

            league1.TopScorer = player2;
            league1.TopAssistant = player1;
            league1.MVP = player1;
            league1.Teams = teams;

            return league1;
        }
        public List<League> GetAllLeagues()
        {
            Player player1 = new Player
            {
                Id = 1,
                FirstName = "Marcus",
                LastName = "Rashford",
                DateOfBirth = new DateTime(1997, 02, 02),
                Position = "SS",
                Nationality = "England",
                Height = 190,
                Weight = 80,
                Goals = 10,
                Assists = 15
            };
            Player player2 = new Player
            {
                Id = 2,
                FirstName = "Romelu",
                LastName = "Lukaku",
                DateOfBirth = new DateTime(1992, 02, 02),
                Position = "ST",
                Nationality = "Belgian",
                Height = 190,
                Weight = 90,
                Goals = 20,
                Assists = 5
            };
            League league1 = new League
            {
                Id = 1,
                Name = "Premier league",
                Country = "England",
                NumberOfTeams = 10,
            };
            Team team1 = new Team
            {
                Id = 1,
                Name = "Monaco",
                City = "Monaco",
                Country = "France",
            };
            List<Player> players = new List<Player>();
            List<Team> teams = new List<Team>();

            player1.Team = team1;
            player2.Team = team1;

            players.Add(player1);
            players.Add(player2);

            team1.Squad = players;
            team1.League = league1;
            teams.Add(team1);

            league1.TopScorer = player2;
            league1.TopAssistant = player1;
            league1.MVP = player1;
            league1.Teams = teams;

            List<League> leagues = new List<League>();
            leagues.Add(league1);

            return leagues;
        }


        }
}