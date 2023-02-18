using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezierCurveVisualizer
{
    internal class DrawFunctions
    {
        public static void DrawCircle(Graphics g, Pen pen, Vector2 position, float radius)
        {
            g.DrawEllipse(pen,
                (float)position.x - radius,
                (float)position.y - radius,
                radius * 2,
                radius * 2);
        }

        public static void DrawFullCircle(Graphics g, Brush brush, float centerX, float centerY, float radius)
        {
            g.FillEllipse(brush,
                centerX - radius,
                centerY - radius,
                radius * 2,
                radius * 2);
        }
    }
}
