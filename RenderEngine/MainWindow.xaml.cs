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

            Scene.Instance.SkyBoxColor = new Vector3D(30,144,255);

            Rengine.RengineObjects.Camera cam = new Rengine.RengineObjects.Camera(756, 1028);
            cam.Position = new Vector3D(0, 10, 0);
            cam.FocalDistance = 150;

            Rengine.RengineObjects.Light light = new Rengine.RengineObjects.Light(new Vector3D(.7, .7, .7));

            //Create spheres
            Vector3D center = new Vector3D(25, 15, -30);
            float radius = 10;
            Vector3D color = new Vector3D(101, 0, 0);
            Sphere s1 = new Sphere(center, radius, color);
            s1.Reflectiveness = .95;

            Vector3D center1 = new Vector3D(-25, 15, -27);
            float radius1 = 10;
            Vector3D color1 = new Vector3D(100, 100, 0);
            Sphere s2 = new Sphere(center1, radius1, color1);

            Vector3D center2 = new Vector3D(0, 15, -25);
            float radius2 = 10;
            Vector3D color2 = new Vector3D(100, 100, 0);
            Sphere s3 = new Sphere(center2, radius2, color2, "C:\\Users\\Santiago\\Documents\\GitHub\\RenderEngine\\RenderEngine\\res\\bball.jpg");

            Plane p = new Plane(new Vector3D(0 , 1, 0), 1);
            p.AlbedoColor = new Vector3D(255, 255, 255);

            Scene.Instance.SceneObjects.Add(s2);
            Scene.Instance.SceneObjects.Add(s1);
            Scene.Instance.SceneObjects.Add(s3);
            Scene.Instance.SceneObjects.Add(p);

            Scene.Instance.DirectionalLight = light;
            Scene.Instance.MainCamera = cam;

            ImageSource canvas = Scene.Instance.Render().GetImageSourceForBitmap();

            renderCanvas.Source = canvas;
            renderCanvas.Width = canvas.Height;
            renderCanvas.Height = canvas.Width;

        }
    }
}
