using DVSleague.Models;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Repository
{
    public class LeagueRepository : Repository
    {
        public void AddNewLeague(League league)
        {
            int maxId = GetMaxId();
            league.Id = ++maxId;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("Name", league.Name);
            queryDict.Add("Country", league.Country);
            queryDict.Add("NumberOfTeams", league.NumberOfTeams);

            string upit = "CREATE (l:League {Id:" + league.Id + ", Name:'" + league.Name
                                                            + "', Country:'" + league.Country + "', NumberOfTeams:" + league.NumberOfTeams
                                                            + "}) return l";
            var query = new Neo4jClient.Cypher.CypherQuery(upit,
                                                            queryDict, CypherResultMode.Set);

            List<League> leagues = ((IRawGraphClient)client).ExecuteGetCypherResults<League>(query).ToList();
        }
        public List<League> GetAllLeagues()
        {
            List<League> leagues;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH(l: League) RETURN l",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);

            leagues = ((IRawGraphClient)client).ExecuteGetCypherResults<League>(query).ToList();
            return leagues;
        }
        public League GetLeagueById(int leagueId)
        {
            League league = new League();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (l:League) WHERE l.Id = " + leagueId + "  RETURN l",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);
            List<League> leagues = ((IRawGraphClient)client).ExecuteGetCypherResults<League>(query).ToList();
            if (leagues.Count != 0)
            {
                league = leagues[0];
            }

            //lista timova
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (l:League { Id:" + league.Id + " })<-[:participates]-(team) RETURN team",
                                                       new Dictionary<string, object>(),
                                                       CypherResultMode.Set);
            List<Team> teams = ((IRawGraphClient)client).ExecuteGetCypherResults<Team>(query).ToList();
            if (teams.Count != 0)
            {
                league.Teams = teams;
            }

            //top skorer
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (l:League {Id:" + league.Id + "})<-[:participates]-(t:Team)<-[:member]-(p:Player)"
                                                        + " return p ORDER BY p.Goals DESC LIMIT 1",
                                                        new Dictionary<string, object>(),
                                                        CypherResultMode.Set);
            List<Player> players = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            if (players.Count != 0)
            {
                league.TopScorer = players[0];
            }

            //top asistent
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (l:League {Id:" + league.Id + "})<-[:participates]-(t:Team)<-[:member]-(p:Player)"
                                                        + " return p ORDER BY p.Assists DESC LIMIT 1",
                                                        new Dictionary<string, object>(),
                                                        CypherResultMode.Set);
            players = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            if (players.Count != 0)
            {
                league.TopAssistant = players[0];
            }

            //mvp
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (l:League {Id:" + league.Id + "})<-[:participates]-(t:Team)<-[:member]-(p:Player)"
                                                        +" return p ORDER BY (p.Assists + p.Goals) DESC LIMIT 1",
                                                        new Dictionary<string, object>(),
                                                        CypherResultMode.Set);
            players = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            if (players.Count != 0)
            {
                league.MVP = players[0];
            }

            return league;
        }

        private int GetMaxId()
        {
            int maxId;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (n:League) RETURN max(n.Id)",
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