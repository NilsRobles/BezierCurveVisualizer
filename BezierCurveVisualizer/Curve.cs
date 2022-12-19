using Aspose.Pdf.Facades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public delegate Vector2 CurveAlgorithm(Vector2[] points, double t);
        CurveAlgorithm algorithm;
        #region Manipulate algorithm
        public void SetCurveAlgorithm(CurveAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        public void SetCurveAlgorithm(CurveAlgorithms algorithm)
        {
            switch (algorithm)
            {
                case CurveAlgorithms.DeCasteljau:
                    this.algorithm = DeCasteljau;
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        #endregion

        public double length;

        Vector2[] points;
        #region Manipulate points
        public void AddPoint(Vector2 point)
        {
            Vector2[] newPoints = new Vector2[points.Length + 1];
            Array.Copy(points, newPoints, points.Length);
            newPoints[newPoints.Length - 1] = point;
            points = newPoints;
        }
        public void AddPointAt(Vector2 point, int index)
        {
            Vector2[] newPoints = new Vector2[points.Length + 1];
            Array.Copy(points, newPoints, index);
            newPoints[index] = point;
            int lengthToCopy = points.Length - index;
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
            algorithm = DeCasteljau;
            points = new Vector2[0];
            length = 1.0;
        }

        public Vector2 Lerp(Vector2 a, Vector2 b, double t) => a + (b - a) * t;

        public Vector2 TranslatePoint(double t)
        {
            if (points.Length == 0) throw new Exception("Cannot translate point on a curve with 0 points");

            return algorithm(points, t * length);
        }

        public Vector2 DeCasteljau(Vector2[] points, double t)
        {
            if (points.Length == 1)
            {
                return points[0];
            }

            Vector2[] newPoints = new Vector2[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                newPoints[i] = Lerp(points[i], points[i + 1], t);
            }

            return DeCasteljau(newPoints, t);
        }
    }
}
