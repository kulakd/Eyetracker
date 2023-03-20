using System;
using System.Collections.Generic;
using System.Text;

namespace GameLab.Geometry
{
    public struct Point
    {
        public int X, Y;        

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point operator+(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator-(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point operator+(Point p, Size r)
        {
            return new Point(p.X + r.Width, p.Y + r.Height);
        }

        public static Point operator-(Point p, Size r)
        {
            return new Point(p.X - r.Width, p.Y - r.Height);
        }

        public static bool operator==(Point p1, Point p2)
        {
            return (p1.X == p2.X) && (p1.Y == p2.Y);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1==p2);
        }

        public override bool Equals(object obj)
        {
            return obj is Point && this == (Point)obj;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + X.ToString() + "," + Y.ToString() + ")";
        }

        public static readonly Point Zero = new Point(0, 0);

        public static implicit operator PointF(Point p)
        {
            return new PointF(p.X, p.Y);
        }
    }

    public struct PointF
    {
        public float X, Y;

        public PointF(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
        
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y);
            }
        }

        public static float Distance(PointF p1, PointF p2)
        {
            return (p2 - p1).Length;
        }

        public Point ToPoint()
        {
            return new Point((int)Math.Round(X), (int)Math.Round(Y));
        }

        public static bool operator ==(PointF p1, PointF p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(PointF p1, PointF p2)
        {
            return !(p1 == p2);
        }

        public static explicit operator Point(PointF p)
        {
            return p.ToPoint();
        }

        public static PointF operator+(PointF p1, PointF p2)
        {
            return new PointF(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static PointF operator-(PointF p1, PointF p2)
        {
            return new PointF(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static PointF operator+(PointF p, Size r)
        {
            return new PointF(p.X + r.Width, p.Y + r.Height);
        }

        public static PointF operator-(PointF p, Size r)
        {
            return new PointF(p.X - r.Width, p.Y - r.Height);
        }

        public static PointF operator *(PointF p, float d)
        {
            return new PointF(p.X * d, p.Y * d);
        }

        public static PointF operator /(PointF p, float d)
        {
            return new PointF(p.X / d, p.Y / d);
        }

        public override bool Equals(object obj)
        {
            if (obj is PointF) return this == (PointF)obj;
            else return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public static readonly PointF Zero = new PointF(0, 0);
    }

    public struct Size
    {
        public int Width, Height;

        public Size(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public static Size operator/(Size size, float divisor)
        {
            int width = (int)Math.Round(size.Width/divisor);
            int height = (int)Math.Round(size.Height/divisor);
            return new Size(width, height);
        }

        public override string ToString()
        {
            return "[" + Width.ToString() + "," + Height.ToString() + "]";
        }
    }
}