using Common.DataStructures.Coordinates;
using NUnit.Framework;
using RoutePlanning;

namespace TestRoutePlanning
{
    public class TwoOptRoutePlannerTestSuite
    {
        const int IMPROVEMENTS = 5;

        ISingleRoutePlanner Sut;

        PointsSetBase testPoints;
        DockStationLocation baseStationLocation;

        [SetUp]
        public void Setup()
        {
            Sut = new TwoOptRoutePlanner(IMPROVEMENTS);

            InitTestRouteAndBaseStationLocation();
        }

        private void InitTestRouteAndBaseStationLocation()
        {
            testPoints = new PointsSetBase();
            testPoints.Add(new Location(1, 1));
            testPoints.Add(new Location(1, 0));
            testPoints.Add(new Location(0, 0));
            testPoints.Add(new Location(0, 1));

            baseStationLocation = new DockStationLocation(2, 0.5, 1);
        }

        [Test]
        public void ReturnsAllPointsPlusBaseStationOnBothEnds()
        {
            Route route = Sut.PlanRoute(testPoints, baseStationLocation);
            Assert.AreEqual(testPoints.PointsCount + 2, route.PointsCount);
            Assert.AreEqual(route[0], baseStationLocation);
            Assert.AreEqual(route[route.PointsCount - 1], baseStationLocation);
        }

        [Test]
        public void ReturnsBetterRoute()
        {
            Route initialRoute = new Route(testPoints);
            initialRoute.Add(baseStationLocation);
            initialRoute.Insert(0, baseStationLocation);
            Assert.IsTrue(Sut.PlanRoute(testPoints, baseStationLocation).Distance < initialRoute.Distance);
        }
    }
}