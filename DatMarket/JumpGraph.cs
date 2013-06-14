using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using QuickGraph;
using QuickGraph.Algorithms;


namespace DatMarket
{
    public static class JumpGraph
    {
        public static Route GetRoute(int from, int target, double minSecurity)
        {
            // Get the graph from the database.
            BidirectionalGraph<int, Edge<int>> graph = GetGraph(minSecurity);

            // Calculate the shortest route with a constant length.
            var dijkstra = graph.ShortestPathsDijkstra((e => 1), from);

            // Query path for given vertices.
            IEnumerable<Edge<int>> path;

            // Count the amount of jumps.
            int jumpCounter = 0;
            double lowestSec = 1;

            // If a path was found, continue.
            if (dijkstra(target, out path))
                foreach (var edge in path)
                {
                    jumpCounter++;
                }

            return new Route(jumpCounter, lowestSec);
        }

        // Creates a graph from a database.
        private static BidirectionalGraph<int, Edge<int>> GetGraph(double minSecurity)
        {
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

            List<int> allowedSecurityList = JumpGraph.allowedSecurityList(minSecurity);

            // Read all database info into the variables.
            while (reader.Read())
            {
                int tempVertex = int.Parse(reader[0].ToString());
                int tempEdge = int.Parse(reader[1].ToString());

                if (!graph.Vertices.Contains(tempVertex))
                {
                    graph.AddVertex(tempVertex);
                }

                if (allowedSecurityList.Contains(tempEdge))
                    tempEdges.Add(new Edge<int>(tempVertex, tempEdge));
            }

            // Add the edges to the graph.
            foreach (var tempEdge in tempEdges)
            {
                graph.AddEdge(tempEdge);
            }

            return graph;
        }

        private static List<int> allowedSecurityList(double minSecurity)
        {
            string connectionString = "server=78.129.218.62;" + "database=eve;" + "uid=eve;" + "password=eve;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // Sql Query to get info from the database and start the reader.
            string cmdText = "SELECT solarsystemid,security FROM mapSolarSystems;";
            MySqlCommand cmd = new MySqlCommand(cmdText, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<int> tempSecurityList = new List<int>();

            // Read all database info into the variables.
            while (reader.Read())
            {
                int tempSolarSystemID = int.Parse(reader[0].ToString());
                double tempSystemSecurityLevel = double.Parse(reader[1].ToString());

                if (tempSystemSecurityLevel >= minSecurity)
                    tempSecurityList.Add(tempSolarSystemID);
            }

            return tempSecurityList;
        }
    }

    public class Route
    {
        public Route(int jumps, double lowestSec)
        {
            Jumps = jumps;
            LowestSec = lowestSec;
        }

        public double LowestSec { get; set; }
        public int Jumps { get; set; }
    }
}
