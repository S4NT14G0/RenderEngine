using RenderEngine.Rengine.Raycast;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using RenderEngine.Rengine.ExtensionMethods;

namespace RenderEngine.Rengine.RengineObjects
{
    class Sphere
    {
        public Vector3D Center { get; set; }
        public double Radius { get; set; }
        public Color AlbedoColor { get; set; }

        public Sphere (Vector3D _center, double _radius, Color _albedoColor)
        {
            Center = _center;
            Radius = _radius;
            AlbedoColor = _albedoColor;
        }

        public double Intersect (Ray ray)
        {
            Vector3D d = ray.S - ray.E;

            return 1.0;
        }

    }
}
