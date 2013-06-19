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
        public static Route GetRoute(uint from, uint target)
        {
            // Calculate the shortest route with a constant length.
            var algorithm = Orders.Graph.ShortestPathsDijkstra((e => 1), from);
            //var algorithm = Orders.Graph.ShortestPathsAStar(e => 1, e => 1, from);

            // Query path for given vertices.
            IEnumerable<Edge<uint>> path;

            // Count the amount of jumps.
            int jumpCounter = 9001;
            double lowestSec = 1;

            // If a path was found, continue.
            if (algorithm(target, out path))
            {
                jumpCounter = 0;
                foreach (var edge in path)
                {
                    jumpCounter++;
                }
            }
            return new Route(jumpCounter, lowestSec);
        }

        public static void GetGraph()
        {
            MySqlConnection connection = new MySqlConnection(Orders.conStr);
            connection.Open();

            // Create an empty graph and an empty edge list.
            Orders.Graph = new BidirectionalGraph<uint, Edge<uint>>();
            // (This is because edges cannot be made to vertexes that doesn't exist.)
            List<Edge<uint>> tempEdges = new List<Edge<uint>>();

            foreach (var solarSystem in Orders.SolarSystemList)
            {
                Orders.allowedSolarSystems.Add(solarSystem.SolarSystemID);
            }

            // Sql Query to get info from the database and start the reader.
            string cmdText = "SELECT from_solarsystem_id,to_solarsystem_id FROM eve_map_solarsystem_jumps;";
            MySqlCommand cmd = new MySqlCommand(cmdText, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            // Read all database info into the variables.
            while (reader.Read())
            {
                uint tempVertex = uint.Parse(reader[0].ToString());
                uint tempEdge = uint.Parse(reader[1].ToString());

                if (!Orders.Graph.Vertices.Contains(tempVertex) && Orders.allowedSolarSystems.Contains(tempVertex))
                {
                    Orders.Graph.AddVertex(tempVertex);
                }

                if (Orders.allowedSolarSystems.Contains(tempEdge) && Orders.allowedSolarSystems.Contains(tempVertex))
                    tempEdges.Add(new Edge<uint>(tempVertex, tempEdge));
            }

            connection.Close();

            // Add the edges to the graph.
            foreach (var tempEdge in tempEdges)
            {
                Orders.Graph.AddEdge(tempEdge);
            }

            return;
        }

        public static void CreateSolarSystems(double minSecurity)
        {
            MySqlConnection connection = new MySqlConnection(Orders.conStr);

            try
            {
                connection.Open();

                // Sql Query to get info from the database and start the reader.
                string cmdText = "SELECT solarsystem_id,security FROM eve_map_solarsystems;";
                MySqlCommand cmd = new MySqlCommand(cmdText, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                Orders.SolarSystemList = new List<SolarSystem>();

                // Read all database info into the variables.
                while (reader.Read())
                {
                    uint SolarSystemID = uint.Parse(reader[0].ToString());
                    double SystemSecurityLevel = double.Parse(reader[1].ToString());

                    if (SystemSecurityLevel >= minSecurity)
                        Orders.SolarSystemList.Add(new SolarSystem(SolarSystemID, SystemSecurityLevel));
                }
            }
            catch
            {
            }

            connection.Close();

            GetGraph();

            return;
        }
    }

    public class SolarRoute
    {
        public uint FromSolarSystemID;
        public uint ToSolarSystemID;

        public SolarRoute(uint fromSolarSystemID, uint toSolarSystemID)
        {
            FromSolarSystemID = fromSolarSystemID;
            ToSolarSystemID = fromSolarSystemID;
        }
    }

    public class SolarSystem
    {
        public SolarSystem(uint solarSystemID, double security)
        {
            SolarSystemID = solarSystemID;
            Security = security;
        }

        public uint SolarSystemID { get; set; }
        public double Security { get; set; }
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
