using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Transport_Tycoon.Models;

namespace Transport_Tycoon
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;

            Console.WriteLine("Hello, what is the tranport plan?");
            input = Console.ReadLine();

            while (!Regex.IsMatch(input, @"^[a-bA-B]+$"))
            {
                Console.WriteLine("That doesn't sound right. What is the tranport plan?");
                input = Console.ReadLine();
            }

            var manager = new TransportManager();
            var truck1 = new Transport();
            truck1.TypeOfPassageWay = PassageWay.Road;
            truck1.CurrentPlace = manager.FactoryHub;
            truck1.HoursOnItsWay = 0;
            truck1.Id = 1;
            var truck2 = new Transport();
            truck2.TypeOfPassageWay = PassageWay.Road;
            truck2.CurrentPlace = manager.FactoryHub;
            truck2.HoursOnItsWay = 0;
            truck2.Id = 2;
            var boat = new Transport();
            boat.TypeOfPassageWay = PassageWay.Water;
            boat.CurrentPlace = manager.PortHub;
            boat.HoursOnItsWay = 0;
            boat.Id = 3;

            var transportList = new List<Transport>();
            transportList.Add(truck1);
            transportList.Add(truck2);
            transportList.Add(boat);

            var confirmedDeliveryStatement = string.Empty;
            Transport drivingTruck = null;

            var transportPlan = input.ToLower().ToArray();
            for (int i = 0; i < transportPlan.Length; i++)
            {
                var drivingtruckId = manager.WhichTruckShouldDrive(transportList);
                drivingTruck = transportList.Where(t => t.Id == drivingtruckId).First();

                //check of drivingtruck al in factory is, zo niet rijdt terug
                if (drivingTruck.CurrentPlace != manager.FactoryHub)
                {
                    var backToFactoryConnection = manager.ConnectionsList.Where(c => c.PointA == drivingTruck.CurrentPlace && c.PointB == manager.FactoryHub).First();
                    backToFactoryConnection.TransportOverConnection(drivingTruck);
                }

                //check of container naar A moet, zo ja, voer uit
                if (transportPlan[i] == 'a')
                {
                    var toPortConnection = manager.ConnectionsList.Where(c => c.PointA == drivingTruck.CurrentPlace && c.PointB == manager.PortHub).First();
                    drivingTruck = toPortConnection.TransportOverConnection(drivingTruck);

                    if (boat.CurrentPlace != manager.PortHub)
                    {
                        var backToPortConnection = manager.ConnectionsList.Where(c => c.PointA == boat.CurrentPlace && c.PointB == manager.PortHub).First();
                        backToPortConnection.TransportOverConnection(boat);
                    }

                    var toAConnection = manager.ConnectionsList.Where(c => c.PointA == drivingTruck.CurrentPlace && c.PointB == manager.DestinationAHub).First();
                    boat = toAConnection.TransportOverConnection(boat);
                    confirmedDeliveryStatement += "a";
                }
                else
                {
                    var toBConnection = manager.ConnectionsList.Where(c => c.PointA == drivingTruck.CurrentPlace && c.PointB == manager.DestinationBHub).First();
                    drivingTruck = toBConnection.TransportOverConnection(drivingTruck);
                    confirmedDeliveryStatement += "b";
                }

                if (confirmedDeliveryStatement == input.ToLower())
                {
                    //Is het hetzelfde, dan complete. het voertuig dat het langst onderweg is, is de uitkomst (let op: boot +1)
                    var result = manager.CalcTotalHours(truck1, truck2, boat);
                    Console.WriteLine($"This transportplan will take {result} hours");
                }
            }
        }
    }
}
