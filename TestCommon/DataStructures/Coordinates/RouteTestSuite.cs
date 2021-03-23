using NUnit.Framework;
using Common.DataStructures.Coordinates;

namespace TestCommon.DataStructures.Coordinates
{
    public class RouteTestSuite
    {
        Location dockStationLocation;

        [SetUp]
        public void Setup()
        {
            dockStationLocation = new DockStationLocation(1, 3, 1);
        }

        [Test]
        public void ShouldReturnNumOfDevidersPlusOneLengthListWhenDevidersAreNotFirstOrLast()
        {
            var sut = new Route();
            sut.Add(new Location(1, 1));
            sut.Add(dockStationLocation);
            sut.Add(new Location(2, 4));
            sut.Add(dockStationLocation);
            sut.Add(new Location(2, 5));

            Assert.AreEqual(3, sut.ConvertToListDevidedBy(dockStationLocation).Count);
        }

        [Test]
        public void ShouldReturnNumOfDevidersLengthListWhenOneDeviderIsLast()
        {
            var sut = new Route();
            sut.Add(new Location(1, 1));
            sut.Add(dockStationLocation);
            sut.Add(new Location(2, 4));
            sut.Add(dockStationLocation);
            sut.Add(new Location(2, 5));
            sut.Add(dockStationLocation);

            Assert.AreEqual(3, sut.ConvertToListDevidedBy(dockStationLocation).Count);
        }

        [Test]
        public void ShouldReturnNumOfDevidersLengthListWhenOneDeviderIsFirst()
        {
            var sut = new Route();
            sut.Add(dockStationLocation);
            sut.Add(new Location(1, 1));
            sut.Add(dockStationLocation);
            sut.Add(new Location(2, 4));
            sut.Add(dockStationLocation);
            sut.Add(new Location(2, 5));

            Assert.AreEqual(3, sut.ConvertToListDevidedBy(dockStationLocation).Count);
        }
    }
}
