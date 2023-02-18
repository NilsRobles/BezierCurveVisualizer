using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurveVisualizer
{
    internal class FreeCurve : Curve
    {
        public FreeCurve(int resolution, Vector2 startPoint) : base(resolution)
        {
            AddPoint(startPoint, 0);
        }

        public override void AddPoint(Vector2 point, int id)
        {
            points.AddtAt(point, id);
            UpdatePath();
        }

        public override void DeletePoint(int pointIndex)
        {
            points.RemoveAt(pointIndex);
            UpdatePath();
        }
    }
}
