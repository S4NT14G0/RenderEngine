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

        private static Scene instance;
        
        public static Scene Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Scene();
                }
                return instance;
            }
        }

        public RengineObjects.Camera MainCamera { get; set; }
        public RengineObjects.Light DirectionalLight { get; set; }

        public Vector3D SkyBoxColor { get; set; }
        public List<RengineObject> SceneObjects { get; set; }

        private Scene () {
            SceneObjects = new List<RengineObject>();
        }

        public RengineImage Render ()
        {
            MainCamera.RenderScene(this);
            return MainCamera.GetCanvas();
        }

        
    }
}
