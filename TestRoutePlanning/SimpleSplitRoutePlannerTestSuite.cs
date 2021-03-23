using Common.DataStructures.Coordinates;
using NUnit.Framework;
using RoutePlanning;

namespace TestRoutePlanning
{
    public class SimpleSplitRoutePlannerTestSuite
    {
        PointsSetBase testPoints;
        Route initialRoute;
        DockStationLocation baseStationLocation;

        [SetUp]
        public void Setup()
        {
            InitTestRouteAndBaseStationLocation();
        }

        private void InitTestRouteAndBaseStationLocation()
        {
            baseStationLocation = new DockStationLocation(2, 0.5, 1);
            testPoints = new PointsSetBase();

            testPoints.Add(baseStationLocation);
            testPoints.Add(new Location(1, 0));
            testPoints.Add(new Location(0, 0));
            testPoints.Add(new Location(0, 1));
            testPoints.Add(new Location(1, 1));
            testPoints.Add(baseStationLocation);

            initialRoute = new Route(testPoints);
        }

        [Test]
        public void ReturnsOneRouteWhenItCan()
        {
            double maxDistance = initialRoute.Distance * 1.05;
            IMultipleRoutesPlanner sut = new SimpleSplitRoutePlanner(maxDistance);

            var outputRoutes = sut.PlanRoutes(testPoints, baseStationLocation);
            Assert.AreEqual(1, outputRoutes.Count);
        }

        [Test]
        public void ReturnsAllPointsWhenOneRoute()
        {
            double maxDistance = initialRoute.Distance * 1.05;
            IMultipleRoutesPlanner sut = new SimpleSplitRoutePlanner(maxDistance);

            var outputRoutes = sut.PlanRoutes(testPoints, baseStationLocation);
            Assert.AreEqual(initialRoute, outputRoutes[0]);
        }

        [Test]
        public void ReturnsBaseStationOnBothEndsWhenOneRouteAndInputHasIt()
        {
            double maxDistance = initialRoute.Distance * 1.05;
            IMultipleRoutesPlanner sut = new SimpleSplitRoutePlanner(maxDistance);

            var outputRoutes = sut.PlanRoutes(testPoints, baseStationLocation);
            Assert.AreEqual(baseStationLocation, outputRoutes[0][0]);
            Assert.AreEqual(baseStationLocation, outputRoutes[0][outputRoutes[0].PointsCount - 1]);
        }

        [Test]
        public void ReturnsBaseStationOnBothEndsWhenOneRouteAndInputHasItOnlyOnEnd()
        {
            double maxDistance = initialRoute.Distance * 1.05;
            initialRoute.Points.RemoveAt(0);
            IMultipleRoutesPlanner sut = new SimpleSplitRoutePlanner(maxDistance);

            var outputRoutes = sut.PlanRoutes(testPoints, baseStationLocation);
            Assert.AreEqual(baseStationLocation, outputRoutes[0][0]);
            Assert.AreEqual(baseStationLocation, outputRoutes[0][outputRoutes[0].PointsCount - 1]);
        }

        [Test]
        public void ReturnsBaseStationOnBothEndsWhenOneRouteAndInputHasItOnlyOnStart()
        {
            double maxDistance = initialRoute.Distance * 1.05;
            initialRoute.Points.RemoveAt(initialRoute.PointsCount - 1);
            IMultipleRoutesPlanner sut = new SimpleSplitRoutePlanner(maxDistance);

            var outputRoutes = sut.PlanRoutes(testPoints, baseStationLocation);
            Assert.AreEqual(baseStationLocation, outputRoutes[0][0]);
            Assert.AreEqual(baseStationLocation, outputRoutes[0][outputRoutes[0].PointsCount - 1]);
        }

        [Test]
        public void ReturnsMoreRoutesWhenNeeded()
        {
            double maxDistance = initialRoute.Distance;
            IMultipleRoutesPlanner sut = new SimpleSplitRoutePlanner(maxDistance);

            var outputRoutes = sut.PlanRoutes(testPoints, baseStationLocation);
            Assert.AreEqual(2, outputRoutes.Count);
        }

        [Test]
        public void ReturnsTheSameNumberOfPointsPlusBaseStationWhenMoreThanOneRoute()
        {
            double maxDistance = initialRoute.Distance;
            IMultipleRoutesPlanner sut = new SimpleSplitRoutePlanner(maxDistance);

            var outputRoutes = sut.PlanRoutes(testPoints, baseStationLocation);
            Assert.AreEqual(initialRoute.PointsCount + 2, outputRoutes[0].PointsCount + outputRoutes[1].PointsCount);
        }

        [Test]
        public void ReturnsBaseStationOnBothEndsOfEachRoute()
        {
            double maxDistance = initialRoute.Distance;
            IMultipleRoutesPlanner sut = new SimpleSplitRoutePlanner(maxDistance);

            var outputRoutes = sut.PlanRoutes(testPoints, baseStationLocation);
            Assert.AreEqual(baseStationLocation, outputRoutes[0][0]);
            Assert.AreEqual(baseStationLocation, outputRoutes[0][outputRoutes[0].PointsCount - 1]);
            Assert.AreEqual(baseStationLocation, outputRoutes[1][0]);
            Assert.AreEqual(baseStationLocation, outputRoutes[1][outputRoutes[1].PointsCount - 1]);
        }
    }
}