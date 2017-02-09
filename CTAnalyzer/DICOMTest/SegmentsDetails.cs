using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DICOMopener
{
    public partial class SegmentsDetails : Form
    {
        public struct Coordinates
        {
            public int leftX;
            public int leftY;

            public int rightX;
            public int rightY;

            public int topX;
            public int topY;

            public int botomX;
            public int botomY;
        }

        [DllImport("CTImageSegmentation.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static int CtSliceSegmentation(byte* intencity_input, int* regions_output,
            int image_height, int image_width, int filter_width = 9, int intencity_threshold = 60);

        private static SegmentsDetails instance = null; // Singleton

        private static  int[,] regions = null;
        private static int _imageHeight = 0;
        private static int _imageWidth = 0;
        private static int regionsNumber = 0;

        // physical distances between the center of each pixel, measured in mm
        private double rowSpacing;
        private double columnSpacing;

        private Imaging.ColorFactory colorFactory = null;

        private SegmentsDetails(byte[,] intencityInput, int imageHeight, int imageWidth, Array pixelSpacing, 
            int filterWidth, int intencityThreshold)
        {
            InitializeComponent();

            _imageHeight = imageHeight;
            _imageWidth = imageWidth;
            rowSpacing = (double)pixelSpacing.GetValue(0, 0);
            columnSpacing = (double)pixelSpacing.GetValue(1, 0);

            regions = new int[_imageHeight, _imageWidth];
            // current slice segmentation
            SliceSegmentation(intencityInput, filterWidth, intencityThreshold);

            // generate new color factory
            colorFactory = new Imaging.ColorFactory(regionsNumber);

            FillFormControlls();
        }

        public static SegmentsDetails GetInstance(byte[,] intencityInput, int imageHeight, int imageWidth, Array pixelSpacing,
            int filterWidth, int intencityThreshold)
        {
            if (instance == null)
                instance = new SegmentsDetails(intencityInput, imageHeight, imageWidth, pixelSpacing, filterWidth, intencityThreshold);

            return instance;
            
        }

        private void FillFormControlls()
        {
            Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(regions, _imageHeight, _imageWidth, colorFactory);
            pictureBox_segmentsDetails.Image = segmentedImage;
        }

        private unsafe static void SliceSegmentation(byte[,] intencity, int filterWidth, int intencityThreshold)
        {
            // prepare data for using in C function
            int imageSize = _imageHeight * _imageWidth;
            byte[] intencityInput = new byte[imageSize];
            int[] regionsOutput = new int[imageSize];

            for (int i = 0; i < _imageHeight; i++)
                for (int j = 0; j < _imageWidth; j++)
                    intencityInput[(i * _imageHeight) + j] = intencity[i, j];

            // using C function
            fixed (byte* intencityInput_ptr = intencityInput)
            {
                fixed (int* regionsOutput_ptr = regionsOutput)
                {
                    regionsNumber = CtSliceSegmentation(intencityInput_ptr, regionsOutput_ptr,
                        _imageHeight, _imageWidth, filterWidth, intencityThreshold);

                    // write result data to C# variables
                    for (int i = 0; i < _imageHeight; i++)
                        for (int j = 0; j < _imageWidth; j++)
                            regions[i, j] = regionsOutput_ptr[(i * _imageHeight) + j];
                }
            }
        }

        private void LocateRegions()
        {
            // TODO: implement a method to locate edge coordinates of regions
        }

        /// <summary>
        /// Forced Garbale Collector call
        /// </summary>
        private void ClearGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void listBox_segmentSizes_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void button_closeForm_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SegmentsDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            regions = null;
            colorFactory = null;
            instance = null;

            ClearGarbage();
            Dispose();
        }
    }
}
