using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatMarket
{
    public static class ItemFinder
    {
        public static List<FoundItem> itemListFinder(int maxJumps, int shipSpace, double minSecurity, float maxPrice)
        {
            List<FoundItem> foundItems = new List<FoundItem>();

            /*
             * Det her tager cirka 1.1 mil * 30 sekunder at genneføre. Jeg tænker vi sammenligner alle ordrer i sell og buy med hinanden.
             * Og sortere dem fra der er for lave/høje i forhold til hinanden fra i vores query. Det skulle speede det her rigtigt meget op.
             * Derudover bliver den meget hurtigere når vi får vores Qty'er regnet ud og ikke bare sammenlignet forkert.
             * HELD OG FUCKING LYKKE FIGA!
             */
            foreach (var buyOrder in Orders.BuyOrders)
            {
                if (buyOrder.QtyAvailable * buyOrder.Volume >= shipSpace && buyOrder.QtyMinimum * buyOrder.Volume <= shipSpace && buyOrder.Price * buyOrder.QtyMinimum * buyOrder.Volume <= maxPrice)
                {
                    foreach (var sellOrder in Orders.SellOrders)
                    {
                        if (buyOrder.TypeId == sellOrder.TypeId && sellOrder.QtyAvailable * sellOrder.Volume >= shipSpace && sellOrder.QtyMinimum * sellOrder.Volume * sellOrder.Price <= maxPrice && sellOrder.Price < buyOrder.Price)
                        {
                            if (JumpGraph.GetRoute(sellOrder.SolarsystemId, buyOrder.SolarsystemId, minSecurity).Jumps <= maxJumps)
                            {
                                foundItems.Add(new FoundItem(buyOrder, sellOrder));
                            }
                        }
                    }
                }
            }

            return foundItems.OrderByDescending(x => x.ProfitPercent).ToList();
        }
    }

    public class FoundItem
    {
        public FoundItem(Order buyOrder, Order sellOrder)
        {
            BuyOrder = buyOrder;
            SellOrder = sellOrder;
        }

        public double ProfitPercent
        {
            get { return (BuyOrder.Price / SellOrder.Price) * 100; }
        }

        public Order BuyOrder { get; set; }

        public Order SellOrder { get; set; }
    }
}
