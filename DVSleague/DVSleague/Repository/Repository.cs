using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVSleague.Repository
{
    public class Repository
    {
        protected GraphClient client;
        public Repository()
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