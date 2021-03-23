using Common;
using Common.DataStructures.Coordinates;
using Moq;
using NUnit.Framework;
using RoutePlanning;
using System.Collections.Generic;

namespace TestRoutePlanning
{
    public class SimpleRoutePlannerTestSuite
    {
        Mock<IBuilder<ISingleRoutePlanner>> PrimaryPlannerBuilder;
        Mock<IBuilder<ISingleRoutePlanner>> SecondaryPlannerBuilder;
        Mock<IBuilder<IMultipleRoutesPlanner>> RouteSplitterBuilder;

        Mock<ISingleRoutePlanner> PrimaryPlanner;
        Mock<ISingleRoutePlanner> SecondaryPlanner;
        Mock<IMultipleRoutesPlanner> RouteSplitter;

        IMultipleRoutesPlanner Sut;

        Route testRoute;
        DockStationLocation baseStationLocation;

        [SetUp]
        public void Setup()
        {
            InitMocks();

            Sut = new SimpleRoutePlanner(
                PrimaryPlannerBuilder.Object,
                RouteSplitterBuilder.Object,
                SecondaryPlannerBuilder.Object);

            InitTestRouteAndBaseStationLocation();
        }

        private void InitMocks()
        {
            PrimaryPlannerBuilder = new Mock<IBuilder<ISingleRoutePlanner>>();
            SecondaryPlannerBuilder = new Mock<IBuilder<ISingleRoutePlanner>>();
            RouteSplitterBuilder = new Mock<IBuilder<IMultipleRoutesPlanner>>();

            PrimaryPlanner = new Mock<ISingleRoutePlanner>();
            SecondaryPlanner = new Mock<ISingleRoutePlanner>();
            RouteSplitter = new Mock<IMultipleRoutesPlanner>();

            PrimaryPlannerBuilder.Setup(p => p.Build()).Returns(PrimaryPlanner.Object);
            SecondaryPlannerBuilder.Setup(p => p.Build()).Returns(SecondaryPlanner.Object);
            RouteSplitterBuilder.Setup(p => p.Build()).Returns(RouteSplitter.Object);
        }

        private void InitTestRouteAndBaseStationLocation()
        {
            testRoute = new Route();
            testRoute.Add(new Location(1, 1));
            testRoute.Add(new Location(1, 0));
            testRoute.Add(new Location(0, 0));
            testRoute.Add(new Location(0, 1));

            baseStationLocation = new DockStationLocation(2, 2, 1);
        }

        [Test]
        public void UnnecessaryTest()
        {
            var list = new List<Route>();
            list.Add(testRoute);

            PrimaryPlanner.Setup(p => p.PlanRoute(It.IsAny<PointsSetBase>(), baseStationLocation)).Returns(testRoute);
            RouteSplitter.Setup(p => p.PlanRoutes(It.IsAny<PointsSetBase>(), baseStationLocation)).Returns(list);
            PrimaryPlanner.Setup(p => p.PlanRoute(It.IsAny<PointsSetBase>(), baseStationLocation)).Returns(testRoute);

            Assert.AreEqual(Sut.PlanRoutes(testRoute, baseStationLocation), list);
        }
    }
}