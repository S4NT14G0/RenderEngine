using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RenderEngine.Rengine.Raycast
{
    class Ray
    {
        // Start point
        public Vector3D E { get; set; }
        // End point
        public Vector3D S { get; set; }

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

    }
}
