using RenderEngine.Rengine.Raycast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RenderEngine.Rengine.RengineObjects
{
    public abstract class RengineObject
    {
        public abstract Vector3D Center { get; set; }
        public abstract Vector3D AlbedoColor { get; set; }

        public abstract double Intersect(Ray ray);
        public abstract Vector3D Normal(Vector3D point);

        public abstract Vector3D GetColor(Ray ray, Vector3D lightVector);

    }
}
