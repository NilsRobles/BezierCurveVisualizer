using System.Diagnostics;
using System.Drawing;

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

        #region Variables
        readonly double jointHitboxRadius = 15.0;
        int curveResolution = 100;
        public DynamicArray<CurvePoint> points;
        public DynamicArray<int> curveReference;
        public DynamicArray<Curve> curves;
        public List<int> selection;
        private readonly Keys keepSelectionKey = Keys.Shift;

        bool holding;
        Vector2? lastHoldPos;

        #endregion

        public CurveManager()
        {
            points = new DynamicArray<CurvePoint>();
            curveReference = new DynamicArray<int>();
            curves = new DynamicArray<Curve>();
            selection = new List<int>();
        }

        #region Points
        public void AddPoint(Vector2 point)
        {
            if (selection.Count == 0)
            {
                curves.Add(new FreeCurve(this, curves.Size(), point));
                UpdateCurves();
            }
            else
            {
                int curveID = curveReference[selection.Last()];
                curves[curveID]
                    .AddPoint(point);

                curveReference.Add(curveID);
                

                UpdateCurves();
            }
        }

        private void UpdateCurves()
        {
            for (int i = 0; i < curves.Size(); i++)
            {
                curves[i].UpdatePoints(curveResolution);
            }
        }

        public void RemovePoint(int point)
        {
            points.RemoveAt(point);

            ClearSelection();
        }

        public Vector2[] GetpointsByIndexes(int[] indexes)
        {
            Vector2[] foundPoints = new Vector2[indexes.Length];
            for (int i = 0; i < indexes.Length; i++)
            {
                foundPoints[i] = points.Get(indexes[i]).position;
            }

            return foundPoints;
        }

        #endregion

        #region User Interact
        public void RegisterClick(MouseEventArgs click)
        {
            Vector2 positionClicked = new(click.X, click.Y);
            
            int itemClicked = CheckPointClick(positionClicked);
            switch (click.Button)
            {
                case MouseButtons.Left:
                    holding = true;
                    if (itemClicked != -1)
                    {
                        SelectPoint(itemClicked);
                    }
                    else
                    {
                        AddPoint(positionClicked);
                    }
                    break;
                case MouseButtons.Right:
                    if (itemClicked != -1)
                    {
                        RemovePoint(itemClicked);
                    }
                    else
                    {
                        ClearSelection();
                    }
                    break;
            }
        }

        private int CheckPointClick(Vector2 point)
        {
            int clickedItem = -1;
            double clickedItemDistance = double.MaxValue;
            for (int i = 0; i < points.Size(); i++)
            {
                double distance = point.DistanceTo(points.Get(i).position);
                if (distance < jointHitboxRadius && distance < clickedItemDistance)
                {
                    clickedItem = i;
                    clickedItemDistance = distance;
                }
            }

            return clickedItem;
        }

        #endregion

        public void Draw(Graphics graphics)
        {
            int pointCount = points.Size();
            if (pointCount == 0) return;
            
            Pen pen = new(Color.Black, 1);

            #region Draw Selection
            if (HasSelection())
            {
                pen.Color = Color.Blue;
                pen.Width = 3;
                foreach (int selectedPoint in selection)
                {
                    Vector2 item = points.Get(selectedPoint).position;
                    if (item != null)
                    {
                        DrawFunctions.DrawCircle(graphics, pen, item, 13);
                    }
                }
            }

            #endregion

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

            #region Draw Curves
            pen.Width = 3;
            pen.Color = Color.Red;
            for (int i = 0; i < curves.Size(); i++)
            {
                curves[i].DrawCurve(graphics, pen);
            }

            #endregion

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

        #region Selection 
        public void SelectPoint(int index)
        {
            if (Control.ModifierKeys != keepSelectionKey)
            {
                ClearSelection();
            }

            if (selection.Contains(index))
            {
                selection.Remove(index);
            }
            else
            {
                Debug.WriteLine("New point");
                selection.Add(index);
            }
        }

        public void ClearSelection()
        {
            selection.Clear();
        }

        public bool HasSelection()
        {
            return selection.Count != 0;
        }

        #endregion

        #region Hold 
        public void UpdateHold(Vector2 newPos)
        {
            if (holding == false) return;

            if (lastHoldPos != null)
            {
                Vector2 diff = newPos - lastHoldPos;
                foreach (int i in selection)
                {
                    points[i] = new CurvePoint(points[i].position + diff, points[i].parentCurveID);
                }

            }

            lastHoldPos = newPos;

            UpdateCurves();
        }

        public void ReleaseHold()
        {
            holding = false;
            lastHoldPos = null;
        }
        #endregion
    }
}
