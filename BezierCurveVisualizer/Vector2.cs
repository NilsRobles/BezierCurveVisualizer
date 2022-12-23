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

        public Vector2()
        {
            x = 0;
            y = 0;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.x + b.x, a.y + b.y);

        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.x - b.x, a.y - b.y);

        public static Vector2 operator *(Vector2 a, double b) => new(a.x * b, a.y * b);

        public static implicit operator PointF(Vector2 v) => new((float)v.x, (float)v.y);

        public static implicit operator (double, double) (Vector2 v) => (v.x, v.y);

        public override string ToString() => $"({x}, {y})";

        public double DistanceTo(Vector2 other) => Math.Sqrt(Math.Pow(other.x - x, 2) + Math.Pow(other.y - y, 2));
    }

}
