using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Services 
{
    public class PlayerService : Service
    {
        public void DeletePlayerById(int playerId)
        {
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player { Id:" + playerId + " }) DETACH DELETE p",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }
    }
}