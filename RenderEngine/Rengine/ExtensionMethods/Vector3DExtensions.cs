using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RenderEngine.Rengine.ExtensionMethods
{
    public static class Vector3DExtensions
    {
        public static Vector3D Transpose(this Vector3D vector3D)
        {
            return new Vector3D(vector3D.Z, vector3D.Y, vector3D.X);
        }
    }
}
