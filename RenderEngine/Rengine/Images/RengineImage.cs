﻿using System;
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
    class RengineImage
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
            pixels = new int[width, height];

            // Fill array with random values
            Random random = new Random();
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    bitmapImage.SetPixel(x, y, System.Drawing.Color.FromArgb(random.Next(255), random.Next(255), random.Next(255), random.Next(255)));
                }
            }
        }

        public void SetPixel (int x, int y, System.Drawing.Color color)
        {
            bitmapImage.SetPixel(x, y, color);
        }

        public Bitmap GetBitmapImage ()
        {
            return bitmapImage;
        }

        public ImageSource GetImageSourceForBitmap ()
        {
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