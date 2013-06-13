using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DatMarket
{
    public static class Orders
    {
        public static List<Order> SellOrders = new List<Order>();
        public static List<Order> BuyOrders = new List<Order>();
        public static string conStr = "server=78.129.218.62;user=eve;database=eve;port=3306;password=eve;";
        public static MySqlConnection connectionBuy = new MySqlConnection(conStr);
        public static MySqlConnection connectionSell = new MySqlConnection(conStr);
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer t = new DispatcherTimer();
        private int sellOrderItems;
        private int buyOrderItems;
        Mysql mysql = new Mysql();

        public MainWindow()
        {
            InitializeComponent();
            //getData();

            Orders.connectionBuy.Open();
            Orders.connectionSell.Open();
            sellOrderItems = mysql.getItemCount("sell_orders");
            buyOrderItems = mysql.getItemCount("buy_orders");

            Thread sellThread = new Thread(new ThreadStart(mysql.getDataSell));
            sellThread.Start();
            Thread buyThread = new Thread(new ThreadStart(mysql.getDataBuy));
            buyThread.Start();

            t.Interval = new TimeSpan(0, 0, 1);
            t.Tick += new EventHandler(TOnElapsed);
            t.Start();
        }

        private void TOnElapsed(object sender, EventArgs elapsedEventArgs)
        {
            if (Orders.SellOrders.Count == sellOrderItems && Orders.BuyOrders.Count == buyOrderItems)
            {
                Orders.connectionBuy.Close();
                Orders.connectionSell.Close();
                t.Stop();
                tLog.AppendText(string.Format("Finished loading: {0} sell orders and {1} buy orders.", sellOrderItems, buyOrderItems));
                progressBar.Visibility = Visibility.Hidden;
                
            }
            progressBar.Value = (int)(((double)(Orders.SellOrders.Count + Orders.BuyOrders.Count) / (double)(sellOrderItems + buyOrderItems)) * 100);
        }


    }
}
