using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RenderEngine.Rengine;
using RenderEngine.Rengine.RengineObjects;
using RenderEngine.Rengine.RengineScene;
using System.Windows.Media.Media3D;

namespace RenderEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Scene scene = new Scene();

            scene.SkyBoxColor = new Vector3D(30,144,255);

            Rengine.RengineObjects.Camera cam = new Rengine.RengineObjects.Camera(768, 768);
            cam.Position = new Vector3D(0, 0, 0);
            cam.FocalDistance = 130;

            Rengine.RengineObjects.Light light = new Rengine.RengineObjects.Light(new Vector3D(.5, .5, 1));

            // Create spheres
            Vector3D center = new Vector3D(15, 50, -80);
            float radius = 30;
            Vector3D color = new Vector3D(101, 0, 0);
            Sphere s1 = new Sphere(center, radius, color);

            Vector3D center1 = new Vector3D(-10, 20, -30);
            float radius1 = 10;
            Vector3D color1 = new Vector3D(100, 100, -20);
            Sphere s2 = new Sphere(center1, radius1, color1);

            Plane p = new Plane(new Vector3D(0 ,-1, 1), 1);
            p.AlbedoColor = new Vector3D(124, 252, 0);

            scene.SceneObjects.Add(s1);
            scene.SceneObjects.Add(s2);
            scene.SceneObjects.Add(p);

            scene.DirectionalLight = light;
            scene.MainCamera = cam;

            renderCanvas.Source =  scene.Render().GetImageSourceForBitmap();
        }
    }
}
