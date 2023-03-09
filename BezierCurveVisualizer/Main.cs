using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Timers;

namespace BezierCurveVisualizer
{
    public partial class Main : Form
    {
        #region Variables
        System.Timers.Timer updateTimer;

        CurveManager curveManager;
        GraphicsPath path;

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

            curveManager = new CurveManager();
            path = new GraphicsPath();

            // This will reduce flickering
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            t = 0;

            followCount = (int)followCountSetting.Value;
            UpdateFollowCount();

            updateTimer = new System.Timers.Timer(1000 / updateFrequency);
            updateTimer.Elapsed += Update;
            updateTimer.AutoReset = true;
            updateTimer.Enabled = true;

            ResizePictureBox();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            algorithmSetting.SelectedIndex = 0;
        }

        private void Update(object? sender, ElapsedEventArgs? e)
        {
            t += updateTimer.Interval / 1000 * followSpeed;
            if (t > 1) t--;
            if (t < 0) t++;

            curvePictureBox.Invalidate();
        }
        
        #region User Interaction
        private void CurveBox_MouseClick(object sender, MouseEventArgs e)
        {
            curveManager.RegisterClick(e);
            ActiveControl = null;
        }

        private void CurveBox_MouseMove(object sender, MouseEventArgs e)
        {
            curveManager.RegisterMouseMove(e);
        }

        private void CurveBox_MouseUp(object sender, MouseEventArgs e)
        {
            curveManager.RegisterRelease(e);
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Delete)
            //{
            //    if (!pointSelection.HasSelection()) return;
            //    foreach (int selectedPoint in pointSelection)
            //    {
            //        curve.RemovePointAt(selectedPoint);
            //    }
            //    UpdatePath();
            //}
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            ResizePictureBox();
        }

        private void ResizePictureBox()
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
            //switch (algorithmSetting.SelectedItem)
            //{
            //    case "DeCasteljau":
            //        curve.SetCurveAlgorithm(Curve.CurveAlgorithms.DeCasteljau);
            //        break;
            //    case "Bernstein":
            //        curve.SetCurveAlgorithm(Curve.CurveAlgorithms.Bernstein);
            //        break;
            //}
        }

        #endregion

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
            /*path = new GraphicsPath();
            double resolution = 100.0;
            PointF[] points = new PointF[(int)resolution];
            for (int i = 0; i < resolution; i++)
            {
                double t = (i / (resolution - 1)) * 1.0;
                points[i] = curve.TranslatePoint(t);
            }
            path.AddLines(points);*/
        }

        //private void AddPoint(Vector2 point)
        //{
        //    curveManager.points.Add(point);
        //    curve.AddPointAt(point, pointSelection.CentralPoint() + 1);
        //    pointSelection.ClearSelection();
        //    UpdatePath();
        //}

        //private void RemovePoint(int point)
        //{
        //    curve.RemovePointAt(point);
        //    UpdatePath();
        //    pointSelection.ClearSelection();
        //}

        private void CurvePictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            curveManager.Draw(g);
        }
    }
}