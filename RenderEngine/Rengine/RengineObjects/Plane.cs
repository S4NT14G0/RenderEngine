using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using RenderEngine.Rengine.Raycast;

namespace RenderEngine.Rengine.RengineObjects
{
    class Plane : RengineObject
    {
        public override Vector3D AlbedoColor { get; set; }

        public override Vector3D Center { get; set; }

        public int Size { get; set; }


        public Plane(Vector3D center, int size)
        {
            Center = center;
            Size = size;
        }

        public override Vector3D GetColor(Ray ray, Vector3D light)
        {
            double t = Intersect(ray);

            if (!double.IsInfinity(t))
            {
                Vector3D p = ray.GetPoint3D(t);

                Vector3D n = Normal(p);
                Vector3D one = new Vector3D(1, 1, 1);
                Vector3D ld = AlbedoColor * System.Math.Max(0, Vector3D.DotProduct(n, one));
                Vector3D v = ray.E - p;
                v.Normalize();
                double phong = 128;
                Vector3D h = v + light;
                h.Normalize();
                Vector3D ls = AlbedoColor * System.Math.Pow(System.Math.Max(0, Vector3D.DotProduct(h, n)), phong);
                return ld + ls;
            }
            else
            {
                return new Vector3D(0, 0, 0);
            }
        }

        public override double Intersect(Ray ray)
        {
            // O is ray starting point
            // D is normalized ray direction vector
            // t is parameter that gives you a point (R (t)) on the ray

            // R (t) = O + tD
            Vector3D O = ray.E;
            Vector3D D = ray.S;
            D.Normalize();
            // T = distance along ray


            // plane equation is
            // ax + by + cz + d = 0

            // N is the plane normal
            // P is a point on the plane
            // d is distance of plane from the origin

            Vector3D P = Center;
            double d = Distance(P, O);
            Vector3D N = Normal(P);
            //N.Normalize();

            //N.P + d = 0

            //N . (O + tD) + d = 0

            //N.O + t(N.D) + d = 0

            //t = -(N.O + d) / (N.D)
            double t = -(Vector3D.DotProduct(N, O) + d) / (Vector3D.DotProduct(N, D));

            if (t < 0)
            {
                return double.PositiveInfinity;
            }
            else
            {
                return t;
            }
        }

        private double Distance (Vector3D p, Vector3D q)
        {

            return Math.Sqrt(Math.Pow((p.X - q.X), 2) + Math.Pow((p.Y - q.Y), 2) + Math.Pow((p.Z - q.Z), 2));
        }

        public override Vector3D Normal(Vector3D point)
        {
            Vector3D c = Center;
            Vector3D a = new Vector3D(Center.X - Size, Center.Y, Center.Z);
            Vector3D b = new Vector3D(Center.X, Center.Y, Center.Z + Size);

            return Vector3D.CrossProduct((b - a), (c - a));

        }
    }
}
