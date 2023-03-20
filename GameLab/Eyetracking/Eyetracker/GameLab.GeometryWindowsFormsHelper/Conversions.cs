using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLab.Geometry.WindowsForms
{
    using D = System.Drawing;
    using G = GameLab.Geometry;

    public static class Conversions
    {
        public static D.Point ToSystemDrawingPoint(this G.Point point)
        {
            return new D.Point(point.X, point.Y);
        }

        public static D.Size ToSystemDrawingSize(this G.Size size)
        {
            return new D.Size(size.Width, size.Height);
        }

        public static D.Rectangle ToSystemDrawingRectangle(this G.Rectangle rectangle)
        {
            return new D.Rectangle(rectangle.Position.ToSystemDrawingPoint(), rectangle.Size.ToSystemDrawingSize());
        }

        public static G.Point ToGameLabPoint(this D.Point point)
        {
            return new G.Point(point.X, point.Y);
        }

        public static G.Size ToGameLabSize(this D.Size size)
        {
            return new G.Size(size.Width, size.Height);
        }

        public static G.Rectangle ToGameLabRectangle(this D.Rectangle rectangle)
        {
            return new G.Rectangle(rectangle.Location.ToGameLabPoint(), rectangle.Size.ToGameLabSize());
        }

        public static G.Ellipse ToGameLabEllipse(D.Rectangle rectangle)
        {
            return new G.Ellipse(rectangle.ToGameLabRectangle());
        }

        /*
        public static implicit operator System.Drawing.Point(GameLab.Geometry.Point point)
        {
            return point.ToSystemDrawingPoint();
        }
        */
    }
}
