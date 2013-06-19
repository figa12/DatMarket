using System;

namespace DatMarket
{
    public class Order
    {
        public uint OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        public uint TypeId
        {
            get { return _typeId; }
            set { _typeId = value; }
        }

        public uint StationId
        {
            get { return _stationId; }
            set { _stationId = value; }
        }

        public uint SolarsystemId
        {
            get { return _solarsystemId; }
            set { _solarsystemId = value; }
        }

        public uint RegionId
        {
            get { return _regionId; }
            set { _regionId = value; }
        }

        public float Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public int Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        public uint QtyTotal
        {
            get { return _qtyTotal; }
            set { _qtyTotal = value; }
        }

        public uint QtyAvailable
        {
            get { return _qtyAvailable; }
            set { _qtyAvailable = value; }
        }

        public uint QtyMinimum
        {
            get { return _qtyMinimum; }
            set { _qtyMinimum = value; }
        }

        public DateTime DateIssued
        {
            get { return _dateIssued; }
            set { _dateIssued = value; }
        }

        public DateTime DateExpires
        {
            get { return _dateExpires; }
            set { _dateExpires = value; }
        }

        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public double Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public double TotalVolume
        {
            get { return (Volume * QtyAvailable); }
        }

        public string ItemName
        {
            get { return Orders.Items.Find(x => x.TypeID == _typeId).TypeName; }
        }

        public string StationName
        {
            get { return Orders.Stations.Find(x => x.Station_ID == _stationId).Station_Name; }
        }

        public string SolarSystemName
        {
            get { return Orders.Stations.Find(x => x.SolarSystem_ID == _solarsystemId).SolarSystem_Name; }
        }

        public Order(uint orderId, uint typeId, uint stationId, uint solarsystemId, uint regionId, float price, int distance, uint qtyTotal, uint qtyAvailable, uint qtyMinimum, DateTime dateIssued, DateTime dateExpires, DateTime dateCreated, double volume)
        {
            _orderId = orderId;
            _typeId = typeId;
            _stationId = stationId;
            _solarsystemId = solarsystemId;
            _regionId = regionId;
            _price = price;
            _distance = distance;
            _qtyTotal = qtyTotal;
            _qtyAvailable = qtyAvailable;
            _qtyMinimum = qtyMinimum;
            _dateIssued = dateIssued;
            _dateExpires = dateExpires;
            _dateCreated = dateCreated;
            _volume = volume;
        }

        private uint _orderId;
        private uint _typeId;
        private uint _stationId;
        private uint _solarsystemId;
        private uint _regionId;
        private float _price;
        private int _distance;
        private uint _qtyTotal;
        private uint _qtyAvailable;
        private uint _qtyMinimum;
        private DateTime _dateIssued;
        private DateTime _dateExpires;
        private DateTime _dateCreated;
        private double _volume;


    }
}
