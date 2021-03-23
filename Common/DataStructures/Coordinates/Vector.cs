using System;

namespace Common.DataStructures.Coordinates
{
    public class Vector
    {
        private double _X;
        private double _Y;
        private double _Angle;
        private double _Module;

        public double X { get => _X; set { _X = value; CalculateAngleAndModule(); } }
        public double Y { get => _Y; set { _Y = value; CalculateAngleAndModule(); } }
        public double Angle { get => _Angle; set { _Angle = value % (Math.PI * 2); CalculateEndPoint(); } }
        public double Module { get => _Module; set { _Module = value; CalculateEndPoint(); } }

        public Vector()
        {
        }
        public Vector(Vector a)
        {
            _X = a.X;
            _Y = a.Y;
            _Angle = a.Angle;
            _Module = a.Module;
        }
        public Vector(Location point)
        {
            _X = point.Latitude;
            Y = point.Longitude;
        }

        public Vector(Location start, Location end)
        {
            _X = end.Latitude - start.Latitude;
            Y = end.Longitude - start.Longitude;
        }

        public Vector(Stretch stretch)
        {
            _X = stretch.End.Latitude - stretch.Start.Latitude;
            Y = stretch.End.Longitude - stretch.Start.Longitude;
        }

        public static implicit operator Vector(Location point)
        {
            return new Vector(point);
        }

        public Vector(double lat, double lng)
        {
            _X = lat;
            Y = lng;
        }

        public Vector GetBisectionWith(Vector vector)
        {
            var angleBreak = Angle - vector.Angle;
            return CreateWithAngleAndModule(Angle - angleBreak / 2);
        }

        public static Vector CreateWithAngleAndModule(double angle, double module = 1)
        {
            var v = new Vector();
            v.Angle = angle;
            v.Module = module;

            return v;
        }

        public void Enlarge(double b)
        {
            _X *= b;
            _Y *= b;
            _Module *= b;
        }

        public void Normalize()
        {
            if (_Module != 0)
                Module = 1;
        }

        private void CalculateEndPoint()
        {
            _X = Module * Math.Cos(Angle);
            _Y = Module * Math.Sin(Angle);
        }

        private void CalculateAngleAndModule()
        {
            if (X == 0)
            {
                if (Y > 0) _Angle = Math.PI / 2;
                else _Angle = -Math.PI / 2;
                _Module = Y;
            }
            else
            {
                if (X > 0) _Angle = Math.Atan(Y / X);
                else _Angle = Math.PI + Math.Atan(Y / X);

                _Module = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            }
        }
    }
}
