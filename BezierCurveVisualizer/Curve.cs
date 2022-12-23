namespace BezierCurveVisualizer
{
    internal class Curve
    {
        public enum CurveAlgorithms
        {
            DeCasteljau,
            Bernstein,
            PolynomialCoefficients,
            MatrixForm
        }

        CurveAlgorithms algorithm;
        #region Manipulate algorithm
        public void SetCurveAlgorithm(CurveAlgorithms algorithm)
        {
            this.algorithm = algorithm;
        }

        #endregion

        public double length;

        Vector2[] points;
        #region Manipulate points
        public void AddPoint(Vector2 point)
        {
            Vector2[] newPoints = new Vector2[points.Length + 1];
            Array.Copy(points, newPoints, points.Length);
            newPoints[^1] = point;
            points = newPoints;
        }
        public void AddPointAt(Vector2 point, int index)
        {
            Vector2[] newPoints = new Vector2[points.Length + 1];
            Array.Copy(points, newPoints, index);
            newPoints[index] = point;
            Array.Copy(points, index, newPoints, index + 1, points.Length - index);
            points = newPoints;
        }

        public void RemovePoint(Vector2 point)
        {
            if (points.Length == 0) return;
            int index = Array.IndexOf(points, point);
            Vector2[] newPoints = new Vector2[points.Length - 1];
            Array.Copy(points, newPoints, index);
            Array.Copy(points, index + 1, newPoints, index, newPoints.Length - index);
            points = newPoints;
        }
        public void RemovePointAt(int index)
        {
            if (points.Length == 0) return;
            Vector2[] newPoints = new Vector2[points.Length - 1];
            Array.Copy(points, newPoints, index);
            Array.Copy(points, index + 1, newPoints, index, newPoints.Length - index);
            points = newPoints;
        }

        public void MovePoint(int index, Vector2 newLocation) => points[index] = newLocation;

        public int GetPointCount() => points.Length;
        public Vector2[] GetPoints() => points;

        #endregion

        public Curve()
        {
            algorithm = CurveAlgorithms.Bernstein;
            points = Array.Empty<Vector2>();
            length = 1.0;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, double t) => a + (b - a) * t;

        public Vector2 TranslatePoint(double t)
        {
            if (points.Length == 0) return new Vector2();

            t *= length;

            switch (algorithm)
            {
                case CurveAlgorithms.DeCasteljau:
                    return DeCasteljau(points, t);
                case CurveAlgorithms.Bernstein:
                    return Bernstein(points, t);
                default:
                    return new Vector2();
            }
        }

        public Vector2 TranslatePoint(double t, Graphics g, Pen pen)
        {
            if (points.Length == 0) return new Vector2();

            t *= length;

            switch (algorithm)
            {
                case CurveAlgorithms.DeCasteljau:
                    return DeCasteljau(points, t, g, pen);
                case CurveAlgorithms.Bernstein:
                    return Bernstein(points, t, g, pen);
                default:
                    return new Vector2();
            }
        }

        #region DeCasteljau
        public Vector2 DeCasteljau(Vector2[] points, double t)
        {
            if (points.Length == 1) return points[0];

            Vector2[] newPoints = new Vector2[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                newPoints[i] = Lerp(points[i], points[i + 1], t);
            }

            return DeCasteljau(newPoints, t);
        }

        public Vector2 DeCasteljau(Vector2[] points, double t, Graphics g, Pen pen)
        {
            if (points.Length == 1) return points[0];

            Vector2[] newPoints = new Vector2[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                newPoints[i] = Lerp(points[i], points[i + 1], t);
            }

            PointF[] newPointsF = new PointF[newPoints.Length];
            for (int i = 0; i < newPoints.Length; i++)
            {
                newPointsF[i] = newPoints[i];
            }
            if (newPointsF.Length > 1)
            {
                g.DrawLines(pen, newPointsF);
            }

            return DeCasteljau(newPoints, t, g, pen);
        }

        #endregion

        #region Bernstein
        public Vector2 Bernstein(Vector2[] points, double t)
        {
            int n = points.Length - 1;
            if (n == 0) return new Vector2(0, 0);

            int nFac = Factorial(n);
            Vector2 resultingPoint = new Vector2(0, 0);

            for (int i = 0; i <= n; i++)
            {
                resultingPoint += points[i] * (nFac / (Factorial(i) * Factorial(n - i)) * Math.Pow(t, i) * Math.Pow(1 - t, n - i));
            }
            return resultingPoint;
        }

        public Vector2 Bernstein(Vector2[] points, double t, Graphics g, Pen pen)
        {
            int n = points.Length - 1;
            if (n == 0) return new Vector2(0, 0);

            int nFac = Factorial(n);
            Vector2 resultingPoint = new Vector2(0, 0);

            for (int i = 0; i <= n; i++)
            {
                Vector2 vector = points[i] * (nFac / (Factorial(i) * Factorial(n - i)) * Math.Pow(t, i) * Math.Pow(1 - t, n - i));
                g.DrawLine(pen, resultingPoint, resultingPoint + vector);
                resultingPoint += vector;
            }
            return resultingPoint;
        }

        #endregion

        private static int Factorial(int value)
        {
            if (value == 0) return 1;
            int result = 1;
            for (int i = 1; i <= value; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
