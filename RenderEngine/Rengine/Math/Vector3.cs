using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace RenderEngine.Rengine.Math
{
    class Vector3
    {
        private double x, y, z;

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                MaskedVector[0] = x;
            }
        }
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                MaskedVector[1] = y;
            }
        }
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
                MaskedVector[2] = z;
            }
        }

        public Vector<double> MaskedVector { get; }

        public Vector3 (double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;

            double[] arrayInitializer = { X, Y, Z };
            var builder = Vector<double>.Build;
            MaskedVector = builder.DenseOfArray(arrayInitializer);
        }

        // Override the ToString method to display an complex number in the suitable format:
        public override string ToString()
        {
            return MaskedVector[0] + ", " + MaskedVector[1] + ", " + MaskedVector[2];
        }

        #region math operations
        Vector3 Add (Vector3 operand)
        {
            Vector<double> sum = MaskedVector + operand.MaskedVector;
            return new Vector3(sum[0], sum[1], sum[2]);
        }

        Vector3 Subtract (Vector3 operand)
        {
            Vector<double> difference = MaskedVector - operand.MaskedVector;
            return new Vector3(difference[0], difference[1], difference[2]);
        }

        Vector3 Multiply(double scalar)
        {
            Vector<double> product = MaskedVector * scalar;
            return new Vector3(product[0], product[1], product[2]);
        }

        Vector3 Divide (Vector3 operand)
        {
            Vector<double> quotient = MaskedVector / operand.MaskedVector;
            return new Vector3(quotient[0], quotient[1], quotient[2]);
        }

        double Dot (Vector3 lhs, Vector3 rhs)
        {
            return lhs.MaskedVector.DotProduct(rhs.MaskedVector);
        }

        public Vector3 Normal ()
        {
            Vector<double> normVector = MaskedVector.Normalize(1);
            return new Vector3(normVector[0], normVector[1], normVector[2]);
        }
        #endregion

        #region operator overloads
        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return lhs.Add(rhs);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return lhs.Subtract(rhs);
        }

        public static Vector3 operator *(Vector3 lhs, double rhs)
        {
            return lhs.Multiply(rhs);
        }

        public static double operator * (Vector3 lhs, Vector3 rhs)
        {
            return lhs.MaskedVector * rhs.MaskedVector;
        }

        public static Vector3 operator / (Vector3 lhs, Vector3 rhs)
        {
            return lhs.Divide(rhs);
        }

        #endregion
    }
}
