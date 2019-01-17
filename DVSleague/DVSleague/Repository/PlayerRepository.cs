using DVSleague.Models;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DVSleague.Repository
{
    public class PlayerRepository : Repository
    {
        public void AddNewPlayer(Player player, int teamId)
        {
            int maxId = GetMaxId();
            player.Id = ++maxId;

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("FirstName", player.FirstName);
            queryDict.Add("LastName", player.LastName);
            queryDict.Add("DateOfBirth", player.DateOfBirth);
            queryDict.Add("Position", player.Position);
            queryDict.Add("Nationality", player.Nationality);
            queryDict.Add("Height", player.Height);
            queryDict.Add("Weight", player.Weight);
            queryDict.Add("Goals", player.Goals);
            queryDict.Add("Assists", player.Assists);

            string prepareQuery = "CREATE (p:Player { Id:" + player.Id
                                                    + ", FirstName:'" + player.FirstName + "'," 
                                                    + " LastName:'" + player.LastName + "',"
                                                    + " DateOfBirth:'" + player.DateOfBirth + "',"
                                                    + " Position:'" + player.Position + "',"
                                                    + " Nationality:'" + player.Nationality + "',"
                                                    + " Height:" + player.Height + ","
                                                    + " Weight:" + player.Weight + ","
                                                    + " Goals:" + player.Goals + ","
                                                    + " Assists:" + player.Assists
                                                    + "}) return p";

            var query = new Neo4jClient.Cypher.CypherQuery(prepareQuery, queryDict, CypherResultMode.Set);
            List<Player> players = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();

            string relationQuery = "MATCH (p:Player),(t:Team) WHERE p.Id = " + player.Id + "  AND t.Id = " + teamId
                                 + " CREATE(p) -[m: member]->(t) RETURN type(m)";

            query = new Neo4jClient.Cypher.CypherQuery(relationQuery, queryDict, CypherResultMode.Set);
            List<string> response = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(query).ToList();
        }
        public List<Player> GetAllPlayers()
        {
            List<Player> players;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH(p: Player) RETURN p",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);

            players = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            return players;
        }
        public List<Player> GetPlayersByTeamId(int teamId)
        {
            List<Player> players;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (t:Team { Id:" + teamId + "})<-[:member]-(player) RETURN player",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);

            players = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            return players;
        }
        public Player GetPlayerById(int playerId)
        {
            Player player;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player) WHERE p.Id = " + playerId + "  RETURN p",
                                                            new Dictionary<string, object>(),
                                                            CypherResultMode.Set);
            List<Player> players = ((IRawGraphClient)client).ExecuteGetCypherResults<Player>(query).ToList();
            player = players[0];

            query = new Neo4jClient.Cypher.CypherQuery("MATCH (n)<-[m:member]-(p) WHERE p.Id =" + player.Id + "  RETURN n",
                                                          new Dictionary<string, object>(),
                                                          CypherResultMode.Set);
            List<Team> teams = ((IRawGraphClient)client).ExecuteGetCypherResults<Team>(query).ToList();
            player.Team = teams[0];

            query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player { Id: " + player.Id + " })-[s:scorer]->() RETURN count(*)",
                                                        new Dictionary<string, object>(), CypherResultMode.Set);
            int goals = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(query).ToList().FirstOrDefault();
            player.Goals = goals;

            query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player { Id: " + player.Id + " })-[a:assistent]->() RETURN count(*)",
                                                       new Dictionary<string, object>(), CypherResultMode.Set);
            int assists = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(query).ToList().FirstOrDefault();
            player.Assists = assists;

            return player;
        }
        private int GetMaxId()
        {
            int maxId;
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player) RETURN max(p.Id)",
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
        public void IncrementPlayerGoals(int playerId)
        {
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player { Id:" + playerId + " }) return p.Goals",
                                                 new Dictionary<string, object>(), CypherResultMode.Set);
            int goals = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(query).ToList().FirstOrDefault();

            query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player { Id:" + playerId + " }) set p.Goals=" + (++goals),
                                                new Dictionary<string, object>(), CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }
        public void IncrementPlayerAssists(int playerId)
        {
            var query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player { Id:" + playerId + " }) return p.Assists",
                                                 new Dictionary<string, object>(), CypherResultMode.Set);
            int assists = ((IRawGraphClient)client).ExecuteGetCypherResults<int>(query).ToList().FirstOrDefault();

            query = new Neo4jClient.Cypher.CypherQuery("MATCH (p:Player { Id:" + playerId + " }) set p.Assists=" + (++assists) ,
                                           new Dictionary<string, object>(), CypherResultMode.Set);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }
        
    }
}