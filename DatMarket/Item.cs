using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatMarket
{
    public class Item
    {
        public int TypeID { get; set; }
        public int GroupID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int GraphicID { get; set; }
        public double Radius { get; set; }
        public double Mass { get; set; }
        public double Volume { get; set; }
        public double Capacity { get; set; }
        public int PortionSize { get; set; }
        public int RaceID { get; set; }
        public decimal BasePrice { get; set; }
        public int Published { get; set; }
        public int MarketGroupID { get; set; }
        public double ChanceOfDuplicating { get; set; }
        public int IconID { get; set; }

        public Item(int typeID, int groupID, string typeName, string description, int graphicID, double radius, double mass, double volume, double capacity, int portionSize, int raceID, decimal basePrice, int published, int marketGroupID, double chanceOfDuplicating, int iconID)
        {
            TypeID = typeID;
            GroupID = groupID;
            TypeName = typeName;
            Description = description;
            GraphicID = graphicID;
            Radius = radius;
            Mass = mass;
            Volume = volume;
            Capacity = capacity;
            PortionSize = portionSize;
            RaceID = raceID;
            BasePrice = basePrice;
            Published = published;
            MarketGroupID = marketGroupID;
            ChanceOfDuplicating = chanceOfDuplicating;
            IconID = iconID;
        }
    }
}
