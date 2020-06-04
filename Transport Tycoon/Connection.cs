using System;

namespace Transport_Tycoon
{
    public class Connection
    {
        public Connection(string pointA, string pointB, PassageWay typeOfPassage, int length)
        {
            PointA = pointA;
            PointB = pointB;
            TypeOfPassage = typeOfPassage;
            Length = length;
        }

        public PassageWay TypeOfPassage { get; set; }

        public int Length { get; set; }

        public string PointA { get; set; }

        public string PointB { get; set; }

        public Transport TransportOverConnection(Transport transport)
        {
            if (TypeOfPassage != transport.TypeOfPassageWay)
            {
                throw new ArgumentException("Means of transport not aloud over this connection");
            }

            if (transport.CurrentPlace == PointA)
            {
                transport.CurrentPlace = PointB;
                transport.HoursOnItsWay += Length;
            }
            else if (transport.CurrentPlace != PointA && transport.CurrentPlace != PointB)
            {
                throw new ArgumentException("Means of transport seems to be at an invalid location");
            }

            return transport;
        }
    }

    public enum PassageWay
    {
        Road,
        Water
    }
}
