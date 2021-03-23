using NUnit.Framework;
using Common.DataStructures.Coordinates;

namespace TestCommon.DataStructures.Coordinates
{
    public class PointToPolygonAffiliationCheckerTestSuite
    {
        Polygon inputPolygonCW, inputPolygonCCW;
        Location pointInside, pointOutside, pointOnEdge;

        [SetUp]
        public void Setup()
        {
            inputPolygonCW = new Polygon();
            inputPolygonCW.Add(new Location(0, 1));
            inputPolygonCW.Add(new Location(1, 1));
            inputPolygonCW.Add(new Location(1, 0));

            inputPolygonCCW = new Polygon();
            inputPolygonCCW.Add(new Location(1, 0));
            inputPolygonCCW.Add(new Location(1, 1));
            inputPolygonCCW.Add(new Location(0, 1));

            pointOutside = new Location(10, 0);
            pointInside = new Location(0.75, 0.75);
            pointOnEdge = new Location(0.5, 0.5);
        }

        [Test]
        public void PointOutside_WhenPolygonClockwise()
        {
            Assert.IsFalse(PointToPolygonAffiliationChecker.IsPointInPolygon(pointOutside, inputPolygonCW));
        }

        [Test]
        public void PointOutside_WhenPolygonCounterClockwise()
        {
            Assert.IsFalse(PointToPolygonAffiliationChecker.IsPointInPolygon(pointOutside, inputPolygonCCW));
        }

        [Test]
        public void PointInside_WhenPolygonClockwise()
        {
            Assert.IsTrue(PointToPolygonAffiliationChecker.IsPointInPolygon(pointInside, inputPolygonCW));
        }

        [Test]
        public void PointInside_WhenPolygonCounterClockwise()
        {
            Assert.IsTrue(PointToPolygonAffiliationChecker.IsPointInPolygon(pointInside, inputPolygonCCW));
        }

        [Test]
        public void PointOnEdge_WhenPolygonClockwise()
        {
            Assert.IsTrue(PointToPolygonAffiliationChecker.IsPointInPolygon(pointOnEdge, inputPolygonCW));
        }

        [Test]
        public void PointOnEdge_WhenPolygonCounterClockwise()
        {
            Assert.IsTrue(PointToPolygonAffiliationChecker.IsPointInPolygon(pointOnEdge, inputPolygonCCW));
        }

    }
}