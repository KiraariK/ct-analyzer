using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMopener
{
    public class ImageProcessing
    {
        /// <summary>
        /// Returns Bitmap created from 32 bit matrix
        /// </summary>
        /// <param name="matrix">Matrix of image 32 bit intensities</param>
        /// <param name="height">Number of strings in matrix</param>
        /// <param name="width">Number of columns in matrix</param>
        /// <returns></returns>
        public static Bitmap GetBitmapFrom32Matrix(int[,] matrix, int height, int width)
        {
            Bitmap resultBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte BytesCount = 3;
                BitmapData UpdatingData = resultBitmap.LockBits(new Rectangle(
                    new Point(0, 0), resultBitmap.Size), ImageLockMode.WriteOnly, resultBitmap.PixelFormat);

                for (int i = 0; i < height; i++)
                {
                    byte* BitmapRowPtr = (byte*)UpdatingData.Scan0 + i * UpdatingData.Stride;
                    for (int j = 0; j < width; j++)
                    {
                        int ColorPosition = j * BytesCount;
                        BitmapRowPtr[ColorPosition] = (byte)matrix[i, j];
                        BitmapRowPtr[ColorPosition + 1] = (byte)matrix[i, j];
                        BitmapRowPtr[ColorPosition + 2] = (byte)matrix[i, j];
                    }
                }

                resultBitmap.UnlockBits(UpdatingData);
            }

            return resultBitmap;
        }

        /// <summary>
        /// Returns Bitmap created from Array, consisted of 16 bit values
        /// </summary>
        /// <param name="matrix">Array as a matrix of image 16 bit intensities</param>
        /// <param name="height">Number of strings in matrix</param>
        /// <param name="width">Number of columns in matrix</param>
        /// <param name="minIntencity">Default min intensity in any CT image</param>
        /// <param name="maxIntencity">Default max intensity in any CT image</param>
        /// <returns></returns>
        //public static Bitmap GetBitmapFrom16Matrix(Array matrix, int height, int width, short minIntencity, short maxIntencity)
        //{
        //    Bitmap resultBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

        //    unsafe
        //    {
        //        byte BytesCount = 3;
        //        BitmapData UpdatingData = resultBitmap.LockBits(new Rectangle(
        //            new Point(0, 0), resultBitmap.Size), ImageLockMode.WriteOnly, resultBitmap.PixelFormat);

        //        for (int i = 0; i < height; i++)
        //        {
        //            byte* BitmapRowPtr = (byte*)UpdatingData.Scan0 + i * UpdatingData.Stride;
        //            for (int j = 0; j < width; j++)
        //            {
        //                int ColorPosition = j * BytesCount;
        //                short valueFromArray = (short)matrix.GetValue(i, j);

        //                // Exclude noninterest areas from an image
        //                byte valueForImage = 0;
        //                if (valueFromArray > 0)
        //                    valueForImage = (byte)(((valueFromArray - minIntencity) / (float)(maxIntencity - minIntencity)) * 255);

        //                // Include noninterest areas in an image
        //                //byte valueForImage = (byte)(((valueFromArray - minIntencity) / (float)(maxIntencity - minIntencity)) * 255);

        //                BitmapRowPtr[ColorPosition] = valueForImage;
        //                BitmapRowPtr[ColorPosition + 1] = valueForImage;
        //                BitmapRowPtr[ColorPosition + 2] = valueForImage;
        //            }
        //        }

        //        resultBitmap.UnlockBits(UpdatingData);
        //    }

        //    return resultBitmap;
        //}
        
        public static Bitmap GetBitmapFrom16Matrix(Array matrix, int height, int width, short minBorder, short maxBorder, 
            short minIntencity, short maxIntencity)
        {
            if (minIntencity < minBorder)
                minIntencity = minBorder;
            if (maxIntencity > maxBorder)
                maxIntencity = maxBorder;

            Bitmap resultBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte BytesCount = 3;
                BitmapData UpdatingData = resultBitmap.LockBits(new Rectangle(
                    new Point(0, 0), resultBitmap.Size), ImageLockMode.WriteOnly, resultBitmap.PixelFormat);

                for (int i = 0; i < height; i++)
                {
                    byte* BitmapRowPtr = (byte*)UpdatingData.Scan0 + i * UpdatingData.Stride;
                    for (int j = 0; j < width; j++)
                    {
                        int ColorPosition = j * BytesCount;
                        short valueFromArray = (short)matrix.GetValue(i, j);

                        // Exclude noninterest areas from an image
                        byte valueForImage = 0;
                        if (valueFromArray >= minBorder && valueFromArray <= maxBorder)
                        {
                            // if value is outside of the electronic window
                            // Comment to show values only inside the area
                            if (valueFromArray < minIntencity)
                                valueFromArray = minIntencity;
                            if (valueFromArray > maxIntencity)
                                valueFromArray = maxIntencity;

                            // Uncomment to show values only inside the area
                            //if (valueFromArray < minIntencity)
                            //    valueFromArray = minBorder;
                            //if (valueFromArray > maxIntencity)
                            //    valueFromArray = minBorder;

                            valueForImage = (byte)(((valueFromArray - minIntencity) / (float)(maxIntencity - minIntencity)) * 255);
                        }

                        BitmapRowPtr[ColorPosition] = valueForImage;
                        BitmapRowPtr[ColorPosition + 1] = valueForImage;
                        BitmapRowPtr[ColorPosition + 2] = valueForImage;
                    }
                }

                resultBitmap.UnlockBits(UpdatingData);
            }

            return resultBitmap;
        }
    }
}
