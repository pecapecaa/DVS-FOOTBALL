using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace DVSleague.Services
{
    public class Service
    {
        protected GraphClient client;
        public Service()
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "DVSleague");
            try
            {
                client.Connect();
            }
            catch (Exception e)
            {
                string error = e.Message;
            }
        }
    }
}