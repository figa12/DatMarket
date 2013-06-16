using System;
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

            try
            {
                /* 
                 * Vores forsøg der tager 4 år og en sommer:
                 * Indsæt input fra user i query.
                 */
                
                string sql = "SELECT sell_orders.* FROM sell_orders, eve_map_solarsystems AS map WHERE map.security > 0.5 AND map.solarsystem_id = sell_orders.solarsystem_id;";

                MySqlCommand cmd = new MySqlCommand(sql, Orders.connectionSell);
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
        }

        private double  getVolume(List<VolumeHolder> volumeHolders, uint typeID)
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
            try
            {
                string sql = "SELECT * FROM  `buy_orders`;";

                MySqlCommand cmd = new MySqlCommand(sql, Orders.connectionBuy);
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
        }

        public List<string> getSolarSystems()
        {
            List<string> solarsystemsList = new List<string>();
            MySqlConnection connectionGUIInfo = new MySqlConnection(Orders.conStr);
            connectionGUIInfo.Open();
            try
            {
                string solarQuery = "SELECT solarsystem_name FROM eve_map_solarsystems ;";
                MySqlCommand cmd = new MySqlCommand(solarQuery, connectionGUIInfo);
                cmd.CommandTimeout = 900;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    solarsystemsList.Add(reader[0].ToString());
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
                    MySqlCommand cmd = new MySqlCommand(sql, Orders.connectionBuy);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        toReturn = int.Parse(reader[0].ToString());
                    }


                    reader.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return toReturn;
        }
    }

    class VolumeHolder
    {
        public uint TypeID { get; set; }
        public double Volume{ get; set; }

        public VolumeHolder(uint typeID, double volume)
        {
            TypeID = typeID;
            Volume = volume;
        }
    }
}
