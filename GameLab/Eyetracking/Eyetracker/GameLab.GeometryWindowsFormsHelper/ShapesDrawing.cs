using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameLab.Geometry.WindowsForms
{    
    using D = System.Drawing;
    using G = GameLab.Geometry;

    public static class ShapeDrawingHelper
    {
        //lepiej byłoby to zrobić z polimorfizmem, ale dla struct nie można

        //TODO
        /*
        public static void Draw(this Rectangle shape, Graphics g, Pen pen, bool debugInformation = false, Control container = null)
        {
            
        }
        */

        public static void Draw(this G.Rectangle shape, D.Graphics g, D.Pen pen, string debugInformation = null, Control container = null)
        {         
            int penHalfWidth = (int)pen.Width / 2;
            D.Rectangle rectangle = shape.ToSystemDrawingRectangle();
            if (container != null) rectangle = container.RectangleToClient(rectangle);
            D.Rectangle _region = new D.Rectangle(rectangle.Left - 1 - penHalfWidth, rectangle.Top - 1 - penHalfWidth, rectangle.Width + 1 + 2 * penHalfWidth, rectangle.Height + 1 + 2 * penHalfWidth);
            if (pen != null) g.DrawRectangle(pen, _region);
            if (!string.IsNullOrWhiteSpace(debugInformation)) g.DrawString(debugInformation, new D.Font(D.FontFamily.GenericSansSerif, 10), D.Brushes.Black, rectangle.Left, rectangle.Top);
        }

        public static void Draw(this G.Ellipse shape, D.Graphics g, D.Pen pen, string debugInformation = null, Control container = null)
        {
            int penHalfWidth = (int)pen.Width / 2;
            D.Rectangle rectangle = shape.AxisAlignedBoundingBox.ToSystemDrawingRectangle();
            if (container != null) rectangle = container.RectangleToClient(rectangle);
            D.Rectangle _region = new D.Rectangle(rectangle.Left - 1 - penHalfWidth, rectangle.Top - 1 - penHalfWidth, rectangle.Width + 1 + 2 * penHalfWidth, rectangle.Height + 1 + 2 * penHalfWidth);
            if (pen != null) g.DrawEllipse(pen, _region);
            if (!string.IsNullOrWhiteSpace(debugInformation)) g.DrawString(debugInformation, new D.Font(D.FontFamily.GenericSansSerif, 10), D.Brushes.Black, rectangle.Left, rectangle.Top);
        }

        private static void drawPolygon(D.Graphics g, G.Point[] points, D.Pen pen, string debugInformation = null, Control container = null)
        {
            int penHalfWidth = (int)pen.Width / 2;
            D.Point[] pointsCopy = new D.Point[points.Length + 1];
            for (int i = 0; i < points.Length + 1; ++i)
            {
                pointsCopy[i] = points[(i == points.Length) ? 0 : i].ToSystemDrawingPoint(); //na koniec wstawiam pierwszy
                if (container != null) pointsCopy[i] = container.PointToClient(pointsCopy[i]);
            }
            //if (container != null) rectangle = container.RectangleToClient(rectangle);            
            //D.Rectangle _region = new D.Rectangle(rectangle.Left - 1 - penWidth, rectangle.Top - 1 - penWidth, rectangle.Width + 1 + 2 * penWidth, rectangle.Height + 1 + 2 * penWidth);
            //if (pen != null) g.DrawEllipse(pen, _region);
            if (pen != null) g.DrawLines(pen, pointsCopy);

            D.Rectangle rectangle = Polygon.CalculateAxisAlignedBoundingBox(points).ToSystemDrawingRectangle();
            if (container != null) rectangle = container.RectangleToClient(rectangle);
            if (!string.IsNullOrWhiteSpace(debugInformation)) g.DrawString(debugInformation, new D.Font(D.FontFamily.GenericSansSerif, 10), D.Brushes.Black, rectangle.Left, rectangle.Top);
        }

        public static void Draw(this G.Polygon shape, D.Graphics g, D.Pen pen, string debugInformation = null, Control container = null)
        {
            drawPolygon(g, shape.Verticies, pen, debugInformation, container);
        }

        public static void Draw(this G.Triangle shape, D.Graphics g, D.Pen pen, string debugInformation = null, Control container = null)
        {
            drawPolygon(g, shape.Verticies, pen, debugInformation, container);
        }

        public static void Draw(this G.IShape shape, D.Graphics g, D.Pen pen, string debugInformation = null, Control container = null)
        {
            //w nowszych wersjach C# można zrobić switch po typach
            if (shape == null) return;
            if (shape is Rectangle) Draw((G.Rectangle)shape, g, pen, debugInformation, container);
            if (shape is Ellipse) Draw((G.Ellipse)shape, g, pen, debugInformation, container);
            if (shape is Triangle) Draw((G.Triangle)shape, g, pen, debugInformation, container);
            if (shape is Polygon) Draw((G.Polygon)shape, g, pen, debugInformation, container);
        }
    }
}
