using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using RenderEngine.Rengine.Raycast;

namespace RenderEngine.Rengine.RengineObjects
{
    class Plane : RengineObject
    {
        public override Vector3D AlbedoColor { get; set; }

        public override Vector3D Center { get; set; }

        private Triangle triangle;

        public Plane(Vector3D center)
        {
            Center = center;
            triangle = new Triangle(Center);
        }

        public override Vector3D GetColor(Ray ray, Vector3D lightVector)
        {
            throw new NotImplementedException();
        }

        public override double Intersect(Ray ray)
        {
            Vector3D p1 = triangle.GetVerticies()[0];

            double numerator = Vector3D.DotProduct((p1 - ray.E), Normal(p1));
            double denominator = Vector3D.DotProduct
        }

        public override Vector3D Normal(Vector3D point)
        {
            Vector3D a = triangle.GetVerticies()[1];
            Vector3D b = triangle.GetVerticies()[2];
            Vector3D c = triangle.GetVerticies()[0];

            return Vector3D.CrossProduct((b - a), (c - a));
        }
    }

    class Triangle
    {
        Vector3D c;
        
        Vector3D[] verticies;

        public Triangle (Vector3D center)
        {
            c = center;

            verticies = new Vector3D[3];

            // C
            verticies[0] = center;
            // A
            verticies[1] =  new Vector3D(center.X + 10, center.Y, center.Z);
            // B
            verticies[2] = new Vector3D(center.X, center.Y, center.Z + 10);
        }

        public Vector3D[] GetVerticies()
        {
            return verticies;
        }
    }
}
