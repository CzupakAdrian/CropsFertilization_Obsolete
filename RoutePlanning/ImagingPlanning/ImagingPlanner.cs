using Common.DataStructures.Coordinates;
using System;
using System.Collections.Generic;

namespace RoutePlanning.ImagingPlanning
{
    public class ImagingPlanner
    {
        double Offset;

        public ImagingPlanner(double distanceBetweenImagesInMeters)
        {
            Offset = distanceBetweenImagesInMeters;
        }

        public PointsSetBase GetPoints(Polygon inputPolygon)
        {
            var points = new PointsSetBase();


            foreach (var polygon in PolygonsWithCrossingEdgesSplitter.Split(inputPolygon))
            {
                if (!polygon.IsOrientedClockwise()) polygon.Points.Reverse();
                var straightSkeletonGenerator= new StraightSkeletonGenerator(polygon);
                straightSkeletonGenerator.GenerateSkeleton();

                foreach (var narrowedPolygon in GetNarrowedPolygons(polygon, straightSkeletonGenerator.Subpolygons))
                    points.AddRange(GetPointsOnPerimeter(narrowedPolygon));

                foreach (var skeletonEdge in straightSkeletonGenerator.SkeletonEdges)
                    points.AddRange(GetPointsOnStretch(skeletonEdge));
            }
            return points;
        }

        private List<Polygon> GetNarrowedPolygons(Polygon polygon, List<Polygon> subpolygonsOfStraightSkeleton)
        {
            var offsettedSubpolygons = new List<Polygon>();

            for (double currentOffsetFactor = 0.5; ; currentOffsetFactor += 1)
            {
                var intersectingStretches = new List<Stretch>();

                // polygon.PointsCount should be equal to subpolygonsOfStraightSkeleton.Count
                for (int current = 0; current < polygon.PointsCount; current++)
                {
                    var previous = (current + polygon.PointsCount - 1) % polygon.PointsCount;
                    var outerEdge = new Stretch(polygon[previous], polygon[current]);
                    var stretch = MoveLinePerperdicularyByOffset(outerEdge, Offset * currentOffsetFactor, polygon);
                    intersectingStretches.AddRange(FindIntersectingStretches(outerEdge.End, stretch, subpolygonsOfStraightSkeleton[current]));
                }
                if (intersectingStretches.Count == 0)
                    return offsettedSubpolygons;

                offsettedSubpolygons.AddRange(ConstructPolygonsFromStretches(intersectingStretches));
            }
        }

        private List<Polygon> ConstructPolygonsFromStretches(List<Stretch> intersectingStretches)
        {
            var polygons = new List<Polygon>();
            //while (intersectingStretches.Count > 0)
            //{
            //    //DoItAsPointsOverlapped(intersectingStretches, polygons);
                
            //}
            polygons.Add(new Polygon());
            polygons[0].Add(intersectingStretches[0].Start);

            intersectingStretches.ForEach(x => polygons[0].Add(x.End));
            polygons[0].Points.RemoveAt(polygons[0].PointsCount - 1);
            return polygons;
        }

        //private static void DoItAsPointsOverlapped(List<Stretch> intersectingStretches, List<Polygon> polygons)
        //{
        //    var polygon = new Polygon();
        //    polygon.Add(intersectingStretches[0].Start);
        //    polygon.Add(intersectingStretches[0].End);
        //    intersectingStretches.RemoveAt(0);
        //    while (polygon != null)
        //    {
        //        for (int i = intersectingStretches.Count - 1; i >= 0; i--)
        //        {
        //            if (AreEqual(intersectingStretches[i].Start, polygon[polygon.PointsCount - 1]))
        //            {
        //                if (intersectingStretches[i].End == polygon[0])
        //                {
        //                    intersectingStretches.RemoveAt(i);
        //                    polygons.Add(polygon);
        //                    polygon = null;
        //                    break;
        //                }
        //                else
        //                {
        //                    polygon.Add(intersectingStretches[i].End);
        //                    intersectingStretches.RemoveAt(i);
        //                }
        //            }
        //        }
        //    }
        //}

        private List<Stretch> FindIntersectingStretches(Location firstPoint, Stretch cuttingStretch, Polygon polygon)
        {
            Func<int, int> abs = x => (x + polygon.PointsCount) % polygon.PointsCount;
            var firstPointInd = abs(polygon.Points.FindIndex(x => x == firstPoint));

            var lastPointInd = abs(firstPointInd - 1);
            var intersectingStretches = new List<Stretch>();
            Stretch intersectingStretch = null;
            for (int j = lastPointInd; j != firstPointInd; j = abs(j - 1))
            {
                var edgeBeingChecked = new Stretch(polygon[abs(j - 1)], polygon[j]);
                var intersection = cuttingStretch.DirectionIntersectionWith(edgeBeingChecked);
                if (edgeBeingChecked.Contains(intersection))
                {
                    if (intersectingStretch == null)
                        intersectingStretch = new Stretch(intersection, default);
                    else
                    {
                        intersectingStretch.End = intersection;
                        intersectingStretches.Add(intersectingStretch);
                        intersectingStretch = null;
                    }
                }
            }
            return intersectingStretches;
        }

        private Stretch MoveLinePerperdicularyByOffset(Stretch stretch, double offset, Polygon polygon)
        {
            Vector vector = new Vector(stretch.Start.Longitude - stretch.End.Longitude,
                                       stretch.End.Latitude - stretch.Start.Latitude);

            vector.Normalize();
            if (!polygon.IsOrientedClockwise()) vector.Enlarge(-1);

            return new Stretch(PolygonOffsetter.MovePointByVectorInMeters(stretch.Start, vector, offset),
                               PolygonOffsetter.MovePointByVectorInMeters(stretch.End, vector, offset));
        }

        private List<Location> GetPointsOnPerimeter(Polygon polygon)
        {
            var points = new List<Location>();
            for (int current = 0, previous = polygon.PointsCount - 1;
                current < polygon.PointsCount;
                current++, previous = current - 1)
            {
                var currentStretch = new Stretch(polygon[previous], polygon[current]);
                points.AddRange(GetPointsOnStretch(currentStretch));
            }
            return points;
        }

        private List<Location> GetPointsOnStretch(Stretch stretch)
        {
            var points = new List<Location>();
            
            for (double currentDistance = Offset / 2; currentDistance < stretch.Length; currentDistance += Offset)
            {
                Vector vector = new Vector(stretch.End.Latitude - stretch.Start.Latitude,
                                           stretch.End.Longitude - stretch.Start.Longitude);
                points.Add(PolygonOffsetter.MovePointByVectorInMeters(stretch.Start, vector, currentDistance));
            }
            return points;
        }

        private static bool AreEqual(Location lhs, Location rhs)
        {
            return lhs != null && rhs != null && lhs.Latitude == rhs.Latitude && lhs.Longitude == rhs.Longitude;
        }
    }
}
