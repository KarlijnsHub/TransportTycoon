using System.ComponentModel.DataAnnotations;

namespace Transport_Tycoon.Models
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
                return transport;
            }

            if (transport.CurrentPlace == PointA)
            {
                transport.CurrentPlace = PointB;
                transport.HoursOnItsWay += Length;
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
