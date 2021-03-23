using Common.DataStructures.Coordinates;
using NUnit.Framework;

namespace TestCommon.DataStructures.Coordinates
{
    class PolygonTestSuite
    {
        Polygon polygonCW, polygonCCW;

        [SetUp]
        public void Setup()
        {
            polygonCW = new Polygon();
            polygonCW.Add(new Location(0, 1));
            polygonCW.Add(new Location(1, 0));
            polygonCW.Add(new Location(0, -1));
            polygonCW.Add(new Location(-1, 0));

            polygonCCW = new Polygon();
            polygonCCW.Add(new Location(-1, 0));
            polygonCCW.Add(new Location(0, -1));
            polygonCCW.Add(new Location(1, 0));
            polygonCCW.Add(new Location(0, 1));
        }

        [Test]
        public void IsOrientedClockwise_worksWellWhenCW()
        {
            Assert.IsTrue(polygonCW.IsOrientedClockwise());
        }
        [Test]
        public void IsOrientedClockwise_worksWellWhenCCW()
        {
            Assert.IsFalse(polygonCCW.IsOrientedClockwise());
        }
    }
}
