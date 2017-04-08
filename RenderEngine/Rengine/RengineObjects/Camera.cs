using RenderEngine.Rengine.Raycast;
using RenderEngine.Rengine.RengineScene;
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
        public Vector3D Position { get; set; }

        public double FocalDistance { get; set; }

        RengineImage canvas;

        public Camera(int renderWidth, int renderHeight)
        {
            canvas = new RengineImage(renderWidth, renderHeight);
        }

        public RengineImage GetCanvas()
        {
            return canvas;
        }

        public void RenderScene (Scene scene)
        {
            // Light unit vector
            Vector3D l = scene.DirectionalLight.Position;
            l.Normalize();

            // Background color
            Vector3D kb = scene.SkyBoxColor;

            for (int i = 0; i < canvas.GetWidth(); i++)
            {
                for (int j = 0; j < canvas.GetHeight(); j++)
                {
                    Vector3D s = GetPoint_s(i, j, FocalDistance, 1028, 720);

                    Ray ray = new Ray(Position, s);

                    Vector3D col = ColorAtRayHit(ray, l, scene.SceneObjects, kb);

                    canvas.SetPixel(i, j, System.Drawing.Color.FromArgb((int)col.X,(int) col.Y, (int)col.Z));
                }
            }
        }

        private Vector3D GetPoint_s (int i, int j, double focal, int canvasRows, int canvasCols)
        {
            float u = (float)((j - 1) - canvasCols / 2 + .5);
            float v =  (float)(-1 * ((i - 1 - canvasRows / 2 + .5)));

            return new Vector3D(u, v, -focal);
        }

        public Vector3D ColorAtRayHit(Ray ray, Vector3D light, List<RengineObject> sceneObjects, Vector3D bgColor)
        {
            List<KeyValuePair<int, double>> tIntersections = new List<KeyValuePair<int, double>>();

            for (int i = 0; i < sceneObjects.Count; i++)
            {
                double t = sceneObjects[i].Intersect(ray);
                tIntersections.Add(new KeyValuePair<int, double>(i, t));
            }

            KeyValuePair<int, double> closestT = tIntersections.First<KeyValuePair<int, double>>();


            foreach (KeyValuePair<int, double> tList in tIntersections)
            {
                if (tList.Value < closestT.Value)
                    closestT = tList;
            }

            if (!double.IsInfinity(closestT.Value))
            {
                return Shade(sceneObjects[closestT.Key], ray, closestT.Value, light);
            }
            else
            {
                return bgColor;
            }
        }

        public Vector3D Shade (RengineObject rengineObject, Ray ray, double t, Vector3D l)
        {
            Vector3D p = ray.GetPoint3D(t);

            Vector3D color = new Vector3D(0, 0, 0);
            //return rengineObject.AlbedoColor + Phong(rengineObject, p, l, ray);
            return color + Phong (rengineObject, p, l, ray);
        }

        public Vector3D Phong (RengineObject rengineObject, Vector3D p, Vector3D l, Ray ray)
        {
            Vector3D k = rengineObject.AlbedoColor;

            Vector3D n = rengineObject.Normal(p);

            Vector3D diffuse = k * Math.Max(0, Vector3D.DotProduct(l, n));

            Vector3D v = (ray.E - p);

            v.Normalize();
            double p_phong = 128;

            Vector3D h = v + l;
            h.Normalize();

            Vector3D ls = k * Math.Pow(System.Math.Max(0, Vector3D.DotProduct(h, n)), p_phong);

            return diffuse + ls;
        }
    }

    
}
