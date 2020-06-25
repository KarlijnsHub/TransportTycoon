using System.Linq;

namespace Transport_Tycoon
{
    public class RouteManager
    {
        public RouteManager(string input)
        {
            Input = input.ToLower();
            TransportPlan = Input.ToCharArray();
            Manager = new TransportManager();
        }

        public string Input { get; set; }

        public char[] TransportPlan { get; set; }

        public string ConfirmedDeliveryStatement { get; set; }

        public TransportManager Manager { get; set; }

        public Transport DrivingTruck { get; set; }

        public int ExecuteRoute()
        {
            for (int i = 0; i < TransportPlan.Length; i++)
            {
                var drivingtruckName = Manager.WhichTruckShouldDrive(Manager.MeansOfTransportList);
                DrivingTruck = Manager.MeansOfTransportList[drivingtruckName];

                //check of drivingtruck al in factory is, zo niet rijdt terug
                if (DrivingTruck.CurrentPlace != Manager.FactoryHub)
                {
                    var backToFactoryConnection = Manager.ConnectionsList.Where(c => c.PointA == DrivingTruck.CurrentPlace && c.PointB == Manager.FactoryHub).First();
                    backToFactoryConnection.TransportOverConnection(DrivingTruck);
                }

                //check of container naar A moet, zo ja, voer uit
                if (TransportPlan[i] == 'a')
                {
                    TransportOverRouteA();
                }
                else //route b
                {
                    TransportOverRouteB();
                }

                if (ConfirmedDeliveryStatement == Input)
                {
                    //Is het hetzelfde, dan complete. het voertuig dat het langst onderweg is, is de uitkomst (let op: boot +1)
                    return Manager.CalcTotalHours(Manager.MeansOfTransportList["truck1"], Manager.MeansOfTransportList["truck2"], Manager.MeansOfTransportList["boat"]);
                    
                }
            }

            return -1;
        }

        private void TransportOverRouteA()
        {
            var toPortConnection = Manager.ConnectionsList.Where(c => c.PointA == DrivingTruck.CurrentPlace && c.PointB == Manager.PortHub).First();
            DrivingTruck = toPortConnection.TransportOverConnection(DrivingTruck);

            if (Manager.MeansOfTransportList["boat"].CurrentPlace != Manager.PortHub)
            {
                var backToPortConnection = Manager.ConnectionsList.Where(c => c.PointA == Manager.MeansOfTransportList["boat"].CurrentPlace && c.PointB == Manager.PortHub).First();
                backToPortConnection.TransportOverConnection(Manager.MeansOfTransportList["boat"]);
            }

            var toAConnection = Manager.ConnectionsList.Where(c => c.PointA == DrivingTruck.CurrentPlace && c.PointB == Manager.DestinationAHub).First();
            Manager.MeansOfTransportList["boat"] = toAConnection.TransportOverConnection(Manager.MeansOfTransportList["boat"]);
            ConfirmedDeliveryStatement += "a";
        }

        private void TransportOverRouteB()
        {
            var toBConnection = Manager.ConnectionsList.Where(c => c.PointA == DrivingTruck.CurrentPlace && c.PointB == Manager.DestinationBHub).First();
            DrivingTruck = toBConnection.TransportOverConnection(DrivingTruck);
            ConfirmedDeliveryStatement += "b";
        }
    }
}
