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
    public partial class MainWindow
    {
        DispatcherTimer t = new DispatcherTimer();
        DispatcherTimer e = new DispatcherTimer();
        private int sellOrderItems;
        private int buyOrderItems;
        Mysql mysql = new Mysql();
        private int Counter = 0;


        public MainWindow()
        {
            InitializeComponent();

            // Route Tester.
            //Route route = JumpGraph.GetRoute(30003498, 30000142, 1);
            Orders.connectionBuy.Open();
            Orders.connectionSell.Open();
            sellOrderItems = mysql.getItemCount("sell_orders");
            buyOrderItems = mysql.getItemCount("buy_orders");

            Thread sellThread = new Thread((mysql.getDataSell));
            sellThread.Start();
            Thread buyThread = new Thread((mysql.getDataBuy));
            buyThread.Start();

            t.Interval = new TimeSpan(0, 0, 1);
            t.Tick += TOnElapsed;
            t.Start();

            if (t.IsEnabled == false)
            {

            }
        }

        private void TOnElapsed(object sender, EventArgs elapsedEventArgs)
        {
            sellOrderItems = mysql.getItemCount("sell_orders");
            buyOrderItems = mysql.getItemCount("buy_orders");

            progressBar.Value = (int)(((double)(Orders.SellOrders.Count + Orders.BuyOrders.Count) / (double)(sellOrderItems + buyOrderItems)) * 100);

            if (Orders.SellOrders.Count >= sellOrderItems && Orders.BuyOrders.Count >= buyOrderItems)
            {
                Orders.connectionBuy.Close();
                Orders.connectionSell.Close();
                t.Stop();
                tLog.AppendText(string.Format("Finished loading: {0} sell orders and {1} buy orders.", sellOrderItems, buyOrderItems));
                progressBar.Visibility = Visibility.Hidden;


                // Det meste af det her er bare for at tælle hvor lang tid det tager at udføre. Det er ligegyldigt i sidste ende.
                Counter = 0;
                t.Interval = new TimeSpan(0, 0, 1);
                t.Tick += EOnElapsed;
                t.Start();
                List<FoundItem> items = ItemFinder.itemListFinder(50, 5000, 0.5, 100000);
            }
        }

        private void EOnElapsed(object sender, EventArgs elapsedEventArgs)
        {
            Counter += 1;
        }

        public string getSolarSystem()
        {
            return this.systemCombobox.Text.ToString();
        }

        public double getISK()
        {
            return double.Parse(this.iskTextBox.Text.ToString());
        }

        public int getMaxJumps()
        {
            return int.Parse(this.maxJumpsCombobox.Text.ToString());
        }

        public double getCargo()
        {
            return double.Parse(this.cargoTextBox.Text.ToString());
        }

        private void systemCombobox_Initialized(object sender, EventArgs e)
        {
            //Solarsystem ComboBox
            List<string> SystemList = new List<string>();
            SystemList = mysql.getSolarSystems();

            this.systemCombobox.ItemsSource = SystemList;
        }

        private void maxJumpsCombobox_Initialized(object sender, EventArgs e)
        {
            List<int> maxJumps = new List<int>() {5,10,20,30};
            this.maxJumpsCombobox.ItemsSource = maxJumps;
        }

        private void findOrderButton_Click(object sender, RoutedEventArgs e)
        {
            //trololololololo;
        }

        
    }
}
