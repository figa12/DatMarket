using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace DatMarket
{
    public static class ItemFinder
    {
        public static List<FoundItem> foundItems;
        private static ConcurrentBag<FoundItem> baggedItems = new ConcurrentBag<FoundItem>();
        public static int buyCounter = 0;
        public static int sellCounter = 0;
        public static int threadsleft = 0;
        public static int threadsmax = 0;
        



        public static List<FoundItem> itemListFinder(int maxJumps, int shipSpace, double minSecurity, float maxPrice)
        {
            foundItems = new List<FoundItem>();

            /*
             * Det her tager cirka 1.1 mil * 30 sekunder at genneføre. Jeg tænker vi sammenligner alle ordrer i sell og buy med hinanden.
             * Og sortere dem fra der er for lave/høje i forhold til hinanden fra i vores query. Det skulle speede det her rigtigt meget op.
             * Derudover bliver den meget hurtigere når vi får vores Qty'er regnet ud og ikke bare sammenlignet forkert.
             * HELD OG FUCKING LYKKE FIGA!
             */
            int i = 0;

            Parallel.ForEach(Orders.BuyOrders, new ParallelOptions { MaxDegreeOfParallelism = 100 }, buyOrder =>
                                                   {

                                                       buyCounter++;
                                                       if (buyOrder.TotalVolume >= shipSpace &&
                                                           buyOrder.QtyMinimum * buyOrder.Volume <= shipSpace &&
                                                           buyOrder.Price * buyOrder.QtyMinimum <= maxPrice &&
                                                           Orders.allowedSolarSystems.Contains(buyOrder.SolarsystemId))
                                                       {
                                                           checkSellOrders(new ObjectState(buyOrder, maxJumps, shipSpace, minSecurity, maxPrice));
                                                       }
                                                   });

            //foreach (var e in doneEvents) e.WaitOne();
            foundItems = baggedItems.ToList();

            return ItemFinder.foundItems.OrderByDescending(x => x.ProfitPercent).ThenByDescending(x => x.ProfitPoints).ToList();
        }


        private static void checkSellOrders(Object args)
        {

            //IWindowInterface windowInterface = new MainWindow();

            ObjectState state = (ObjectState)args;
            Order buyOrder = state.BuyOrder;

            foreach (var sellOrder in Orders.SellOrders)
            {
                if (buyOrder.TypeId == sellOrder.TypeId &&
                    sellOrder.TotalVolume >= state.ShipSpace && 
                    sellOrder.QtyMinimum * sellOrder.Price <= state.MaxPrice && 
                    sellOrder.Price < buyOrder.Price && 
                    ((buyOrder.Price / sellOrder.Price) * 100) > 100  && 
                    Orders.allowedSolarSystems.Contains(sellOrder.SolarsystemId))
                {
                    /*  ATTENTION:
                     * GetRoute her skal laves så den kun henter databasen 1 gang, ellers har den 64 connections åben på én gang og det vil den ikke.
                     */
                    int jumps = JumpGraph.GetRoute(sellOrder.SolarsystemId, buyOrder.SolarsystemId).Jumps;
                    if (jumps <= state.MaxJumps)
                    {
                        sellCounter++;
                        baggedItems.Add(new FoundItem(buyOrder, sellOrder, jumps));
                        //ItemFinder.foundItems.Add(new FoundItem(buyOrder, sellOrder));

                        //windowInterface.appendLog((foundItems.Count).ToString());
                    }
                }
            }
            threadsleft++;
        }
    }

    public class FoundItem
    {
        public FoundItem(Order buyOrder, Order sellOrder, int jumps)
        {
            BuyOrder = buyOrder;
            SellOrder = sellOrder;
            Jumps = jumps;
        }

        public int Jumps { get; set; }

        public double ProfitPercent
        {
            get { return Math.Round((BuyOrder.Price / SellOrder.Price) * 100 - 100, 2); }
        }

        public double ProfitPoints
        {
            get { return Math.Round(ProfitPercent / Jumps, 2); }
        }
        
        
        public Order BuyOrder { get; set; }

        public Order SellOrder { get; set; }
    }

    public class ObjectState
    {
        public ObjectState(Order buyOrder, int maxJumps, int shipSpace, double minSecurity, float maxPrice)
        {
            BuyOrder = buyOrder;
            MaxJumps = maxJumps;
            ShipSpace = shipSpace;
            MinSecurity = minSecurity;
            MaxPrice = maxPrice;
        }

        public Order BuyOrder { get; set; }
        public int MaxJumps { get; set; }
        public int ShipSpace { get; set; }
        public double MinSecurity { get; set; }
        public float MaxPrice { get; set; }

    }
}
