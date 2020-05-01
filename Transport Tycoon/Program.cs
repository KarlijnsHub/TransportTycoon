using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            var confirmedDeliveryStatement = string.Empty;
            Transport drivingTruck = null;

            var transportPlan = input.ToLower().ToArray();
            for (int i = 0; i < transportPlan.Length; i++)
            {
                var drivingtruckName = manager.WhichTruckShouldDrive(manager.MeansOfTransportList);
                drivingTruck = manager.MeansOfTransportList[drivingtruckName];

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

                    if (manager.MeansOfTransportList["boat"].CurrentPlace != manager.PortHub)
                    {
                        var backToPortConnection = manager.ConnectionsList.Where(c => c.PointA == manager.MeansOfTransportList["boat"].CurrentPlace && c.PointB == manager.PortHub).First();
                        backToPortConnection.TransportOverConnection(manager.MeansOfTransportList["boat"]);
                    }

                    var toAConnection = manager.ConnectionsList.Where(c => c.PointA == drivingTruck.CurrentPlace && c.PointB == manager.DestinationAHub).First();
                    manager.MeansOfTransportList["boat"] = toAConnection.TransportOverConnection(manager.MeansOfTransportList["boat"]);
                    confirmedDeliveryStatement += "a";
                }
                else //route b
                {
                    var toBConnection = manager.ConnectionsList.Where(c => c.PointA == drivingTruck.CurrentPlace && c.PointB == manager.DestinationBHub).First();
                    drivingTruck = toBConnection.TransportOverConnection(drivingTruck);
                    confirmedDeliveryStatement += "b";
                }

                if (confirmedDeliveryStatement == input.ToLower())
                {
                    //Is het hetzelfde, dan complete. het voertuig dat het langst onderweg is, is de uitkomst (let op: boot +1)
                    var result = manager.CalcTotalHours(manager.MeansOfTransportList["truck1"], manager.MeansOfTransportList["truck2"], manager.MeansOfTransportList["boat"]);
                    Console.WriteLine($"This transportplan will take {result} hours");
                }
            }
        }
    }
}
