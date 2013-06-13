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
                string sql = "SELECT * FROM  `sell_orders`;";

                MySqlCommand cmd = new MySqlCommand(sql, Orders.connectionSell);
                cmd.CommandTimeout = 900;
                MySqlDataReader reader = cmd.ExecuteReader();
                Orders.SellOrders = new List<Order>();

                while (reader.Read())
                {
                    Orders.SellOrders.Add(new Order(
                        uint.Parse(reader[0].ToString()),
                        uint.Parse(reader[1].ToString()),
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
                        DateTime.Parse(reader[12].ToString())));
                }

                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
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

                while (reader.Read())
                {
                    Orders.BuyOrders.Add(new Order(
                        uint.Parse(reader[0].ToString()),
                        uint.Parse(reader[1].ToString()),
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
                        DateTime.Parse(reader[12].ToString())));
                }

                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
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
}
