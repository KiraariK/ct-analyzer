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
        /// Returns Bitmap created from 16 bit matrix
        /// </summary>
        /// <param name="matrix">Matrix of DICOM values</param>
        /// <param name="height">Height of each slice in DICOM set</param>
        /// <param name="width">Width in each slice in DICOM set</param>
        /// <param name="minBorder">The bottom level of DICOM velues range</param>
        /// <param name="maxBorder">The top level of DICOM values range</param>
        /// <param name="minIntencity">The bottom level of electronic window</param>
        /// <param name="maxIntencity">The top level of electronic window</param>
        /// <returns></returns>
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
