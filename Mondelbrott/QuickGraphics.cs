using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Mondelbrott
{
    class QuickGraphics
    {
        Int32Rect _fullSize;
        WriteableBitmap _memory;
        Color[] _colors;

        public QuickGraphics(double width, double height, Color[] colors)
            :this((int)width, (int)height, colors)
        {
        }

        public QuickGraphics(int width, int height, Color[] colors)
        {
            // dpi doesn't really matter
            var dpi = 96;

            // create in-memory image which will connected to real WPF image
            _memory = new WriteableBitmap(width, height, dpi, dpi, PixelFormats.Bgr32, null); 
            
            // remember the screen size
            _fullSize = new Int32Rect(0, 0, width, height);

            // remember the pallete
            _colors = colors;
        }

        /// <summary>
        /// Get image address to associate with WPF image
        /// </summary>
        /// <returns></returns>
        public ImageSource GetImage()
        {
            return _memory;
        }

        /// <summary>
        /// Clears all screen to a given color
        /// </summary>
        public void Clear(Color color)
        {            
            // store components of color in a memory-ready way
            byte[] pixel = Color2Bytes(color); // B,G,R,notused
            
            // the rectangle, filled with this color - created initially purely in memory to speed up 
            var screenFill = new byte[4 * _fullSize.Width * _fullSize.Height];

            // filling rectangle with colors
            int address = 0;
            for (int x = 0; x < _fullSize.Width; x++)
            {
                for (int y = 0; y < _fullSize.Height; y++)
                {
                    for (int c = 0; c < 4; c++)
                    {
                        screenFill[address + c] = pixel[c];
                    }
                    address += 4;
                }
            }

            // write to screen
            _memory.WritePixels(_fullSize, screenFill, 4 * _fullSize.Width, 0);
        }

        /// <summary>
        /// Write pixel directly to screen
        /// </summary>
        /// <param name="x">Image coordinate x</param>
        /// <param name="y">Image coordinate y</param>
        /// <param name="colorIndex">index of a color in an array</param>
        public void WritePixel(int x, int y, int colorIndex)
        {
            // prepare - a rectangle of 1 pixel
            var sourceData = new Int32Rect(x, y, 1, 1);
            byte[] bytes = Color2Bytes(_colors[colorIndex]);
            
            // write to screen
            _memory.WritePixels(sourceData, bytes, 4, 0);
        }

        /// <summary>
        /// Write pixel, but not update screen
        /// </summary>
        /// <param name="x">Image coordinate x</param>
        /// <param name="y">Image coordinate y</param>
        /// <param name="colorIndex">index of a color in an array</param>
        public void WritePixelDelayed(int x, int y, int colorIndex)
        {
            _memory.Lock();

            Color color = _colors[colorIndex];
            unsafe
            {
                // Get a pointer to the back buffer.
                int pBackBuffer = (int)_memory.BackBuffer;

                // Find the address of the pixel to draw.
                pBackBuffer += y * _memory.BackBufferStride;
                pBackBuffer += x * 4;

                // Compute the pixel's color.
                int color_data = (color.R << 16) | (color.G << 8) | color.B;
                    
                // Assign the color data to the pixel.
                *((int*)pBackBuffer) = color_data;
            }

            _memory.Unlock();
        }

        /// <summary>
        /// Refresh memory and write to screen
        /// </summary>
        public void RefreshDelayed()
        {
            _memory.Lock();

            _memory.AddDirtyRect(_fullSize);

            _memory.Unlock();
        }

        private byte[] Color2Bytes(Color color)
        {
            return new byte[] { color.B, color.G, color.R, 0 };
        }
    }
}
