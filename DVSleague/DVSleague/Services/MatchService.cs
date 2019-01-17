using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVSleague.Models;
using DVSleague.Repository;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace DVSleague.Services
{
    public class MatchService : Service
    {   
        private TeamRepository TeamRepository = new TeamRepository();
        private MatchRepository MatchRepository = new MatchRepository();
        private PlayerRepository PlayerRepository = new PlayerRepository();

        public IEnumerable<SelectListItem> GetTeamsForChoices(int leagueId)
        {
            List<Team> teamsList = TeamRepository.GetTeamsByLeagueId(leagueId);
            var teams = teamsList
                        .Select(t =>
                                new SelectListItem
                                {
                                    Value = t.Id.ToString(),
                                    Text = t.Name
                                });

            return new SelectList(teams, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetPlayersForChoices(int matchId)
        {
            Match match = MatchRepository.GetMatchById(matchId);
            List<Player> homePlayers = PlayerRepository.GetPlayersByTeamId(match.HomeTeam.Id);
            List<Player> awayPlayers = PlayerRepository.GetPlayersByTeamId(match.AwayTeam.Id);

            List<Player> playersList = new List<Player>();

            foreach(Player player in homePlayers)
            {
                playersList.Add(player);
            }
            foreach(Player player in awayPlayers)
            {
                playersList.Add(player);
            }

            var players = playersList
                        .Select(p =>
                                new SelectListItem
                                {
                                    Value = p.Id.ToString(),
                                    Text = p.FirstName + " " + p.LastName 
                                });

            return new SelectList(players, "Value", "Text");
        }

        public void DeleteMatchById(int matchId)
        {
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (m:Match { Id:" + matchId + " }) DETACH DELETE m",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        public int AddNewMatch(Match match, int leagueId, int homeTeamId, int guestTeamId)
        {
            int maxId = GetMaxId();
            match.Id = ++maxId;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("MatchTime", match.MatchTime);
            queryDict.Add("HomeScore", match.HomeScore);
            queryDict.Add("AwayScore", match.AwayScore);


            string prepareQuery = "CREATE (m:Match { Id:" + match.Id
                                                    + ", MatchTime:'" + match.MatchTime + "',"
                                                    + " HomeScore:" + match.HomeScore + ","
                                                    + " AwayScore:" + match.AwayScore
                                                    + "}) return m";

            var query = new Neo4jClient.Cypher.CypherQuery(prepareQuery, queryDict, CypherResultMode.Set);
            List<Match> matches = ((IRawGraphClient)client).ExecuteGetCypherResults<Match>(query).ToList();

            string relationQuery = "MATCH (m:Match),(t:Team) WHERE m.Id = " + match.Id + "  AND t.Id = " + homeTeamId
                                 + " CREATE(t) -[h: playsHome]->(m) RETURN type(h)";

            query = new Neo4jClient.Cypher.CypherQuery(relationQuery, queryDict, CypherResultMode.Set);
            List<string> response = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(query).ToList();

            relationQuery = "MATCH (m:Match),(t:Team) WHERE m.Id = " + match.Id + "  AND t.Id = " + guestTeamId
                            + " CREATE(t) -[a: playsAway]->(m) RETURN type(a)";

            query = new Neo4jClient.Cypher.CypherQuery(relationQuery, queryDict, CypherResultMode.Set);
            response = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(query).ToList();

            relationQuery = "MATCH (m:Match),(l:League) WHERE m.Id = " + match.Id + "  AND l.Id = " + leagueId
                          + " CREATE (m) -[b: belongsTo]->(l) RETURN type(b)";

            query = new Neo4jClient.Cypher.CypherQuery(relationQuery, queryDict, CypherResultMode.Set);
            response = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(query).ToList();
            return match.Id;
        }
        public void AddScorersAndAssistants(int matchId, List<int> scorersIds, List<int> assistenceIds)
        {
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            if (scorersIds != null)
            {
                foreach (int id in scorersIds)
                {
                    string relationQuery = "MATCH (m:Match),(p:Player) WHERE m.Id = " + matchId + "  AND p.Id = " + id
                                         + " CREATE (p) -[s: scorer]->(m) RETURN type(s)";

                    var query = new Neo4jClient.Cypher.CypherQuery(relationQuery, queryDict, CypherResultMode.Set);
                    ((IRawGraphClient)client).ExecuteCypher(query);

                    PlayerRepository.IncrementPlayerGoals(id);
                }
            }
            
            if (assistenceIds != null)
            {
                foreach (int id in assistenceIds)
                {
                    string relationQuery = "MATCH (m:Match),(p:Player) WHERE m.Id = " + matchId + "  AND p.Id = " + id
                                         + " CREATE (p) -[a: assistent]->(m) RETURN type(a)";

                    var query = new Neo4jClient.Cypher.CypherQuery(relationQuery, queryDict, CypherResultMode.Set);
                    ((IRawGraphClient)client).ExecuteCypher(query);
                    PlayerRepository.IncrementPlayerAssists(id);
                }
            }
            
        }
        private int GetMaxId()
        {
            int maxId;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (m:Match) RETURN max(m.Id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);
            try
            {
                maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(query).ToList().FirstOrDefault();
            }
            catch
            {
                maxId = 0;
            }

            return maxId;
        }
    }
}