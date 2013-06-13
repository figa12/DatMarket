using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using QuickGraph;
using QuickGraph.Algorithms;


namespace DatMarket
{
    class JumpGraph
    {
        int GetRoute(int from, int target)
        {
            // Get the graph from the database.
            BidirectionalGraph<int, Edge<int>> graph = GetGraph();
 
            // Calculate the shortest route with a constant length.
            var dijkstra = graph.ShortestPathsDijkstra((e => 1), from);
 
            // Query path for given vertices.
            IEnumerable<Edge<int>> path;
 
            // Count the amount of jumps.
            int jumpCounter = 0;
 
            // If a path was found, continue.
            if (dijkstra(target, out path))
                foreach (var edge in path)
                {
                    jumpCounter++;
                }
 
            return jumpCounter;
        }
 
        // Creates a graph from a database.
        private BidirectionalGraph<int, Edge<int>> GetGraph()
        {
            // Connection string and mysql connection.
            string connectionString = "server=78.129.218.62;" + "database=eve;" + "uid=eve;" + "password=eve;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
 
            // Sql Query to get info from the database and start the reader.
            string cmdText = "SELECT from_solarsystem_id,to_solarsystem_id FROM eve_map_solarsystem_jumps;";
            MySqlCommand cmd = new MySqlCommand(cmdText, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
 
            // Create an empty graph and an empty edge list.
            BidirectionalGraph<int, Edge<int>> graph = new BidirectionalGraph<int, Edge<int>>();
            // (This is because edges cannot be made to vertexes that doesn't exist.)
            List<Edge<int>> tempEdges = new List<Edge<int>>();
 
            // Read all database info into the variables.
            while (reader.Read())
            {
                int tempVertex = int.Parse(reader[0].ToString());
                int tempEdge = int.Parse(reader[1].ToString());
                if (!graph.Vertices.Contains(tempVertex))
                {
                    graph.AddVertex(tempVertex);
                }
                tempEdges.Add(new Edge<int>(tempVertex, tempEdge));
            }
            connection.Close();
 
            // Add the edges to the graph.
            foreach (var tempEdge in tempEdges)
            {
                graph.AddEdge(tempEdge);
            }
 
            return graph;
        }
    }
}
