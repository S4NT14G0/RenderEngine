using System;
using System.Collections.Generic;
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

        RengineImage renderedCanvas = new RengineImage(300,300);
        
    }
}
