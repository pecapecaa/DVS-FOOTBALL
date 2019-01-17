using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Services
{
    public class LeagueService : Service
    {
        public void DeleteLeagueById(int leagueId)
        {
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (l:League { Id:" + leagueId + " }) DETACH DELETE l",
                               new Dictionary<string, object>(), CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }
    }
}