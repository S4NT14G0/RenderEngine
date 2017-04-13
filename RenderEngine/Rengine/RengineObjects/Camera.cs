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

        /// <summary>
        /// Raycasting function
        /// </summary>
        /// <param name="scene"></param>
        public void RenderScene (Scene scene)
        {
            // Light unit vector
            Vector3D l = scene.DirectionalLight.Position;
            //l.Normalize();

            // Background color
            Vector3D kb = scene.SkyBoxColor;

            for (int i = 0; i < canvas.GetWidth(); i++)
            {
                for (int j = 0; j < canvas.GetHeight(); j++)
                {
                    // Construct ray from pixel
                    Vector3D s = GetPoint_s(i, j, FocalDistance, canvas.GetWidth(), canvas.GetHeight());

                    // Construct a new ray from camera position towards the pixel
                    Ray ray = new Ray(Position, s);

                    // Get color where the ray hits
                    Vector3D col = ColorAtRayHit(ray, l, scene.SceneObjects, kb);

                    // Normalize the coords so they don't go out of range
                    int r = (col.X >= 0  && col.X <= 255) ? (int)col.X : 0;
                    int g = (col.Y >= 0 && col.Y <= 255) ? (int)col.Y: 0;
                    int b = (col.Z >= 0 && col.Z<= 255) ? (int)col.Z : 0;


                    canvas.SetPixel(i, j, Color.FromArgb(255, r, g, b));
                }
            }
        }

        private Vector3D GetPoint_s (int i, int j, double focal, int canvasRows, int canvasCols)
        {
            float u = (float)((j - 1) - canvasCols / 2 + .5);
            float v =  (float)(-1 * ((i - 1 - canvasRows / 2 + .5)));

            return new Vector3D(u, v, -focal);
        }

        /// <summary>
        /// Returns color at ray hit
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="light"></param>
        /// <param name="sceneObjects"></param>
        /// <param name="bgColor"></param>
        /// <returns></returns>
        public Vector3D ColorAtRayHit(Ray ray, Vector3D light, List<RengineObject> sceneObjects, Vector3D bgColor)
        {
            
            // Index and t value
            List<KeyValuePair<int, double>> tIntersections = new List<KeyValuePair<int, double>>();

            // Store all the keyvalue pairs
            for (int i = 0; i < sceneObjects.Count; i++)
            {
                double t = sceneObjects[i].Intersect(ray);
                tIntersections.Add(new KeyValuePair<int, double>(i, t));
            }

            // Cache for after loop
            KeyValuePair<int, double> closestT = tIntersections.First<KeyValuePair<int, double>>();

            // Sort the closest hit
            foreach (KeyValuePair<int, double> tList in tIntersections)
            {
                if (tList.Value < closestT.Value)
                    closestT = tList;
            }

            // Has it hit the skybox
            if (!double.IsInfinity(closestT.Value))
            {
                // Get the shade at the hit point
                return Shade(sceneObjects, sceneObjects[closestT.Key], ray, closestT.Value, light);
            }
            else
            {
                return bgColor;
            }
        }

        /// <summary>
        /// Shade the intersected object
        /// </summary>
        /// <param name="rengineObject"></param>
        /// <param name="ray"></param>
        /// <param name="t"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public Vector3D Shade (List<RengineObject> sceneObjects, RengineObject rengineObject, Ray ray, double t, Vector3D l)
        {
            // Point of intersection
            Vector3D p = ray.GetPoint3D(t);

            // Cache for calculation
            Vector3D color = new Vector3D(0, 0, 0);

            if (IsShadowAtPoint(p, l, sceneObjects))
                return color;
            
            return color + Phong(rengineObject, p, l, ray);

        }

        public Vector3D Phong (RengineObject rengineObject, Vector3D p, Vector3D l, Ray ray)
        {
            // Object color
            Vector3D k = rengineObject.AlbedoColor;

            // normal of object
            Vector3D n = rengineObject.Normal(p);

            // Diffuse shading
            Vector3D diffuse = k * Math.Max(0, Vector3D.DotProduct(l, n));

            Vector3D v = (ray.E - p);
            v.Normalize();
            double p_phong = 128;

            Vector3D h = v + l;
            h.Normalize();

            Vector3D ls = k * Math.Pow(System.Math.Max(0, Vector3D.DotProduct(h, n)), p_phong);

            return diffuse + ls;
        }

        Boolean IsShadowAtPoint (Vector3D p, Vector3D l, List<RengineObject> sceneObjects)
        {
            // Shoot ray from p to light
            // If it hits light then we return false
            // If there is interesection return true

            Ray rayToLight = new Ray(l, p);
            Vector3D sigma = rayToLight.GetPoint3D(4);
            rayToLight = new Ray(sigma, l);

            foreach (RengineObject sceneObject in sceneObjects)
            {
                if (!double.IsInfinity(sceneObject.Intersect(rayToLight))) {
                    return true;
                }
            }

            return false;
        }
    }

    
}
