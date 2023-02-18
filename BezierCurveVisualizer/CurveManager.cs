using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;

namespace BezierCurveVisualizer
{
    internal class CurveManager
    {
        public struct CurvePoint
        {
            public Vector2 position;
            public int parentCurveID;
            public CurvePoint(Vector2 position, int parentCurveID)
            {
                this.position = position;
                this.parentCurveID = parentCurveID;
            }
        }

        public struct PointSelection
        {
            public int curveID;
            public int pointID;
            public PointSelection(int curveID, int pointID)
            {
                this.curveID = curveID;
                this.pointID = pointID;
            }
        }

        #region Variables
        readonly double jointHitboxRadius = 15.0;
        int curveResolution = 5;
        public DynamicArray<Curve> curves;
        public List<PointSelection> selection;
        private readonly Keys keepSelectionKey = Keys.Shift;

        bool holding;
        Vector2? lastHoldPos;

        #endregion

        public CurveManager()
        {
            curves = new DynamicArray<Curve>();
            selection = new List<PointSelection>();
        }

        #region Points
        public void AddPoint(Vector2 point)
        {
            if (selection.Count == 0)
            {
                Curve newCurve = new FreeCurve(curves.Size(), point);
                newCurve.SetResolution(curveResolution);
                curves.Add(newCurve);
                SelectPoint(curves.Size() - 1, newCurve.GetPoints().Size() - 1);
            }
            else
            {
                int curveID = selection.Last().curveID;
                int pointID = selection.Last().pointID + 1;

                curves[curveID].AddPoint(point, pointID);
                SelectPoint(curveID, pointID);
            }
        }

        public void RemovePoint(int point)
        {
            //points.RemoveAt(point);

            //ClearSelection();
        }

        #endregion

        #region User Interact
        public void RegisterClick(MouseEventArgs click)
        {
            Vector2 positionClicked = new(click.X, click.Y);

            (int curveClickedID, int pointClickedID) = CheckPointClick(positionClicked);
            
            switch (click.Button)
            {
                case MouseButtons.Left:
                    holding = true;
                    if (pointClickedID != -1)
                    {
                        SelectPoint(curveClickedID, pointClickedID);
                    }
                    else
                    {
                        AddPoint(positionClicked);
                    }
                    break;
                case MouseButtons.Right:
                    if (pointClickedID != -1)
                    {
                        RemovePoint(pointClickedID);
                    }
                    else
                    {
                        selection = new List<PointSelection>();
                    }
                    break;
            }
        }

        private (int, int) CheckPointClick(Vector2 clickedPosition)
        {
            int clickedCurve = -1;
            int clickedPoint = -1;
            double clickedPointDistance = double.MaxValue;

            for (int curveID = 0; curveID < curves.Size(); curveID++)
            {
                DynamicArray<Vector2> points = curves[curveID].GetPoints();
                for (int i = 0; i < points.Size(); i++)
                {
                    double distance = clickedPosition.DistanceTo(points[i]);
                    if (distance < jointHitboxRadius && distance < clickedPointDistance)
                    {
                        clickedPoint = i;
                        clickedCurve = curveID;
                        clickedPointDistance = distance;
                    }
                }
            }

            return (clickedCurve, clickedPoint);
        }

        #endregion

        #region Graphics
        public void Draw(Graphics graphics)
        {
            DrawSelection(graphics);

            #region Draw Connections
            for (int i = 0; i < curves.Size(); i++)
            {
                curves[i].DrawConnections(
                    graphics,
                    new SolidBrush(Color.White),
                    new Pen(Color.Black, 4)
                );
            }

            #endregion

            DrawCurves(graphics);

            #region Draw Along Curve
            //pen.Color = Color.Black;
            //pen.Width = 3;
            //double globalOffset = followOffset * t;
            //for (int i = 0; i < followCount; i++)
            //{
            //    double pointT = globalOffset + followOffset * i;
            //    DrawCircle(graphics, pen, curve.TranslatePoint(pointT, graphics, new Pen(Color.DarkGreen, 2)), 5);
            //}

            #endregion
        }

        private void DrawSelection(Graphics graphics)
        {
            if (selection.Count == 0) return;

            Pen pen = new Pen(Color.Blue, 3);
            foreach (PointSelection selectedPoint in selection)
            {
                Vector2 position = curves[selectedPoint.curveID].GetPoints()[selectedPoint.pointID];
                if (position != null)
                {
                    DrawFunctions.DrawCircle(graphics, pen, position, 13);
                }
            }
        }

        private void DrawCurves(Graphics graphics) 
        {
            Pen pen = new(Color.Red, 3);
            for (int curveID = 0; curveID < curves.Size(); curveID++)
            {
                curves[curveID].DrawCurve(graphics, pen);
            }
        }

        #endregion

        #region Selection 
        public void SelectPoint(int curveID, int pointID)
        {
            if (Control.ModifierKeys != keepSelectionKey)
            {
                selection = new List<PointSelection>();
            }

            PointSelection newSelection = new PointSelection(curveID, pointID);
            int oldSelectionID = selection.IndexOf(newSelection);
            if (oldSelectionID == -1)
            {
                selection.Add(newSelection);
            }
            else
            {
                selection.RemoveAt(oldSelectionID);
            }
        }

        #endregion

        #region Hold 
        public void UpdateHold(Vector2 newPos)
        {
            if (holding == false) return;

            if (lastHoldPos != null)
            {
                Vector2 diff = newPos - lastHoldPos;
                foreach (PointSelection pointSelection in selection)
                {
                    Curve curve = curves[pointSelection.curveID];
                    curve.GetPoints()[pointSelection.pointID] += diff;
                    curve.UpdatePath();
                }
            }

            lastHoldPos = newPos;
        }

        public void ReleaseHold()
        {
            holding = false;
            lastHoldPos = null;
        }
        #endregion
    }
}
