﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DatMarket
{
    class Mysql
    {
        public void getDataSell()
        {
            MySqlConnection connection = new MySqlConnection(Orders.conStr);
            connection.Open();

            try
            {
                /* 
                 * Vores forsøg der tager 4 år og en sommer:
                 * Indsæt input fra user i query.
                 * "SELECT sell_orders.* FROM sell_orders, eve_map_solarsystems AS map WHERE map.security > 0.5 AND map.solarsystem_id = sell_orders.solarsystem_id;"
                 */

                string sql = "SELECT * FROM sell_orders;";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.CommandTimeout = 900;
                MySqlDataReader reader = cmd.ExecuteReader();
                Orders.SellOrders = new List<Order>();
                List<VolumeHolder> volumeHolders = getVolumes();

                while (reader.Read())
                {
                    uint typeID = uint.Parse(reader[1].ToString());

                    Orders.SellOrders.Add(new Order(
                        uint.Parse(reader[0].ToString()),
                        typeID,
                        uint.Parse(reader[2].ToString()),
                        uint.Parse(reader[3].ToString()),
                        uint.Parse(reader[4].ToString()),
                        float.Parse(reader[5].ToString()),
                        int.Parse(reader[6].ToString()),
                        uint.Parse(reader[7].ToString()),
                        uint.Parse(reader[8].ToString()),
                        uint.Parse(reader[9].ToString()),
                        DateTime.Parse(reader[10].ToString()),
                        DateTime.Parse(reader[11].ToString()),
                        DateTime.Parse(reader[12].ToString()),
                        getVolume(volumeHolders, typeID)));
                }

                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            connection.Close();


        }

        private double getVolume(List<VolumeHolder> volumeHolders, uint typeID)
        {
            double result = 0;

            foreach (var volumeHolder in volumeHolders)
            {
                if (volumeHolder.TypeID == typeID)
                {
                    return volumeHolder.Volume;
                }
            }

            return result;
        }

        private List<VolumeHolder> getVolumes()
        {
            MySqlConnection connection = new MySqlConnection(Orders.conStr);
            connection.Open();

            // Sql Query to get info from the database and start the reader.
            string cmdText = string.Format("SELECT typeid,volume FROM invTypes");
            MySqlCommand cmd = new MySqlCommand(cmdText, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<VolumeHolder> volumeHolders = new List<VolumeHolder>();

            // Read all database info into the variables.
            while (reader.Read())
            {
                volumeHolders.Add(new VolumeHolder(uint.Parse(reader[0].ToString()), double.Parse(reader[1].ToString())));
            }

            connection.Close();
            return volumeHolders;
        }

        public void getDataBuy()
        {
            MySqlConnection connection = new MySqlConnection(Orders.conStr);
            connection.Open();
            try
            {
                string sql = "SELECT * FROM  `buy_orders`;";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.CommandTimeout = 900;
                MySqlDataReader reader = cmd.ExecuteReader();
                Orders.BuyOrders = new List<Order>();
                List<VolumeHolder> volumeHolders = getVolumes();

                while (reader.Read())
                {
                    uint typeID = uint.Parse(reader[1].ToString());

                    Orders.BuyOrders.Add(new Order(
                        uint.Parse(reader[0].ToString()),
                        typeID,
                        uint.Parse(reader[2].ToString()),
                        uint.Parse(reader[3].ToString()),
                        uint.Parse(reader[4].ToString()),
                        float.Parse(reader[5].ToString()),
                        int.Parse(reader[6].ToString()),
                        uint.Parse(reader[7].ToString()),
                        uint.Parse(reader[8].ToString()),
                        uint.Parse(reader[9].ToString()),
                        DateTime.Parse(reader[10].ToString()),
                        DateTime.Parse(reader[11].ToString()),
                        DateTime.Parse(reader[12].ToString()),
                        getVolume(volumeHolders, typeID)));
                }

                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            connection.Close();
        }

        public List<string> getSolarSystems()
        {
            List<string> solarsystemsList = new List<string>();
            MySqlConnection connectionGUIInfo = new MySqlConnection(Orders.conStr);
            connectionGUIInfo.Open();
            try
            {
                string solarQuery = "SELECT solarsystem_name FROM eve_map_solarsystems;";
                MySqlCommand cmd = new MySqlCommand(solarQuery, connectionGUIInfo);
                cmd.CommandTimeout = 900;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string tempRead = reader[0].ToString();
                    int x;

                    // Sort out wormholes.
                    if (tempRead.IndexOf("J") != 0 || !int.TryParse(tempRead.Substring(1), out x))
                        solarsystemsList.Add(tempRead);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            connectionGUIInfo.Close();
            return solarsystemsList;
        }

        public int getItemCount(string item)
        {
            MySqlConnection connection = new MySqlConnection(Orders.conStr);
            connection.Open();

            int toReturn = -1;

            try
            {
                string sql = string.Empty;

                if (item == "sell_orders")
                {
                    sql = "SELECT count(*) FROM  `sell_orders`;";
                }
                else if (item == "buy_orders")
                {
                    sql = "SELECT count(*) FROM  `buy_orders`;";
                }

                if (sql != string.Empty)
                {
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        toReturn = int.Parse(reader[0].ToString());
                    }


                    reader.Close();
                }
                connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            connection.Close();
            return toReturn;
        }

        public void GetAllItems()
        {
            MySqlConnection connection = new MySqlConnection(Orders.conStr);

            try
            {
                connection.Open();

                string sql = "SELECT * FROM  `invTypes`;";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string description = reader[3].ToString();

                    int graphicID = 0;
                    int raceID = 0;
                    int marketGroupID = 0;
                    int iconID = 0;

                    int.TryParse(reader[4].ToString(), out graphicID);
                    int.TryParse(reader[10].ToString(), out raceID);
                    int.TryParse(reader[13].ToString(), out marketGroupID);
                    int.TryParse(reader[15].ToString(), out iconID);


                    Orders.Items.Add(new Item(
                        int.Parse(reader[0].ToString()),
                        int.Parse(reader[1].ToString()),
                        reader[2].ToString(),
                        description,
                        graphicID,
                        double.Parse(reader[5].ToString()),
                        double.Parse(reader[6].ToString()),
                        double.Parse(reader[7].ToString()),
                        double.Parse(reader[8].ToString()),
                        int.Parse(reader[9].ToString()),
                        raceID,
                        decimal.Parse(reader[11].ToString()),
                        int.Parse(reader[12].ToString()),
                        marketGroupID,
                        double.Parse(reader[14].ToString()),
                        iconID));
                }

                reader.Close();
                connection.Close();
            }

            catch (Exception e)
            {
                throw e;
            }

            return;
        }


        public void GetAllStations()
        {
            MySqlConnection connection = new MySqlConnection(Orders.conStr);

            try
            {
                connection.Open();

                string sql = "SELECT * FROM  `eve_sta_stations`;";

                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    Orders.Stations.Add(new Station(
                        int.Parse(reader[0].ToString()),
                        int.Parse(reader[1].ToString()),
                        reader[2].ToString(),
                        int.Parse(reader[3].ToString()),
                        reader[4].ToString(),
                        int.Parse(reader[5].ToString()),
                        reader[6].ToString(),
                        int.Parse(reader[7].ToString()),
                        reader[8].ToString()));
                }

                reader.Close();
                connection.Close();
            }

            catch (Exception)
            {
            }

            return;
        }
    }

    class VolumeHolder
    {
        public uint TypeID { get; set; }
        public double Volume { get; set; }

        public VolumeHolder(uint typeID, double volume)
        {
            TypeID = typeID;
            Volume = volume;
        }
    }
}
