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
    public class Camera
    {
        Vector3D Position { get; set; }

        double FocalDistance { get; set; }

        RengineImage canvas = new RengineImage(1028, 720);

        public Camera()
        {
            RenderScene();
        }

        public RengineImage GetCanvas()
        {
            return canvas;
        }

        public void RenderScene ()
        {
            // Eye position
            Vector3D e = new Vector3D(0, 0, 0);

            // Focal distance
            float f = 130;

            // Light unit vector
            Vector3D l = new Vector3D(0.5, 0.5, 1);
            l.Normalize();

            // Background color
            Vector3D kb = new Vector3D(0, 0, 100);

            List<Sphere> spheres = new List<Sphere>();

            // Create spheres
            Vector3D center = new Vector3D(10, 10, -75);
            float radius = 30;
            Vector3D color = new Vector3D(100, 0, 0);
            Sphere s1 = new Sphere(center, radius, color);

            Vector3D center1 = new Vector3D(10, 10, -35);
            float radius1 = 10;
            Vector3D color1 = new Vector3D(100, 100, 0);
            Sphere s2 = new Sphere(center1, radius1, color1);

            spheres.Add(s1);
            spheres.Add(s2);

            for (int i = 0; i < 1028; i++)
            {
                for (int j = 0; j < 720; j++)
                {
                    Vector3D s = GetPoint_s(i, j, f, 1028, 720);

                    Ray ray = new Ray(e, s);

                    Vector3D col = RayHitColor(ray, l, spheres, kb);

                    canvas.SetPixel(i, j, System.Drawing.Color.FromArgb((int)col.X,(int) col.Y, (int)col.Z));
                }
            }
        }

        private Vector3D GetPoint_s (int i, int j, float focal, int canvasRows, int canvasCols)
        {
            float u = (float)((j - 1) - canvasCols / 2 + .5);
            float v =  (float)(-1 * ((i - 1 - canvasRows / 2 + .5)));

            return new Vector3D(u, v, -focal);
        }

        public Vector3D RayHitColor(Ray ray, Vector3D light, List<Sphere> spheres, Vector3D bgColor)
        {
            List<KeyValuePair<int, double>> tIntersections = new List<KeyValuePair<int, double>>();

            for (int i = 0; i < spheres.Count; i++)
            {
                double t = spheres[i].Intersect(ray);
                tIntersections.Add(new KeyValuePair<int, double>(i, t));
            }

            KeyValuePair<int, double> closestT = tIntersections.First<KeyValuePair<int, double>>();


            foreach (KeyValuePair<int, double> tList in tIntersections)
            {
                if (tList.Value < closestT.Value)
                    closestT = tList;
            }

            //KeyValuePair<int, double> closestT = tIntersections.Min();

            if (!double.IsInfinity(closestT.Value))
            {
                return Shade(spheres[closestT.Key], ray, closestT.Value, light);
            }
            else
            {
                return bgColor;
            }
        }

        public Vector3D Shade (Sphere sphere, Ray ray, double t, Vector3D l)
        {
            Vector3D p = ray.GetPoint3D(t);

            Vector3D color = new Vector3D(0, 0, 0);

            return color + Phong (sphere, p, l, ray);
        }

        public Vector3D Phong (Sphere sphere, Vector3D p, Vector3D l, Ray ray)
        {
            Vector3D k = sphere.AlbedoColor;

            Vector3D n = sphere.Normal(p);

            Vector3D diffuse = k * System.Math.Max(0, Vector3D.DotProduct(l, n));

            Vector3D v = (ray.E - p);
            v.Normalize();
            double p_phong = 128;
            Vector3D h = v + l;
            h.Normalize();

            Vector3D ls = k * System.Math.Pow(System.Math.Max(0, Vector3D.DotProduct(h, n)), p_phong);

            return diffuse + ls;
        }
    }

    
}
