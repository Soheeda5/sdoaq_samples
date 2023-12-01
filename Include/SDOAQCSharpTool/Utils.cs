﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SDOAQCSharpTool
{
    public static class Utils
    {
        public static uint RGB(byte r, byte g, byte b)
        {
            return (uint)(r | (g << 8) | (b << 16));
        }

        public static string GetWsioLastError()
        {
            var sbLastError = new StringBuilder(4 * 1024);
            SDOWSIO.WSIO.WSIO_LastErrorString(sbLastError, sbLastError.Length);
            return sbLastError.ToString();
        }


        public static Bitmap ConvertByteToBitmap(byte[] data, int size, int idx, string title, int width, int height, bool bColor)
        {
            Bitmap bmp = new Bitmap(width, height);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            {
                if (bColor)
                {
                    // Get the address of the first line.
                    IntPtr ptr = bmpData.Scan0;
                    Marshal.Copy(data, 0, ptr, size);
                }
                else
                {
                    byte[] byteArray;

                    size = size * 3;
                    byteArray = new byte[size];

                    Parallel.For(0, size / 3, index =>
                    {
                        byte gray = (byte)data[index];

                        byteArray[index * 3] = gray;
                        byteArray[index * 3 + 1] = gray;
                        byteArray[index * 3 + 2] = gray;
                    });

                    // Get the address of the first line.
                    IntPtr ptr = bmpData.Scan0;
                    Marshal.Copy(byteArray, 0, ptr, size);
                }
            }
            bmp.UnlockBits(bmpData);
            
            return bmp;
        }
    }
}
