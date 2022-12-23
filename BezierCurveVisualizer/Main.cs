using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Timers;

namespace BezierCurveVisualizer
{
    public partial class Main : Form
    {
        #region Variables
        System.Timers.Timer updateTimer;

        Curve curve;
        GraphicsPath path;

        int itemSelected;

        double t;
        int followCount;
        double followSpeed;
        double followOffset;

        double updateFrequency = 144;

        bool holding = false;
        readonly double jointHitboxRadius = 15.0;

        #endregion

        public Main()
        {
            InitializeComponent();

            curve = new Curve();
            path = new GraphicsPath();
            itemSelected = -1;

            // This will reduce flickering
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            t = 0;

            followCount = (int)followCountSetting.Value;
            UpdateFollowCount();

            updateTimer = new System.Timers.Timer(1000 / updateFrequency);
            updateTimer.Elapsed += Update;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            algorithmSetting.SelectedIndex = 0;
        }

        private void Update(object? sender, ElapsedEventArgs? e)
        {
            t += updateTimer.Interval / 1000 * decimal.ToDouble((decimal)followSpeed);
            if (t > 1) t--;
            if (t < 0) t++;

            curvePictureBox.Invalidate();
        }

        #region User Interaction
        private void CurveBox_MouseClick(object sender, MouseEventArgs e)
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
                    break;
                case MouseButtons.Right:
                    if (itemClicked != -1)
                    {
                        RemovePoint(itemClicked);
                    }
                    else
                    {
                        itemSelected = -1;
                    }
                    break;
            }

            ActiveControl = null;
        }

        private void CurveBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (holding && itemSelected != -1)
            {
                curve.MovePoint(itemSelected, new Vector2(e.X, e.Y));
                UpdatePath();
            }
        }

        private void CurveBox_MouseUp(object sender, MouseEventArgs e)
        {
            holding = false;
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("Key");
            if (e.KeyCode == Keys.Delete)
            {
                if (itemSelected == -1) return;
                curve.RemovePointAt(itemSelected);
                itemSelected--;
                UpdatePath();
            }
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            curvePictureBox.Width = Size.Width - settingsMenu.Width - 20;
        }

        #region Settings
        private void SpeedSetting_ValueChanged(object sender, EventArgs e)
        {
            followSpeed = decimal.ToDouble(followSpeedSetting.Value) * followCount;
        }

        private void FollowCountSetting_ValueChanged(object sender, EventArgs e)
        {
            UpdateFollowCount();
        }

        private void algorithmSetting_TextUpdate(object sender, EventArgs e)
        {
            switch (algorithmSetting.SelectedItem)
            {
                case "DeCasteljau":
                    curve.SetCurveAlgorithm(Curve.CurveAlgorithms.DeCasteljau);
                    break;
                case "Bernstein":
                    curve.SetCurveAlgorithm(Curve.CurveAlgorithms.Bernstein);
                    break;
            }
        }

        #endregion

        private int CheckItemClick(MouseEventArgs e)
        {
            Vector2 pressLocation = new(e.X, e.Y);
            Vector2[] points = curve.GetPoints();
            int clickedItem = -1;
            double clickedItemDistance = double.MaxValue;
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

        private void UpdateFollowCount()
        {
            if (followCountSetting.Value == 0) return;

            followCount = (int)followCountSetting.Value;
            followSpeed = (double)(followSpeedSetting.Value * followCountSetting.Value);
            followOffset = (double)(1 / followCountSetting.Value);
        }

        #endregion

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

        private void AddPoint(Vector2 point)
        {
            curve.AddPointAt(new Vector2(point.x, point.y), itemSelected + 1);
            SelectItem(itemSelected + 1);
            UpdatePath();
        }

        private void RemovePoint(int point)
        {
            curve.RemovePointAt(point);
            UpdatePath();
            itemSelected = -1;
        }

        private void SelectItem(int itemIndex)
        {
            itemSelected = itemIndex;
        }

        #region Draw Functions
        private static void DrawCircle(Graphics g, Pen pen, Vector2 position, float radius)
        {
            g.DrawEllipse(pen,
                (float)position.x - radius,
                (float)position.y - radius,
                radius * 2,
                radius * 2);
        }

        private static void DrawFullCircle(Graphics g, Brush brush, float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush,
                centerX - radius,
                centerY - radius,
                radius * 2,
                radius * 2);
        }

        #endregion

        private void CurvePictureBox_Paint(object sender, PaintEventArgs e)
        {
            Vector2[] points = curve.GetPoints();
            int pointCount = points.Length;
            if (pointCount == 0) return;

            Graphics graphics = e.Graphics;

            Pen pen = new(Color.Black, 1);

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
                SolidBrush brush = new(Color.White);
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
            double globalOffset = followOffset * t;
            for (int i = 0; i < followCount; i++)
            {
                double pointT =  globalOffset + followOffset * i;
                DrawCircle(graphics, pen, curve.TranslatePoint(pointT, graphics, new Pen(Color.DarkGreen, 2)), 5);
            }

            #endregion
        }
    }
}