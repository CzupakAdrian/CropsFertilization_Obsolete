using Common;
using Common.DataStructures.Coordinates;
using System.Collections.Generic;

namespace RoutePlanning.ImagingPlanning
{
    class StraightSkeletonGenerator
    {
        public StraightSkeletonGenerator(Polygon polygon)
        {
            Polygon = polygon;
            ActiveEdges = new List<Stretch>();
            ActiveVectors = new List<Stretch>();
            SkeletonEdges = new List<Stretch>();
            Subpolygons = new List<Polygon>();
            ActiveSubPolygons = new List<Polygon>();

            InitEdgesAndSubpolygons();
            InitBisectionVectors();
        }

        public void GenerateSkeleton()
        {
            int i = 0;
            while (ActiveVectors.Count != 0)
            {
                var current = Abs(i);
                var previous = Abs(current - 1);
                var intersection = ActiveVectors[current].DirectionIntersectionWith(ActiveVectors[previous]);
                if (CurrentActive == 3)
                    Finish(intersection);
                else if (IsNextIntersectionFartherThanPrevious(i) && IsPreviousIntersectionFartherThanNext(i - 1))
                    ApplyIntersectionFor(i, intersection);
                else
                    i += 1;
            }
            SortSubpolygons();
        }

        private void InitEdgesAndSubpolygons()
        {
            for (CurrentActive = 0; CurrentActive < Polygon.PointsCount; CurrentActive++)
            {
                var previousIndex = (CurrentActive + Polygon.PointsCount - 1) % Polygon.PointsCount;
                var edge = new Stretch(Polygon[previousIndex], Polygon[CurrentActive]);
                ActiveEdges.Add(edge);
                ActiveSubPolygons.Add(new Polygon());
                ActiveSubPolygons[CurrentActive].Add(edge.Start);
                ActiveSubPolygons[CurrentActive].Add(edge.End);
            }
        }

        private void InitBisectionVectors()
        {
            for (int i = 0; i < CurrentActive; i++)
                ActiveVectors.Add(new Stretch(Polygon[i], CalculateBisectionVectorAt(i)));
        }

        private void Finish(Location intersection)
        {
            ActiveVectors.ForEach(x => SkeletonEdges.Add(new Stretch(x.Start, intersection)));
            ActiveSubPolygons.ForEach(x => x.Add(intersection));

            Functional.Times(3).Do(() => CloseSubPolygon(0));
            ActiveVectors.Clear();
            ActiveEdges.Clear();

            CurrentActive = 0;

        }

        private void ApplyIntersectionFor(int current, Location intersection)
        {
            current = Abs(current);
            var previous = Abs(current - 1);
     
            ActiveVectors[current].End = intersection;
            ActiveVectors[previous].End = intersection;
            SkeletonEdges.Add(ActiveVectors[current]);
            SkeletonEdges.Add(ActiveVectors[previous]);

            ActiveEdges.RemoveAt(current);
            ApplyIntersectionToSubPolygons(current, intersection);

            CurrentActive--;
            var bisectionVector = CalculateBisectionVectorAt(previous);
            ActiveVectors[previous] = new Stretch(intersection, bisectionVector);
            ActiveVectors.RemoveAt(current);
        }

        private void ApplyIntersectionToSubPolygons(int current, Location intersection)
        {
            var previous = Abs(current - 1);
            var next = Abs(current + 1);
            ActiveSubPolygons[previous].Add(intersection);
            ActiveSubPolygons[current].Add(intersection);
            ActiveSubPolygons[next].Insert(0, intersection);
            CloseSubPolygon(current);
        }

        private void CloseSubPolygon(int current)
        {
            Subpolygons.Add(ActiveSubPolygons[current]);
            ActiveSubPolygons.RemoveAt(current);
        }

        private Vector CalculateBisectionVectorAt(int v)
        {
            var firstDir = new Vector(-ActiveEdges[Abs(v)]);
            var secondDir = new Vector(ActiveEdges[Abs(v + 1)]);

            return firstDir.GetBisectionWith(secondDir);
        }

        private bool IsNextIntersectionFartherThanPrevious(int i)
        { // to może być usprawnione przez przetrzymywanie wyników w tabeli
            i = Abs(i);
            var previousIntersection = ActiveVectors[i].DirectionIntersectionWith(ActiveVectors[Abs(i - 1)]);
            var nextIntersection = ActiveVectors[i].DirectionIntersectionWith(ActiveVectors[Abs(i + 1)]);
            return ActiveVectors[i].Start.GetDistanceTo(previousIntersection) <= ActiveVectors[i].Start.GetDistanceTo(nextIntersection);
        }

        private bool IsPreviousIntersectionFartherThanNext(int i)
        {
            i = Abs(i);
            var previousIntersection = ActiveVectors[i].DirectionIntersectionWith(ActiveVectors[Abs(i - 1)]);
            var nextIntersection = ActiveVectors[i].DirectionIntersectionWith(ActiveVectors[Abs(i + 1)]);
            return ActiveVectors[i].Start.GetDistanceTo(previousIntersection) >= ActiveVectors[i].Start.GetDistanceTo(nextIntersection);
        }

        private int Abs(int i)
        {
            return (i + CurrentActive) % CurrentActive;
        }

        private void SortSubpolygons()
        {
            var tempSubpolygons = new List<Polygon>();
            for (int current = 0, previous = Polygon.PointsCount - 1;
                 current < Polygon.PointsCount;
                 current++, previous = current - 1)
            {
                foreach(var subpol in Subpolygons)
                {
                    if (subpol.Points.FindAll(x => AreEqual(x, Polygon[current]) || AreEqual(x, Polygon[previous])).Count == 2)
                    {
                        tempSubpolygons.Add(subpol);
                    }
                }
            }
            Subpolygons = tempSubpolygons;
        }

        private static bool AreEqual(Location lhs, Location rhs)
        {
            return lhs != null && rhs != null && lhs.Latitude == rhs.Latitude && lhs.Longitude == rhs.Longitude;
        }

        int CurrentActive;
        private Polygon Polygon;
        private List<Polygon> ActiveSubPolygons;
        private List<Stretch> ActiveEdges;
        private List<Stretch> ActiveVectors;
        public List<Stretch> SkeletonEdges { get; private set; }
        public List<Polygon> Subpolygons { get; private set; }

    }
}
