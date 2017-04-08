using RenderEngine.Rengine.RengineObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace RenderEngine.Rengine.RengineScene
{
    public class Scene
    {
        public RengineObjects.Camera MainCamera { get; set; }
        public RengineObjects.Light DirectionalLight { get; set; }

        public Vector3D SkyBoxColor { get; set; }
        public List<RengineObject> SceneObjects { get; set; }

        public Scene () {
            SceneObjects = new List<RengineObject>();
        }

        public RengineImage Render ()
        {
            MainCamera.RenderScene(this);
            return MainCamera.GetCanvas();
        }

        
    }
}
