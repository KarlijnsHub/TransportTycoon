using Xunit;
using FluentAssertions;
using Transport_Tycoon;

namespace TransportTycoon.Tests
{
    public class RouteManagerTests
    {
        [Theory]
        [InlineData("A", 5)]
        [InlineData("AB", 5)]
        [InlineData("ABB", 7)]
        [InlineData("AABABBAB", 29)]
        [InlineData("ABBBABAAABBB", 39)]
        public void ExecuteRouteTest_Success(string input, int expected)
        {
            var manager = new RouteManager(input);

            var actual = manager.ExecuteRoute();

            actual.Should().Be(expected);
        }
    }
}
