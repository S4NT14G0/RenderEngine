using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace RenderEngine.Rengine
{
    public class RengineImage
    {
        int width;
        int height;

        Bitmap bitmapImage;

        // Pixels representing image
        int[,] pixels;

        public RengineImage (int _width, int _height)
        {
            width = _width;
            height = _height;

            bitmapImage = new Bitmap(width, height);
            bitmapImage.SetResolution(width, height);
            pixels = new int[width, height];
        }

        public void RotateClockwise ()
        {
            bitmapImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }


        public void SetPixel (int x, int y, System.Drawing.Color color)
        {
            bitmapImage.SetPixel(x, y, color);
        }

        public int GetWidth () { return this.width; }
        public int GetHeight () { return this.height; }

        public Bitmap GetBitmapImage ()
        {
            return bitmapImage;
        }

        public ImageSource GetImageSourceForBitmap ()
        {
            RotateClockwise();
            var handle = bitmapImage.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            } finally
            {
                DeleteObject(handle);
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

    }
}
