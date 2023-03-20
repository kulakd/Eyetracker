using System;
using System.Collections.Generic;
using System.Text;

namespace GameLab.Geometry
{
    public interface IShape
    {
        bool Contains(Point point);
        PointF Center { get; }
        Rectangle AxisAlignedBoundingBox { get; } //AABB
        //Polygon ConvecHull;
    }

    public struct Rectangle : IShape
    {
        public Point Position;
        public Size Size;

        public Rectangle(Point position, Size size)
        {
            this.Position = position;
            this.Size = size;
        }

        public Rectangle(int x, int y, int width, int height)
            : this(new Point(x, y), new Size(width, height))
        { }

        public int Left
        {
            get
            {
                return Position.X;
            }
        }

        public int Right
        {
            get
            {
                return Position.X + Size.Width - 1; //po Position zajmuje już pierwszy piksel
            }
        }

        public int Width
        {
            get
            {
                return Size.Width;
            }
        }

        public int Top
        {
            get
            {
                return Position.Y;
            }
        }

        public int Bottom
        {
            get
            {
                return Position.Y + Size.Height - 1;
            }
        }

        public int Height
        {
            get
            {
                return Size.Height;
            }
        }

        public PointF Center
        { 
            get
            {
                //return Position + Size / 2; //rounds to Point
                return new PointF(Position.X + (Size.Width - 1) / 2f, Position.Y + (Size.Height - 1) / 2f);
            }
        }

        public bool Contains(Point point)
        {
            return point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom;
            //TODO: Znaki większości dopasowane do tego, jak jest w Polygon. Nie powinno tak być.
        }

        public Rectangle AxisAlignedBoundingBox
        {
            get
            {
                return new Rectangle(Position, Size); //klon oryginalnego kształtu
            }
        }

        public Point[] Verticies
        {
            get
            {
                return new Point[4]
                {
                    new Point(Left,Top),
                    new Point(Right,Top),
                    new Point(Right,Bottom),
                    new Point(Left,Bottom)
                };
            }
        }

        public override string ToString()
        {
            return "Left = " + Left + ", Top = " + Top + ", Width = " + Width + ", Height = " + Height;
        }
    }

    public struct Ellipse : IShape
    {
        private Rectangle rectangle;

        public Ellipse(Rectangle area)
        {
            this.rectangle = area;
        }

        public Ellipse(Point position, Size size)
            :this(new Rectangle(position, size))
        { }

        public Ellipse(int x, int y, int width, int height)
            : this(new Point(x, y), new Size(width, height))
        { }

        //to wywali wyjątki, jeżeli użyty będzie konstruktor domyślny
        public Point Position { get { return rectangle.Position; } }
        public Size Size { get { return rectangle.Size; } }
        public int Left { get { return rectangle.Left; } }
        public int Right { get { return rectangle.Right; } }
        public int Width { get { return rectangle.Width; } }            
        public int Top { get { return rectangle.Top; } }
        public int Bottom { get { return rectangle.Bottom; } }
        public int Height { get { return rectangle.Height; } }
        public PointF Center { get { return rectangle.Center; } }  

        public bool Contains(Point point)
        {
            double semiAxisX = rectangle.Size.Width / 2f;
            double semiAxisY = rectangle.Size.Height / 2f;
            double dx = Center.X - point.X;
            double dy = Center.Y - point.Y;
            return (dx * dx / (semiAxisX * semiAxisX) + dy * dy / (semiAxisY * semiAxisY)) <= 1;
        }

        public Rectangle AxisAlignedBoundingBox
        {
            get
            {
                return new Rectangle(Position, Size); //klon oryginalnego kształtu
            }
        }
    }

    public struct Polygon : IShape
    {
        public Point[] Verticies;

        public Polygon(params Point[] verticies)
        {
            Verticies = new Point[verticies.Length];
            //if(verticies.Length>2) throw new GameLabException("Triangle should have three verticies specified");
            for (int i = 0; i < verticies.Length; ++i) this.Verticies[i] = verticies[i];
        }

        public PointF Center
        {
            get
            {
                PointF center = PointF.Zero;
                foreach (Point vertex in Verticies) center += (PointF)vertex;
                center /= Verticies.Length;
                return center;
            }
        }

        public bool Contains(Point point)
        {
            //http://dominoc925.blogspot.com/2012/02/c-code-snippet-to-determine-if-point-is.html
            //chyba metoda ray casting zob. https://en.wikipedia.org/wiki/Point_in_polygon
            bool contains = false;
            for (int i = 0, j = Verticies.Length - 1; i < Verticies.Length; j = i++)
            {
                if (((Verticies[i].Y > point.Y) != (Verticies[j].Y > point.Y)) && (point.X < (Verticies[j].X - Verticies[i].X) * (point.Y - Verticies[i].Y) / (Verticies[j].Y - Verticies[i].Y) + Verticies[i].X))
                {
                    contains = !contains;
                }
            }
            return contains;
        }

        public static Rectangle CalculateAxisAlignedBoundingBox(Point[] verticies)
        {
            Point min = verticies[0], max = verticies[0];
            for(int i = 1; i < verticies.Length; ++i)
            {
                if (verticies[i].X < min.X) min.X = verticies[i].X;
                if (verticies[i].Y < min.Y) min.Y = verticies[i].Y;
                if (verticies[i].X > max.X) max.X = verticies[i].X;
                if (verticies[i].Y > max.Y) max.Y = verticies[i].Y;
            }
            Size size = new Size(max.X - min.X, max.Y - min.Y);
            return new Rectangle(min, size);
        }

        //werteksy mogą być zmieniane, więc liczone za każdym razem
        public Rectangle AxisAlignedBoundingBox
        {
            get
            {
                return CalculateAxisAlignedBoundingBox(Verticies);
            }
        }
    }

    //zasadniczo to nie jest potrzebne skoro jest Polygon
    //nie może dziedziczyć z Polygon bo struktury
    public struct Triangle : IShape
    {
        public Point[] Verticies;

        public Triangle(params Point[] verticies)
        {
            Verticies = new Point[3];
            if (verticies.Length != 3) throw new GameLabException("Triangle should have three verticies specified");
            //może warto sprawdzić niewspółliniowość, żeby wykluczyć trójkąty bez pola
            for (int i = 0; i < 3; ++i) this.Verticies[i] = verticies[i];
        }

        public PointF Center
        {
            get
            {
                PointF center = PointF.Zero;
                foreach (Point vertex in Verticies) center += (PointF)vertex;
                return center;
            }
        }

        public double[] GetBaricentricCoordinates(Point point)
        {
            //współrzędne barycentyczne
            //http://fizyka.umk.pl/~jacek/dydaktyka/modsym/notatki/przeciecie_odcinka_z_trojkatem.pdf
            Point t1 = Verticies[0];
            Point t2 = Verticies[1];
            Point t3 = Verticies[2];
            Point w = point - t1;
            Point u = t2 - t1;
            Point v = t3 - t1;
            double uu = u.X * u.X + u.Y * u.Y;
            double vv = v.X * v.X + v.Y * v.Y;
            double uv = u.X * v.X + u.Y * v.Y;
            double wu = w.X * u.X + w.Y * u.Y;
            double wv = w.X * v.X + w.Y * v.Y;
            double denominator = vv * uu + uv * uv;
            double mu3 = (wv * uu + wu * uv) / denominator;
            double mu2 = (wu * vv + wv * uv) / denominator;
            double mu1 = 1 - mu2 - mu3;
            return new double[] { mu1, mu2, mu3 };
        }

        public bool Contains(Point point)
        {
            double[] baricentricCoordinates = GetBaricentricCoordinates(point);
            bool contains = true;
            foreach (double baricentricCoordinate in baricentricCoordinates)
                contains = contains && (baricentricCoordinate >= 0.0 && baricentricCoordinate <= 1.0);
            return contains;
        }

        public Rectangle AxisAlignedBoundingBox
        {
            get
            {
                return Polygon.CalculateAxisAlignedBoundingBox(Verticies);
            }
        }
    }
}
