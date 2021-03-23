using NUnit.Framework;
using Common.DataStructures.Coordinates;

namespace TestCommon.DataStructures.Coordinates
{
    public class LocationTestSuite
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AreEqualWhenHaveTheSameDockStationId()
        {
            int dockStationId = 3;
            Location loc1 = new DockStationLocation(1, 2, dockStationId);
            Location loc2 = new DockStationLocation(3, 4, dockStationId);
            Assert.IsTrue(loc1 == loc2);
        }

        [Test]
        public void AreNotEqualWhenHaveDifferentDockIds()
        {
            int dockStationId = 3;
            Location loc1 = new DockStationLocation(1, 2, dockStationId);
            Location loc2 = new DockStationLocation(3, 4, dockStationId + 1);
            Assert.IsFalse(loc1 == loc2);
        }

        [Test]
        public void AreNotEqualWhenAreNotDocksAndHaveDifferentCoordinates()
        {
            Location loc1 = new Location(1, 2);
            Location loc2 = new Location(3, 4);
            Assert.IsFalse(loc1 == loc2);
        }

        [Test]
        public void AreEqualWhenAreNotDocksAndHaveTheSameCoordinates()
        {
            Location loc1 = new Location(1, 2);
            Location loc2 = new Location(1, 2);
            Assert.IsTrue(loc1 == loc2);
        }
    }
}
