using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RenderEngine.Rengine.RengineObjects
{
    public class Light
    {
        public Vector3D Position { get; set; }

        public Light (Vector3D _position)
        {
            Position = _position;
        }
    }
}
