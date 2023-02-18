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
        CurveAlgorithms algorithm;
        protected readonly DynamicArray<int> pointsIndexes;
        private Vector2[] points;
        protected readonly CurveManager manager;
        private double length;
        protected int curveID;
        GraphicsPath curvePath;

        #endregion

        public Curve(CurveManager manager, int curveID)
        {
            manager.curveReference.Add(curveID);
            algorithm = CurveAlgorithms.Bernstein;
            pointsIndexes = new DynamicArray<int>();
            length = 1.0;
            this.manager = manager;
            this.curveID = curveID;
            points = Array.Empty<Vector2>();
            curvePath = new GraphicsPath();
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

        #endregion

        public void UpdatePoints(int resolution)
        {
            points = manager.GetpointsByIndexes(pointsIndexes.GetItems());

            if (resolution == 0) return;
            PointF[] pathPoints = new PointF[resolution];
            for (int i = 0; i < resolution; i++)
            {
                pathPoints[i] = TranslatePoint((float)i / resolution);
            }
            curvePath = new GraphicsPath();
            curvePath.AddLines(pathPoints);
        }

        public Vector2 TranslatePoint(double t)
        {
            if (pointsIndexes.Size() == 0) return new Vector2();

            t *= length;

            return algorithm switch
            {
                CurveAlgorithms.DeCasteljau => DeCasteljau(points, t),
                CurveAlgorithms.Bernstein => Bernstein(points, t),
                CurveAlgorithms.PolynomialCoefficients => new Vector2(),
                CurveAlgorithms.MatrixForm => new Vector2(),
                _ => new Vector2(),
            };
        }

        public Vector2 TranslatePoint(double t, Graphics g, Pen pen)
        {
            if (pointsIndexes.Size() == 0) return new Vector2();

            t *= length;

            return algorithm switch
            {
                CurveAlgorithms.DeCasteljau => DeCasteljau(points, t, g, pen),
                CurveAlgorithms.Bernstein => Bernstein(points, t, g, pen),
                CurveAlgorithms.PolynomialCoefficients => new Vector2(),
                CurveAlgorithms.MatrixForm => new Vector2(),
                _ => new Vector2(),
            };
        }

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

        public abstract void AddPoint(Vector2 point);

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
            for (int i = 0; i < points.Length - 1; i++)
            {
                g.DrawLine(pen, points[i], points[i + 1]);
            }

            for (int i = 0; i < points.Length; i++)
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
