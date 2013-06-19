using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatMarket
{
    public class Station
    {
        public Station(int stationID, int solarSystemID, string solarSystemName, int regionID, string regionName, int stationTypeID, string stationName, int corporationID, string corporationName)
        {
            Station_ID = stationID;
            SolarSystem_ID = solarSystemID;
            SolarSystem_Name = solarSystemName;
            Region_ID = regionID;
            Region_Name = regionName;
            Station_Type_ID = stationTypeID;
            Station_Name = stationName;
            Corporation_ID = corporationID;
            Corporation_Name = corporationName;
        }

        public int Station_ID { get; set; }
        public int SolarSystem_ID { get; set; }
        public string SolarSystem_Name { get; set; }
        public int Region_ID { get; set; }
        public string Region_Name { get; set; }
        public int Station_Type_ID { get; set; }
        public string Station_Name { get; set; }
        public int Corporation_ID { get; set; }
        public string Corporation_Name { get; set; }
    }
}
