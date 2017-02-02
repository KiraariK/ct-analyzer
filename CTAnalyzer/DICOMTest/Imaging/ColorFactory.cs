using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMopener.Imaging
{
    public class ColorFactory
    {
        public struct MyColors
        {
            public byte red;
            public byte green;
            public byte blue;
        }

        public List<MyColors> CreatedColors { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="colorsNumber">Number of colors to generate</param>
        public ColorFactory(int colorsNumber)
        {
            Random rnd = new Random();
            CreatedColors = new List<MyColors>();

            int r, g, b;
            while (CreatedColors.Count < colorsNumber)
            {
                do
                {
                    r = rnd.Next(0, 256);
                    g = rnd.Next(0, 256);
                    b = rnd.Next(0, 256);
                } while (ContainsColor(r, g, b));

                CreatedColors.Add(new MyColors
                {
                    red = (byte)r,
                    green = (byte)g,
                    blue = (byte)b
                });
            }
        }

        /// <summary>
        /// Returns true if list of colors already contains the color
        /// </summary>
        /// <param name="r">Value of red channel of the color</param>
        /// <param name="g">Value of green channel of the color</param>
        /// <param name="b">Value of blue channel of the color</param>
        /// <returns></returns>
        private bool ContainsColor(int r, int g, int b)
        {
            return CreatedColors.Contains(new MyColors
            {
                red = (byte)r,
                green = (byte)g,
                blue = (byte)b
            });
        }
    }
}
