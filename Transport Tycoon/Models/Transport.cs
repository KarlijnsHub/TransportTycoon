namespace Transport_Tycoon.Models
{
    public class Transport
    {
        public int Id { get; set; }

        public PassageWay TypeOfPassageWay { get; set; }

        public string CurrentPlace { get; set; }

        public int HoursOnItsWay { get; set; }
    }
}
