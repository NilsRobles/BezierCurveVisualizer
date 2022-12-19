using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurveVisualizer
{
    internal class Vector2
    {
        public double x, y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, double b) => new Vector2(a.x * b, a.y * b);

        public static implicit operator PointF(Vector2 d) => new PointF((float)d.x, (float)d.y);

        public override string ToString() => $"({x}, {y})";

        public double DistanceTo(Vector2 other) => Math.Sqrt(Math.Pow(other.x - x, 2) + Math.Pow(other.y - y, 2));
    }

}
