using System.Collections.Generic;
using System.Linq;

namespace Common.DataStructures.Coordinates
{
    public class Route : PointsSetBase //może dać route itp jako interfejsy???
        //Łatwiej będzie przekształcać (bez rzutowania) Do przemyślenia
    {
        
        public Route() : base() { }
        public Route(Route route) : base(route.Points)
        { }

        public Route(PointsSetBase route) : base(route.Points)
        { }

        public Route(List<Location> route) : base(route)
        { }

        public double Distance 
        {
            get
            {
                double distance = 0;
                for (int i = 1; i < PointsCount; i++)
                    distance += Points[i - 1].GetDistanceTo(Points[i]);
                return distance;
            }
        }

        public List<Route> ConvertToListDevidedBy(Location devider)
        {
            var output = new List<Route>();
            output.Add(new Route());
            for (int i = 0, currentIndex = 0; i < PointsCount; i++)
            {
                if (Points[i].Equals(devider))
                {
                    if (output[output.Count - 1].PointsCount != 0)
                    {
                        output.Add(new Route());
                        currentIndex++;
                    }
                }
                else
                {
                    output[currentIndex].Add(Points[i]);
                }
            }
            if (output[output.Count - 1].PointsCount == 0)
                output.RemoveAt(output.Count - 1);
            return output;
        }
    }
}
