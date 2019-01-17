using DVSleague.Models;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Repository
{
    public class MatchRepository : Repository
    {   
        public List<Match> GetAllMatchesByLeagueId(int leagueId)
        {
            List<Match> leagues;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (m:Match)-[b:belongsTo]-(l:League {Id: " + leagueId + "}) RETURN m",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);

            leagues = ((IRawGraphClient)client).ExecuteGetCypherResults<Match>(query).ToList();
            return leagues;
        }
        public Match GetMatchById(int matchId)
        {
            Match match = new Match();
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (m:Match) WHERE m.Id = " + matchId + "  RETURN m",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);
            List<Match> matches = ((IRawGraphClient)client).ExecuteGetCypherResults<Match>(query).ToList();
            if (matches.Count != 0)
            {
                match = matches[0];
            }

            //domacin
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (t:Team)-[h:playsHome]->(m:Match { Id:" + match.Id + " }) RETURN t",
                                                       new Dictionary<string, object>(),
                                                       CypherResultMode.Set);
            List<Team> teams = ((IRawGraphClient)client).ExecuteGetCypherResults<Team>(query).ToList();
            if (teams.Count != 0)
            {
                match.HomeTeam = teams[0];
            }

            //gost
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (t:Team)-[a:playsAway]->(m:Match { Id:" + match.Id + " }) RETURN t",
                                                      new Dictionary<string, object>(),
                                                      CypherResultMode.Set);
            teams = ((IRawGraphClient)client).ExecuteGetCypherResults<Team>(query).ToList();
            if (teams.Count != 0)
            {
                match.AwayTeam = teams[0];
            }

            //golgeteri
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player)-[s:scorer]->(m:Match { Id: " + matchId + " }) RETURN p",
                                                     new Dictionary<string, object>(),
                                                     CypherResultMode.Set);
            List<Player> scorers = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            if (teams.Count != 0)
            {
                match.Scorers = scorers;
            }

            //asistenti
            query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player)-[a:assistent]->(m:Match { Id: " + matchId + " }) RETURN p",
                                                    new Dictionary<string, object>(),
                                                    CypherResultMode.Set);
            List<Player> assistants = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            if (teams.Count != 0)
            {
                match.Assistents = assistants;
            }

            return match;
        }
       
    }
}