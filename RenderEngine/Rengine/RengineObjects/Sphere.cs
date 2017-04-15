 using RenderEngine.Rengine.Raycast;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RenderEngine.Rengine.RengineObjects
{
    public class Sphere : RengineObject
    {
        public override Vector3D Center { get; set; }
        public double Radius { get; set; }

        public double Reflectiveness { get; set; }

        public double Refractiveness { get; set; }
        public override Vector3D AlbedoColor { get; set; }

        public Sphere (Vector3D _center, double _radius, Vector3D _albedoColor)
        {
            Center = _center;
            Radius = _radius;
            AlbedoColor = _albedoColor;
        }

        public Sphere()
        {
            Reflectiveness = 0;
        }

        public override double Intersect (Ray ray)
        {
            Vector3D d = ray.S - ray.E;

            Vector3D center = Center;
            double radius = Radius;

            double a = Vector3D.DotProduct(d, d);
            double b = Vector3D.DotProduct(d, ray.E - center) * 2; //(d * (ray.E - center)) * 2.0;
            double c = Vector3D.DotProduct((ray.E - center),(ray.E - center)) - radius * radius;

            double delta = b * b - 4.0 * a * c;

            if (delta < 0)
            {
                return double.PositiveInfinity;
            } else
            {
                double t1 = (-1 * b - System.Math.Sqrt(delta)) / (2.0 * a);
                double t2 = (-1 * b + System.Math.Sqrt(delta)) / (2.0 * a);

                return System.Math.Min(t1, t2);
            }
        }

        public override Vector3D Normal(Vector3D point)
        {
            Vector3D normalPoint = (point - Center);
            normalPoint.Normalize();
            return normalPoint;
        }

        public override Vector3D GetColor (Ray ray, Vector3D light)
        {
            double t = Intersect(ray);

            if (!double.IsInfinity(t))
            {
                Vector3D p = ray.GetPoint3D(t);

                Vector3D n = Normal(p);
                Vector3D one = new Vector3D(1, 1, 1);
                Vector3D ld = AlbedoColor * System.Math.Max(0, Vector3D.DotProduct(n , one));
                Vector3D v = ray.E - p;
                v.Normalize();
                double phong = 128;
                Vector3D h = v + light;
                h.Normalize();
                Vector3D ls = AlbedoColor * System.Math.Pow(System.Math.Max (0, Vector3D.DotProduct(h , n)), phong);
                return ld + ls;
            } else
            {
                return new Vector3D(0, 0, 0);
            }
        }

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


            if (shadowRay.Hit() != null && shadowRay.Hit() != this)
                return color;

            return color + Phong(p, light, ray);
        }

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

            // Direction of the bounced light
            Vector3D h = v + l.Position;
            h.Normalize();

            // Reflection vector
            Vector3D r = 2 * (Vector3D.DotProduct(n, v)) * n - v;

            // Reflective color
            Ray reflectionRay = new Ray(p, p - r);
            Vector3D pPrime = reflectionRay.GetPoint3D(.005);
            reflectionRay = new Raycast.Ray(pPrime, pPrime - r);
            Vector3D lm = ColorOfReflection(new Ray(p, p - r), l);

            Vector3D ls = k * Math.Pow(Math.Max(0, Vector3D.DotProduct(h, n)), p_phong);
            return diffuse + ls + lm;
        }

        Vector3D ColorOfReflection(Ray ray, Light light)
        {
            RengineObject re = ray.Hit();

            if (re != null && re != this)
            {
                double t = re.Intersect(ray);
                Vector3D p = ray.GetPoint3D(t);
                return re.Shade(ray, t, this.Normal(p), light) * Reflectiveness;
            }
            return new Vector3D(0, 0, 0);
        }
    }
}
