using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MS.Internal.Xml.XPath;
using MySql.Data;
using MySql.Data.MySqlClient;
using QuickGraph;

namespace DatMarket
{
    public interface IWindowInterface
    {
        void appendLog(String txt);
    }

    public static class Orders
    {
        public static List<Order> SellOrders = new List<Order>();
        public static List<Order> BuyOrders = new List<Order>();
        public static string conStr = "server=78.129.218.62;user=eve;database=eve;port=3306;password=eve;";
        public static List<SolarSystem> SolarSystemList = new List<SolarSystem>();
        public static List<SolarRoute> SolarRoutes = new List<SolarRoute>();
        public static BidirectionalGraph<uint, Edge<uint>> Graph = new BidirectionalGraph<uint, Edge<uint>>();
        public static List<uint> allowedSolarSystems = new List<uint>();
        public static List<Item> Items = new List<Item>();
        public static List<Station> Stations = new List<Station>();

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IWindowInterface
    {

        public void appendLog(String txt)
        {
            tLog.AppendText(txt);
        }

        DispatcherTimer t = new DispatcherTimer();
        private int sellOrderItems;
        private int buyOrderItems;
        Mysql mysql = new Mysql();

        public MainWindow()
        {
            InitializeComponent();

            // Laver alle lister. Tager minSecurity.
            CreateLists(0.5);

            Thread sellThread = new Thread((mysql.getDataSell));
            sellThread.Start();
            Thread buyThread = new Thread((mysql.getDataBuy));
            buyThread.Start();

            t.Interval = new TimeSpan(0, 0, 1);
            t.Tick += TOnElapsed;
            t.Start();
        }

        private void CreateLists(double minSecurity)
        {
            sellOrderItems = mysql.getItemCount("sell_orders");
            buyOrderItems = mysql.getItemCount("buy_orders");

            mysql.GetAllItems();
            mysql.GetAllStations();

            // Tager fra et security og op efter
            JumpGraph.CreateSolarSystems(minSecurity);
        }

        private void TOnElapsed(object sender, EventArgs elapsedEventArgs)
        {
            if (Orders.SellOrders.Count >= sellOrderItems && Orders.BuyOrders.Count >= buyOrderItems)
            {
                t.Stop();
                tLog.AppendText(string.Format("Finished loading: {0} sell orders and {1} buy orders.", sellOrderItems, buyOrderItems));
                progressBar.Visibility = Visibility.Hidden;

                List<FoundItem> items = ItemFinder.itemListFinder(10, 5000, 0.5, 100000);
                tLog.AppendText("DONE MOTHERFUCKER");

                ListView.ItemsSource = items;

            }
            progressBar.Value = (int)(((double)(Orders.SellOrders.Count + Orders.BuyOrders.Count) / (double)(sellOrderItems + buyOrderItems)) * 100);
        }

        public string getSolarSystem()
        {
            return systemCombobox.Text;
        }

        public double getISK()
        {
            double x = 0;
            if (!double.TryParse(iskTextBox.Text, out x))
            {
                MessageBox.Show(x + " is not a valid number for ISK.");
            }

            return x;
        }

        public int getMaxJumps()
        {
            int x = 0;
            if (!int.TryParse(maxJumpsCombobox.Text, out x))
            {
                MessageBox.Show(x + " is not a valid number for max jumps.");
            }

            return x;
        }

        public double getCargo()
        {
            double x = 0;
            if (!double.TryParse(cargoTextBox.Text, out x))
            {
                MessageBox.Show(x + " is not a valid number for Cargo Hold.");
            }

            return x;
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
            List<int> maxJumps = new List<int>() { 5, 10, 20, 30 };
            this.maxJumpsCombobox.ItemsSource = maxJumps;
        }

        private void findOrderButton_Click(object sender, RoutedEventArgs e)
        {
            //trololololololo;
        }


    }
}
