using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Services
{
    public class TeamService : Service
    {
        public void DeleteTeamById(int teamId)
        {
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (t:Team { Id:" + teamId + " }) DETACH DELETE t",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }
    }
}