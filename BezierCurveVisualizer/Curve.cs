using System.Drawing.Drawing2D;


namespace BezierCurveVisualizer
{
    internal abstract class Curve
    {
        public enum CurveAlgorithms
        {
            DeCasteljau,
            Bernstein,
            PolynomialCoefficients,
            MatrixForm
        }

        #region Variables
        // Curve shape
        protected DynamicArray<Vector2> points;
        private GraphicsPath curvePath;
        
        // Settings
        private CurveAlgorithms algorithm;
        private double length;
        private int resolution;

        #endregion

        public Curve(int resolution)
        {
            points = new DynamicArray<Vector2>();
            curvePath = new GraphicsPath();

            algorithm = CurveAlgorithms.DeCasteljau;
            length = 1.0;
            this.resolution = resolution;
        }

        #region Change Settings
        public void SetCurveAlgorithm(CurveAlgorithms algorithm)
        {
            this.algorithm = algorithm;
        }

        public void SetLength(double length)
        {
            this.length = length;
        }

        public void SetResolution(int resolution)
        {
            this.resolution = resolution;
            UpdatePath();
        }

        #endregion

        public void UpdatePath()
        {
            curvePath = new GraphicsPath();

            if (resolution == 0 || points.Size() == 0) return;

            PointF[] pathPoints = new PointF[resolution];
            for (int i = 0; i < resolution; i++)
            {
                pathPoints[i] = TranslatePoint((float)i / (resolution - 1));
            }
            curvePath.AddLines(pathPoints);
        }

        #region Translate Point
        public Vector2 TranslatePoint(double t)
        {
            if (points.Size() == 0) return new Vector2();

            t *= length;

            return algorithm switch
            {
                CurveAlgorithms.DeCasteljau => DeCasteljau(points.GetArray(), t),
                CurveAlgorithms.Bernstein => Bernstein(points.GetArray(), t),
                CurveAlgorithms.PolynomialCoefficients => new Vector2(),
                CurveAlgorithms.MatrixForm => new Vector2(),
                _ => new Vector2(),
            };
        }

        public Vector2 TranslatePoint(double t, Graphics g, Pen pen)
        {
            if (points.Size() == 0) return new Vector2();

            t *= length;

            return algorithm switch
            {
                CurveAlgorithms.DeCasteljau => DeCasteljau(points.GetArray(), t, g, pen),
                CurveAlgorithms.Bernstein => Bernstein(points.GetArray(), t, g, pen),
                CurveAlgorithms.PolynomialCoefficients => new Vector2(),
                CurveAlgorithms.MatrixForm => new Vector2(),
                _ => new Vector2(),
            };
        }

        #endregion

        #region DeCasteljau
        private Vector2 DeCasteljau(Vector2[] points, double t)
        {
            if (points.Length == 1) return points[0];

            Vector2[] newPoints = new Vector2[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                newPoints[i] = Lerp(points[i], points[i + 1], t);
            }

            return DeCasteljau(newPoints, t);
        }

        private Vector2 DeCasteljau(Vector2[] points, double t, Graphics g, Pen pen)
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
        private static Vector2 Bernstein(Vector2[] points, double t)
        {
            int n = points.Length - 1;
            if (n == 0) return new Vector2(0, 0);

            int nFac = Factorial(n);
            Vector2 resultingPoint = new(0, 0);

            for (int i = 0; i <= n; i++)
            {
                resultingPoint += points[i] * (nFac / (Factorial(i) * Factorial(n - i)) * Math.Pow(t, i) * Math.Pow(1 - t, n - i));
            }
            return resultingPoint;
        }

        private static Vector2 Bernstein(Vector2[] points, double t, Graphics g, Pen pen)
        {
            int n = points.Length - 1;
            if (n == 0) return new Vector2(0, 0);

            int nFac = Factorial(n);
            Vector2 resultingPoint = new(0, 0);

            for (int i = 0; i <= n; i++)
            {
                Vector2 vector = points[i] * (nFac / (Factorial(i) * Factorial(n - i)) * Math.Pow(t, i) * Math.Pow(1 - t, n - i));
                g.DrawLine(pen, resultingPoint, resultingPoint + vector);
                resultingPoint += vector;
            }
            return resultingPoint;
        }

        #endregion

        public DynamicArray<Vector2> GetPoints() => points;

        public abstract void AddPoint(Vector2 point, int id);

        public abstract void DeletePoint(int pointIndex);

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

        private static Vector2 Lerp(Vector2 a, Vector2 b, double t) => a + (b - a) * t;

        public void DrawConnections(Graphics g, Brush brush, Pen pen)
        {
            // Draw lines between points
            for (int i = 0; i < points.Size() - 1; i++)
            {
                g.DrawLine(pen, points[i], points[i + 1]);
            }

            // Draw points
            for (int i = 0; i < points.Size(); i++)
            {
                Vector2 vector = points[i];
                DrawFunctions.DrawFullCircle(g, brush, (float)vector.x, (float)vector.y, 9);
                DrawFunctions.DrawCircle(g, pen, vector, 8);
            }
        }

        public void DrawCurve(Graphics g, Pen pen)
        {
            g.DrawPath(pen, curvePath);
        }
    }
}
