using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace Common.DataStructures.Coordinates
{
    public class PointsSetBase
    {
        public List<Location> Points { get; set; }
        public string Message { get; set; }
        public bool Empty() { return Points.Count == 0; }
        public PointsSetBase()
        {
            Points = new List<Location>();
            Message = Messages.NOT_SET;
        }

        public PointsSetBase(List<Location> points)
        {
            Points = new List<Location>(points);
        }

        public PointsSetBase(PointsSetBase set)
        {
            Points = new List<Location>(set.Points);
        }
        public int PointsCount { get { return Points.Count; } }
        public void Insert(int index, Location item) { Points.Insert(index, item); }
        public void Add(Location item) { Points.Add(item); }
        public void AddRange(List<Location> items) { Points.AddRange(items); }
        public void Add(PointsSetBase items) { Points.AddRange(items.Points); }
        public void RemoveAt(int index) { Points.RemoveAt(index); }

        public Location this[int index]
        {
            get { return Points[index]; }
            set { Points[index] = value; }
        }

        public override string ToString() { return Message.ToString(); }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((PointsSetBase)obj);
        }

        public bool Equals(PointsSetBase other)
        {
            return Enumerable.SequenceEqual(Points, other.Points);
        }
    }
}
