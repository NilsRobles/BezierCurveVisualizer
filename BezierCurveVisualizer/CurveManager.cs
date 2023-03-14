using System.Diagnostics;

namespace BezierCurveVisualizer
{
    internal class CurveManager
    {

        #region Structs
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

        #endregion

        #region Variables
        private readonly double jointHitboxRadius = 15.0;
        private readonly float dragDeadzone = 8;
        private readonly Keys keepSelectionKey = Keys.Shift;

        private int curveResolution = 100;
        public DynamicArray<Curve> curves;

        public List<PointSelection> selection;
        bool holding;
        bool dragging;
        Vector2 lastClickPos;
        Vector2 lastHoldPos;

        #endregion

        public CurveManager()
        {
            curves = new DynamicArray<Curve>();

            selection = new List<PointSelection>();
            holding = false;
            dragging = false;
            lastClickPos = new Vector2();
            lastHoldPos = new Vector2();
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

        public void RemovePoint(int curveID, int pointID)
        {
            curves[curveID].DeletePoint(pointID);
        }

        #endregion

        #region User Interact
        public void RegisterClick(MouseEventArgs click)
        {
            Vector2 positionClicked = new(click.X, click.Y);
            lastClickPos = positionClicked;
            lastHoldPos = positionClicked;
            holding = true;

            (int curveID, int pointID) = CheckPointClick(positionClicked);

            switch (click.Button)
            {
                case MouseButtons.Left:
                    if (pointID != -1)
                    {
                        SelectPoint(curveID, pointID);
                    }
                    else
                    {
                        AddPoint(positionClicked);
                    }
                    break;
                case MouseButtons.Right:
                    if (pointID != -1)
                    {
                        RemovePoint(curveID, pointID);
                    }
                    else
                    {
                        selection.Clear();
                    }
                    break;
            }
        }

        public void RegisterRelease()
        {
            holding = false;
            if (dragging)
            {
                dragging = false;
                return;
            }
        }

        public void RegisterMouseMove(MouseEventArgs e)
        {
            Vector2 mousePos = new(e.X, e.Y);

            if (!holding) return;
            
            if (!dragging)
            {
                if (lastClickPos.DistanceTo(mousePos) < dragDeadzone) return;
                dragging = true;
            }
            UpdateHold(mousePos);

            lastHoldPos = mousePos;
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

            Pen pen = new(Color.Blue, 3);
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

            PointSelection newSelection = new(curveID, pointID);
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
            if (lastHoldPos != null)
            {
                List<Curve> updatedCurves = new();

                Vector2 diff = newPos - lastHoldPos;
                foreach (PointSelection pointSelection in selection)
                {
                    Curve curve = curves[pointSelection.curveID];
                    curve.GetPoints()[pointSelection.pointID] += diff;

                    if (!updatedCurves.Contains(curve))
                    {
                        updatedCurves.Add(curve);
                    }
                }

                foreach (Curve curve in updatedCurves)
                {
                    curve.UpdatePath();
                }
            }
        }
        #endregion
    }
}
