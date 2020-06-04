using System;
using System.Collections.Generic;
using System.Linq;

namespace Transport_Tycoon
{
    public class TransportManager
    {
        public readonly string FactoryHub = "Factory";
        public readonly string PortHub = "Port";
        public readonly string DestinationAHub = "DestinationA";
        public readonly string DestinationBHub = "DestinationB";

        public TransportManager()
        {
            ConnectionsList = GetAllConnections();
            MeansOfTransportList = GetAllMeansOfTransport();
        }

        public List<Connection> ConnectionsList { get; set; }
        public Dictionary<String, Transport> MeansOfTransportList { get; set; }

        private List<Connection> GetAllConnections()
        {
            var list = new List<Connection>();

            list.Add(new Connection(FactoryHub, PortHub, PassageWay.Road, 1));
            list.Add(new Connection(PortHub, FactoryHub, PassageWay.Road, 1));
            list.Add(new Connection(FactoryHub, DestinationBHub, PassageWay.Road, 5));
            list.Add(new Connection(DestinationBHub, FactoryHub, PassageWay.Road, 5));
            list.Add(new Connection(PortHub, DestinationAHub, PassageWay.Water, 4));
            list.Add(new Connection(DestinationAHub, PortHub, PassageWay.Water, 4));

            return list;
        }

        private Dictionary<string, Transport> GetAllMeansOfTransport()
        {
            var truck1 = new Transport
            {
                Id = 1,
                Name = "truck1",
                TypeOfPassageWay = PassageWay.Road,
                CurrentPlace = FactoryHub,
                HoursOnItsWay = 0,

            };

            var truck2 = new Transport
            {
                Id = 2,
                Name = "truck2",
                TypeOfPassageWay = PassageWay.Road,
                CurrentPlace = FactoryHub,
                HoursOnItsWay = 0,
            };

            var boat = new Transport
            {
                Id = 3,
                Name = "boat",
                TypeOfPassageWay = PassageWay.Water,
                CurrentPlace = PortHub,
                HoursOnItsWay = 0,
            };

            var meansOfTransportList = new Dictionary<string, Transport>();
            meansOfTransportList.Add(truck1.Name, truck1);
            meansOfTransportList.Add(truck2.Name, truck2);
            meansOfTransportList.Add(boat.Name, boat);

            return meansOfTransportList;
        }

        public string WhichTruckShouldDrive(Dictionary<string, Transport> transportList)
        {
            var truckToDrive = string.Empty;
            var minHoursOnTheWay = -1;

            var trucks = transportList.Where(t => t.Value.TypeOfPassageWay == PassageWay.Road).ToList();

            foreach (var truck in trucks)
            {
                var truckOnItsWay = truck.Value.HoursOnItsWay;

                if (truck.Value.CurrentPlace == FactoryHub)
                {
                    if (minHoursOnTheWay < 0 || minHoursOnTheWay > truck.Value.HoursOnItsWay)
                    {
                        minHoursOnTheWay = truck.Value.HoursOnItsWay;
                        truckToDrive = truck.Value.Name;
                    }
                }
                else
                {
                    var roadBackToFactory = ConnectionsList.Where(c => c.PointA == truck.Value.CurrentPlace && c.PointB == FactoryHub).First();
                    truckOnItsWay += roadBackToFactory.Length;

                    if (minHoursOnTheWay < 0 || minHoursOnTheWay > truckOnItsWay)
                    {
                        minHoursOnTheWay = truckOnItsWay;
                        truckToDrive = truck.Value.Name;
                    }
                }
            }

            return truckToDrive;
        }

        public int CalcTotalHours(Transport truck1, Transport truck2, Transport boat)
        {
            int truckMaxHours = Math.Max(truck1.HoursOnItsWay, truck2.HoursOnItsWay);
            return Math.Max(truckMaxHours, boat.HoursOnItsWay + 1);
        }
    }
}
