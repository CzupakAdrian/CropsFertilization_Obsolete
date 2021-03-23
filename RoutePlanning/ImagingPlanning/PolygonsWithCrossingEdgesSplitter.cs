using Common.DataStructures.Coordinates;
using System;
using System.Collections.Generic;

namespace RoutePlanning.ImagingPlanning
{
    public class PolygonsWithCrossingEdgesSplitter
    {
        private Polygon InputPolygon;

        public static List<Polygon> Split(Polygon polygon)
        {
            var instance = new PolygonsWithCrossingEdgesSplitter();
            return instance.SplitIfSomeCrossingsPresent(polygon);
        }

        private List<Polygon> SplitIfSomeCrossingsPresent(Polygon inputPolygon)
        {
            InputPolygon = inputPolygon;
            var polygons = new List<Polygon>();
            if (inputPolygon.PointsCount <= 3)
                polygons.Add(inputPolygon);
            else
            {
                polygons = GetListOfPolygonsAfterSplitting(GetPointsListWithIntersections());
                InnerPolygonsCutter.Cut(polygons);
            }

            return polygons;
        }

        public class InnerPolygonsCutter
        {
            private List<Polygon> Polygons;

            public static void Cut(List<Polygon> polygons)
            {
                var instance = new InnerPolygonsCutter();
                instance.CutInnerPolygons(polygons);
            }

            private void CutInnerPolygons(List<Polygon> polygons)
            {
                Polygons = polygons;
                while (!TryToCutNext());
            }

            private bool TryToCutNext()
            {
                for (int i = Polygons.Count - 2; i >= 0; i--)
                {
                    var firstPolygon = Polygons[i];
                    for (int j = Polygons.Count - 1; j > i; j--)
                    {
                        var secondPolygon = Polygons[j];
                        var crossingPoint = firstPolygon.Points.Find(x => default != secondPolygon.Points.Find(y => AreEqual(x, y)));

                        if (default != crossingPoint && IsOneInsideAnother(firstPolygon, secondPolygon, crossingPoint))
                        {
                            
                            if (firstPolygon.PolygonArea() > secondPolygon.PolygonArea())
                            {
                                CutSecondPolygonFromFirst(firstPolygon, secondPolygon, crossingPoint);
                                Polygons.RemoveAt(j);
                                return false;
                            }
                            else
                            {
                                CutSecondPolygonFromFirst(secondPolygon, firstPolygon, crossingPoint);
                                Polygons.RemoveAt(i);
                                return false;
                            }
                        }
                    }
                }
                return true;
            }

            private static bool IsOneInsideAnother(Polygon firstPolygon, Polygon secondPolygon, Location crossingPoint)
            {
                var p1 = firstPolygon.Points.Find(x => !AreEqual(x, crossingPoint) && PointToPolygonAffiliationChecker.IsPointInPolygon(x, secondPolygon));
                var p2 = secondPolygon.Points.Find(x => !AreEqual(x, crossingPoint) && PointToPolygonAffiliationChecker.IsPointInPolygon(x, firstPolygon));
                return default != p1
                    || default != p2;
            }

            private void CutSecondPolygonFromFirst(Polygon outerPolygon, Polygon innerPolygon, Location crossingPoint)
            {
                var crossingId = innerPolygon.Points.FindIndex(x => AreEqual(x, crossingPoint));
                var pointsAfterCrossing = innerPolygon.Points.GetRange(crossingId, innerPolygon.PointsCount - 1 - crossingId);
                innerPolygon.Points.InsertRange(0, pointsAfterCrossing);
                innerPolygon.Points.Reverse();
                crossingId = outerPolygon.Points.FindIndex(x => AreEqual(x, crossingPoint));
                outerPolygon.Points.InsertRange(crossingId, innerPolygon.Points);
            }

            private static bool AreEqual(Location lhs, Location rhs)
            {
                return lhs != null && rhs != null && lhs.Latitude == rhs.Latitude && lhs.Longitude == rhs.Longitude;
            }
        }

        private bool AreEqual(Location lhs, Location rhs)
        {
            return lhs != null && rhs != null && lhs.Latitude == rhs.Latitude && lhs.Longitude == rhs.Longitude;
        }

        private List<Polygon> GetListOfPolygonsAfterSplitting(List<Location> pointsWithIntersections)
        {
            Func<int> getFirstIntersectionIndexIfLeft = ()
                => pointsWithIntersections.FindIndex(x => default == InputPolygon.Points.Find(y => AreEqual(x, y)));
            var polygonsList = new List<Polygon>();

            while (true)
            {
                var nextIntersectionIndex = getFirstIntersectionIndexIfLeft();
                if (nextIntersectionIndex == -1)
                {
                    if (polygonsList.Count == 0)
                        polygonsList.Add(new Polygon());
                    polygonsList[0].AddRange(pointsWithIntersections);
                    break;
                }
                else
                {
                    polygonsList.Add(new Polygon());
                    polygonsList[polygonsList.Count - 1].AddRange(pointsWithIntersections.GetRange(0, nextIntersectionIndex + 1));
                    pointsWithIntersections.RemoveRange(0, nextIntersectionIndex + 1);
                }
            }
            return polygonsList;
        }

        private List<Location> GetPointsListWithIntersections()
        {
            var points = new List<Location>();
            Func<int, int> abs = x => (x + InputPolygon.PointsCount) % InputPolygon.PointsCount;
            for (int i = 0; i < InputPolygon.PointsCount; i++)
            {
                points.Add(InputPolygon[i]);
                var lastCrossingsList = new List<Location>();
                var stretch1 = new Stretch(InputPolygon[i], InputPolygon[abs(i + 1)]);

                for (int j = i + 2; abs(j) != abs(i - 1); j++)
                {
                    var stretch2 = new Stretch(InputPolygon[abs(j)], InputPolygon[abs(j + 1)]);

                    if (stretch1.Crossing(stretch2))
                    {
                        lastCrossingsList.Add(stretch1.DirectionIntersectionWith(stretch2));
                    }
                }

                lastCrossingsList.Sort((point1, point2) =>
                    InputPolygon[i].GetDistanceTo(point1).CompareTo(InputPolygon[i].GetDistanceTo(point2)));

                lastCrossingsList.ForEach(point => points.Add(point));

            }
            return points;
        }
    }
}
