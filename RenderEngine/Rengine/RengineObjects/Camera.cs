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
            //Vector3D l = scene.DirectionalLight.Position;
            //l.Normalize();

            // Background color
            Vector3D kb = scene.SkyBoxColor;

            Vector3D[,] pixels = new Vector3D[canvas.GetWidth(), canvas.GetHeight()];

            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    // Construct ray from pixel
                    Vector3D s = GetPoint_s(i, j, FocalDistance, pixels.GetLength(0), pixels.GetLength(1));

                    // Construct a new ray from camera position towards the pixel
                    Ray ray = new Ray(Position, s);

                    // Get color where the ray hits
                    Vector3D col = ColorAtRayHit(ray, scene.DirectionalLight, scene.SceneObjects, kb);

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
        public Vector3D ColorAtRayHit(Ray ray, Light light, List<RengineObject> sceneObjects, Vector3D bgColor)
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
                Vector3D normal = sceneObjects[closestT.Key].Normal(ray.GetPoint3D(closestT.Value));

                // Get the shade at the hit point
                return sceneObjects[closestT.Key].Shade(ray, closestT.Value, normal, light);
            }
            else
            {
                return bgColor;
            }
        }

    }
    
}
