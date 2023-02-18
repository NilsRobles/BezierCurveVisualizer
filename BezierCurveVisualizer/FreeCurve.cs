using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurveVisualizer
{
    internal class FreeCurve : Curve
    {
        public FreeCurve(CurveManager manager, int curveID, Vector2 startPoint) : base(manager, curveID)
        {
            AddPoint(startPoint);
        }

        public override void AddPoint(Vector2 point)
        {
            int idInCurve = pointsIndexes.Size();
            if (manager.selection.Count != 0)
            {
                int clickedPointID = manager.selection.Last();
                Vector2 clickedPointPosition = manager.points[manager.selection.Last()].position;

                for (int i = 0; i < pointsIndexes.Size(); i++)
                {
                    if (clickedPointPosition == manager.points[pointsIndexes[i]].position)
                    {
                        idInCurve = i + 1;
                    }
                }
            }

            int globalID = manager.points.Size();
            manager.points.Add(new CurveManager.CurvePoint(point, curveID));
            manager.SelectPoint(globalID);
            pointsIndexes.AddtAt(globalID, idInCurve);
        }

        public override void DeletePoint(int pointIndex)
        {
            manager.points.RemoveAt(pointIndex);

            pointsIndexes.Remove(pointIndex);
        }
    }
}
