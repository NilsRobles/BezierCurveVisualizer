using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Timers;

namespace BezierCurveVisualizer
{
    public partial class Main : Form
    {
        System.Timers.Timer updateTimer;

        Curve curve;
        GraphicsPath path;
        GraphicsPath path2;

        int itemSelected;

        double t;
        double followSpeed = 0.5;
        int followCount = 5;
        double followOffset;

        double updateFrequency = 144;

        bool holding = false;

        double jointHitboxRadius = 15.0;

        public Main()
        {
            InitializeComponent();

            curve = new Curve();
            path = new GraphicsPath();
            itemSelected = -1;

            // This will reduce flickering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            t = 0;

            followSpeed *= followCount;
            followOffset = 1.0 / followCount;

            updateTimer = new System.Timers.Timer(1000 / updateFrequency);
            updateTimer.Elapsed += Update;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;

            path2 = new GraphicsPath();
            path2.AddLine(new Point(40, 40), new Point(80, 80));
            path2.AddLine(new Point(60, 100), new Point(80, 200));
        }

        private void OnLoad(object sender, EventArgs e)
        {

        }

        private void Update(object? sender, ElapsedEventArgs? e)
        {
            t += updateTimer.Interval / 1000 * followSpeed;
            if (t > 1)
            {
                t--;
            }

            Invalidate();
        }

        private void OnDraw(object sender, PaintEventArgs e)
        {
            Vector2[] points = curve.GetPoints();
            int pointCount = points.Length;
            if (pointCount == 0) return;
            Graphics graphics = e.Graphics;

            Pen pen = new Pen(Color.Black, 1);


            graphics.DrawPath(pen, path2);

            #region Draw Selection
            if (itemSelected != -1)
            {
                pen.Color = Color.Blue;
                pen.Width = 3;
                Vector2 item = curve.GetPoints()[itemSelected];
                if (item != null)
                {
                    DrawCircle(graphics, pen, item, 13);
                }
            }

            #endregion

            #region Draw Connections
            pen.Color = Color.Black;
            pen.Width = 4;
            for (int i = 0; i < pointCount - 1; i++)
            {
                graphics.DrawLine(pen, points[i], points[i + 1]);
            }
            #endregion

            #region Draw Control Points
            pen.Color = Color.Black;
            pen.Width = 4;
            foreach (Vector2 vector2 in curve.GetPoints())
            {
                SolidBrush brush = new SolidBrush(Color.White);
                DrawFullCircle(graphics, brush, (float)vector2.x, (float)vector2.y, 9);
                DrawCircle(graphics, pen, vector2, 8);
            }

            #endregion

            #region Draw Curves
            pen.Color = Color.Red;
            pen.Width = 4;
            graphics.DrawPath(pen, path);

            #endregion

            #region Draw Along Curve
            pen.Color = Color.Black;
            pen.Width = 3;
            for (int i = 0; i < followCount; i++)
            {
                double pointT = (followOffset * t) + (followOffset * i);
                DrawCircle(graphics, pen, curve.TranslatePoint(pointT), 5);
            }
            
            #endregion
        }

        private void UpdatePath()
        {
            path = new GraphicsPath();
            double resolution = 100.0;
            PointF[] points = new PointF[(int)resolution];
            for (int i = 0; i < resolution; i++)
            {
                double t = (i / (resolution - 1)) * 1.0;
                points[i] = curve.TranslatePoint(t);
            }
            path.AddLines(points);
        }

        private void Main_MouseClick(object sender, MouseEventArgs e)
        {
            int itemClicked = CheckItemClick(e);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    holding = true;
                    if (itemClicked != -1)
                    {
                        SelectItem(itemClicked);
                    }
                    else
                    {
                        AddPoint(new Vector2(e.X, e.Y));
                    }
                    return;
                case MouseButtons.Right:
                    if (itemClicked != -1)
                    {
                        curve.RemovePointAt(itemClicked);
                        UpdatePath();
                        itemSelected = -1;
                    }
                    else
                    {
                        itemSelected = -1;
                    }
                    return;
            }
            Invalidate();
        }

        private void AddPoint(Vector2 point)
        {
            curve.AddPointAt(new Vector2(point.x, point.y), itemSelected + 1);
            SelectItem(itemSelected + 1);
            UpdatePath();
        }

        private int CheckItemClick(MouseEventArgs e)
        {
            Vector2 pressLocation = new Vector2(e.X, e.Y);
            Vector2[] points = curve.GetPoints();
            int clickedItem = -1;
            double clickedItemDistance = Double.MaxValue;
            for (int i = 0; i < points.Length; i++)
            {
                double distance = pressLocation.DistanceTo(points[i]);
                if (distance < jointHitboxRadius && distance < clickedItemDistance)
                {
                    clickedItem = i;
                    clickedItemDistance = distance;
                }
            }

            return clickedItem;
        }

        private void SelectItem(int itemIndex)
        {
            itemSelected = itemIndex;
        }

        private void DrawCircle(Graphics g, Pen pen, Vector2 position, float radius)
        {
            g.DrawEllipse(pen,
                (float)position.x - radius,
                (float)position.y - radius,
                radius * 2,
                radius * 2);
        }

        private void DrawFullCircle(Graphics g, Brush brush, float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush,
                centerX - radius,
                centerY - radius,
                radius * 2,
                radius * 2);
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (holding && itemSelected != -1)
            {
                curve.MovePoint(itemSelected, new Vector2(e.X, e.Y));
                UpdatePath();
            }
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            holding = false;
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (itemSelected == -1) return;
                curve.RemovePointAt(itemSelected);
                itemSelected = -1;
                UpdatePath();
            }
        }
    }
}