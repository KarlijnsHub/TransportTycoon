using Xunit;
using FluentAssertions;

namespace Transport_Tycoon.Tests
{
    public class TransportManagerTests
    {
        [Fact]
        public void WhichTruckShouldDrive_NoMovementYet_Truck1()
        {
            var manager = new TransportManager();
            var expected = "truck1";

            var actual = manager.WhichTruckShouldDrive(manager.MeansOfTransportList);

            actual.Should().Be(expected);
        }

        [Fact]
        public void WhichTruckShouldDrive_OnlyTruck1Moved_Truck2()
        {
            var manager = new TransportManager();
            manager.MeansOfTransportList["truck1"].HoursOnItsWay = 5;
            var expected = "truck2";

            var actual = manager.WhichTruckShouldDrive(manager.MeansOfTransportList);

            actual.Should().Be(expected);
        }

        [Fact]
        public void WhichTruckShouldDrive_NoTruckAtFactory_TruckWithLeastHoursOnItsWay()
        {
            var manager = new TransportManager();
            manager.MeansOfTransportList["truck1"].CurrentPlace = "DestinationB";
            manager.MeansOfTransportList["truck1"].HoursOnItsWay = 5;
            manager.MeansOfTransportList["truck2"].CurrentPlace = "DestinationB";
            manager.MeansOfTransportList["truck2"].HoursOnItsWay = 15;

            var expected = "truck1";

            var actual = manager.WhichTruckShouldDrive(manager.MeansOfTransportList);

            actual.Should().Be(expected);
        }

        [Fact]
        public void CalcTotalHours_MostMovementByBoat_BoatHours()
        {
            var expected = 4;

            var manager = new TransportManager();
            manager.MeansOfTransportList["truck1"].HoursOnItsWay = 1;
            manager.MeansOfTransportList["truck2"].HoursOnItsWay = 2;
            manager.MeansOfTransportList["boat"].HoursOnItsWay = 3;

            var actual = manager.CalcTotalHours(manager.MeansOfTransportList["truck1"], manager.MeansOfTransportList["truck2"], manager.MeansOfTransportList["boat"]);

            actual.Should().Be(expected);
        }

        [Fact]
        public void CalcTotalHours_MostMovementByTrucks_TruckHours()
        {
            var expected = 3;

            var manager = new TransportManager();
            manager.MeansOfTransportList["truck1"].HoursOnItsWay = 3;
            manager.MeansOfTransportList["truck2"].HoursOnItsWay = 2;
            manager.MeansOfTransportList["boat"].HoursOnItsWay = 1;

            var actual = manager.CalcTotalHours(manager.MeansOfTransportList["truck1"], manager.MeansOfTransportList["truck2"], manager.MeansOfTransportList["boat"]);

            actual.Should().Be(expected);
        }
    }
}
