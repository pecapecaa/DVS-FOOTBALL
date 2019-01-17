using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVSleague.Models;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace DVSleague.Repository
{
    public class TeamRepository : Repository
    {
        public Team AddNewTeam(Team team, int leagueId)
        {
            int maxId = GetMaxId();
            team.Id = ++maxId;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", team.Name);
            queryDict.Add("City", team.City);
            queryDict.Add("Country", team.Country);

            string prepareQuery = "CREATE (t:Team {Id:" + team.Id + ", Name:'" + team.Name
                                                            + "', City:'" + team.City + "', Country:'" + team.Country
                                                            + "'}) return t";
            var query = new Neo4jClient.Cypher.CypherQuery(prepareQuery, queryDict, CypherResultMode.Set);

            List<Team> teams = ((IRawGraphClient)client).ExecuteGetCypherResults<Team>(query).ToList();

            string relationQuery = "MATCH (t:Team),(l:League) WHERE t.Id = " +team.Id + "  AND l.Id = " + leagueId + " CREATE(t) -[p: participates]->(l) RETURN type(p)";

            query = new Neo4jClient.Cypher.CypherQuery(relationQuery, queryDict, CypherResultMode.Set);

            List<string> response = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(query).ToList();

            return teams[0];
        }
        public List<Team> GetAllTeams()
        {
            List<Team> teams;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH(t: Teams) RETURN t",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);

            teams = ((IRawGraphClient)client).ExecuteGetCypherResults<Team>(query).ToList();
            return teams;
        }
        public List<Team> GetTeamsByLeagueId(int leagueId)
        {
            List<Team> teams;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (l:League { Id:" + leagueId + "})<-[:participates]-(team) RETURN team",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);

            teams = ((IRawGraphClient)client).ExecuteGetCypherResults<Team>(query).ToList();
            return teams;
        }
        public Team GetTeamById(int teamId)
        {
            Team team= new Team();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (t:Team) WHERE t.Id = " + teamId + "  RETURN t",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);
            List<Team> teams = ((IRawGraphClient)client).ExecuteGetCypherResults<Team>(query).ToList();
            if (teams.Count != 0)
            {
                team = teams[0];
            }
         
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (t:Team { Id:" + team.Id + " })-[:participates]->(league) RETURN league",
                                                          new Dictionary<string, object>(),
                                                          CypherResultMode.Set);
            List<League> leagues = ((IRawGraphClient)client).ExecuteGetCypherResults<League>(query).ToList();
            if (leagues.Count != 0)
            {
                team.League = leagues[0];
            }

            query = new Neo4jClient.Cypher.CypherQuery("MATCH (t:Team { Id:" + team.Id + " })<-[:member]-(player) RETURN player",
                                                        new Dictionary<string, object>(),
                                                        CypherResultMode.Set);
            List<Player> players = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            if (players.Count != 0)
            {
                team.Squad = players;
            }

            return team;
        }
        private int GetMaxId()
        {
            int maxId;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (t:Team) RETURN max(t.Id)",
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