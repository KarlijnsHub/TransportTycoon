using System;
using System.Collections.Generic;
using System.Linq;

namespace Transport_Tycoon.Models
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
        }

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

        public List<Connection> ConnectionsList { get; set; }

        public int WhichTruckShouldDrive(List<Transport> transportList)
        {
            //stap 1: check welke trucks in de factory zijn
            var trucksAvailable = transportList.Where(t => t.CurrentPlace == "Factory" && t.TypeOfPassageWay == PassageWay.Road).ToList();
            if (trucksAvailable.Count() == 2)
            {
                //zijn er 2 trucks? dan rijdt degene met laagste aantal uren
                if (trucksAvailable[0].HoursOnItsWay <= trucksAvailable[1].HoursOnItsWay)
                {
                    return trucksAvailable[0].Id;
                }
                else
                {
                    return trucksAvailable[1].Id;
                }
            }
            else if (trucksAvailable.Count() == 1)
            {
                //is er 1 truck? dan rijdt die
                return trucksAvailable[0].Id;
            }
            else if (trucksAvailable.Count() == 0)
            {
                //stap 1a: is er geen truck? check dan waar de trucks zijn en laat de dichtsbijzijnde terugrijden (de truck met minste uren onderweg als ie bij de factory is)
                var hoursToGetBackTruck1 = transportList[0].HoursOnItsWay;
                if (transportList[0].CurrentPlace == "DestinationB")
                {
                    //tel uren voor terugweg bij de truckuren
                    hoursToGetBackTruck1 += 5;
                }
                else
                {
                    hoursToGetBackTruck1 += 1;
                }

                var hoursToGetBackTruck2 = transportList[1].HoursOnItsWay;
                if (transportList[1].CurrentPlace == "DestinationB")
                {
                    hoursToGetBackTruck2 += 5;
                }
                else
                {
                    hoursToGetBackTruck2 += 1;
                }

                if (hoursToGetBackTruck1 <= hoursToGetBackTruck2)
                {
                    return transportList[0].Id;

                }
                else
                {
                    return transportList[1].Id;
                }
            }

            return 0;
        }

        public int CalcTotalHours(Transport truck1, Transport truck2, Transport boat)
        {
            int truckMaxHours = Math.Max(truck1.HoursOnItsWay, truck2.HoursOnItsWay);
            return Math.Max(truckMaxHours, boat.HoursOnItsWay + 1);
        }
    }
}
