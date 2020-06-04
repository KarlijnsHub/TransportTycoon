using System;
using Xunit;
using FluentAssertions;

namespace Transport_Tycoon.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void TransportOverConnection_WrongMeansOfTransport_ArgumentException()
        {
            var pointA = "pointA";
            var conn = new Connection(pointA, "pointB", PassageWay.Road, 2);

            var boat = new Transport
            {
                Id = 3,
                Name = "boat",
                TypeOfPassageWay = PassageWay.Water,
                CurrentPlace = pointA,
                HoursOnItsWay = 0,
            };

            Action act = () => conn.TransportOverConnection(boat);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Means of transport not aloud over this connection");
        }

        [Fact]
        public void TransportOverConnection_RightMeansOfTransport_TruckMoves()
        {
            var pointA = "pointA";
            var conn = new Connection(pointA, "pointB", PassageWay.Road, 2);

            var truck = new Transport
            {
                Id = 3,
                Name = "truck",
                TypeOfPassageWay = PassageWay.Road,
                CurrentPlace = pointA,
                HoursOnItsWay = 0,
            };

            var expected = new Transport
            {
                Id = 3,
                Name = "truck",
                TypeOfPassageWay = PassageWay.Road,
                CurrentPlace = "pointB",
                HoursOnItsWay = 2,
            };

            var actual = conn.TransportOverConnection(truck);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void TransportOverConnection_TransportAtPointB_TruckMoves()
        {
            var pointB = "pointB";
            var conn = new Connection("pointA", pointB, PassageWay.Road, 2);

            var truck = new Transport
            {
                Id = 3,
                Name = "truck",
                TypeOfPassageWay = PassageWay.Road,
                CurrentPlace = pointB,
                HoursOnItsWay = 0,
            };

            var actual = conn.TransportOverConnection(truck);

            actual.Should().BeEquivalentTo(truck);
        }

        [Fact]
        public void TransportOverConnection_WrongLocation_ArgumentException()
        {
            var pointA = "pointA";
            var conn = new Connection(pointA, "pointB", PassageWay.Road, 2);

            var truck = new Transport
            {
                Id = 3,
                Name = "truck",
                TypeOfPassageWay = PassageWay.Road,
                CurrentPlace = "pointC",
                HoursOnItsWay = 0,
            };

            Action act = () => conn.TransportOverConnection(truck);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Means of transport seems to be at an invalid location");
        }
    }
}
