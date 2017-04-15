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
            Vector3D n = Normal(Center);
            double numerator = Vector3D.DotProduct((Center - ray.E), n);
            double denominator = Vector3D.DotProduct((ray.S - ray.E), n);
            double t =  numerator / denominator;

            if (t > 0)
                return t;
            else
                return double.PositiveInfinity;
        }

        private double Distance (Vector3D p, Vector3D q)
        {

            return Math.Sqrt(Math.Pow((p.X - q.X), 2) + Math.Pow((p.Y - q.Y), 2) + Math.Pow((p.Z - q.Z), 2));
        }

        public override Vector3D Normal(Vector3D point)
        {
            Vector3D c = new Vector3D(Center.X, Center.Y, Center.Z);
            Vector3D a = new Vector3D(Center.X - Size, Center.Y, Center.Z);
            Vector3D b = new Vector3D(Center.X, Center.Y, Center.Z + Size);

            //Vector3D c = new Vector3D(1, 0, 0);
            //Vector3D a = new Vector3D(0, 0, 0);
            //Vector3D b = new Vector3D(0, 0, 1);

            Vector3D n = Vector3D.CrossProduct((b - a), (c - a));
            return n;

        }

        /// <summary>
        /// Calculate this object shading at point
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="t"></param>
        /// <param name="normal"></param>
        /// <param name="light"></param>
        /// <returns></returns>
        public override Vector3D Shade(Ray ray, double t, Vector3D normal, Light light)
        {
            // Point of intersection
            Vector3D p = ray.GetPoint3D(t);

            // Cache for calculation
            Vector3D color = new Vector3D(0, 0, 0);
            Vector3D dirToLight = p - light.Position;
            Ray shadowRay = new Ray(p, dirToLight);
            Vector3D pPrime = shadowRay.GetPoint3D(0.005);

            dirToLight = pPrime - light.Position;
            shadowRay = new Ray(pPrime, dirToLight);


            if (shadowRay.Hit() != null)
                return color;

            return color + Phong(p, light, ray);
        }

        /// <summary>
        /// Lambertian shading
        /// </summary>
        /// <param name="p"></param>
        /// <param name="l"></param>
        /// <param name="ray"></param>
        /// <returns></returns>
        public Vector3D Phong(Vector3D p, Light l, Ray ray)
        {
            // Object color
            Vector3D k = AlbedoColor;

            // normal of object
            Vector3D n = Normal(p);

            // Diffuse shading
            Vector3D diffuse = k * Math.Max(0, Vector3D.DotProduct(l.Position, n));

            Vector3D v = (ray.E - p);
            v.Normalize();
            double p_phong = 128;

            Vector3D h = v + l.Position;
            h.Normalize();

            Vector3D ls = k * Math.Pow(System.Math.Max(0, Vector3D.DotProduct(h, n)), p_phong);

            return diffuse + ls;
        }
    }
}
