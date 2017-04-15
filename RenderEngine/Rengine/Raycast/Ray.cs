using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using RenderEngine.Rengine.RengineScene;
using RenderEngine.Rengine.RengineObjects;

namespace RenderEngine.Rengine.Raycast
{
    public class Ray
    {
        // Start point
        public Vector3D E { get; set; }
        // End point
        public Vector3D S { get; set; }

        /// <summary>
        /// Constructs a ray in 3D space
        /// </summary>
        /// <param name="_e">Start of ray</param>
        /// <param name="_s">End of ray</param>
        public Ray(Vector3D _e, Vector3D _s)
        {
            this.E = _e;
            this.S = _s;
        }

        public Vector3D GetPoint3D (double t)
        {
            // p(t) = e + (s - e) * t
            return E + (S - E) * t;
        }

        public RengineObject Hit()
        {

            // Index and t value
            List<KeyValuePair<int, double>> tIntersections = new List<KeyValuePair<int, double>>();

            // Store all the keyvalue pairs
            for (int i = 0; i < Scene.Instance.SceneObjects.Count; i++)
            {
                double t = Scene.Instance.SceneObjects[i].Intersect(this);
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
                Vector3D normal = Scene.Instance.SceneObjects[closestT.Key].Normal(this.GetPoint3D(closestT.Value));

                // Get the shade at the hit point
                return Scene.Instance.SceneObjects[closestT.Key];
            }
            else
            {
                return null;
            }
        }
    }
}
