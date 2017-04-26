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
            public int X;
            public int Y;
        }

        [DllImport("CTImageSegmentation.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static int CtSliceSegmentation(byte* intencity_input, int* regions_output,
            int image_height, int image_width, int filter_width = 9, int intencity_threshold = 60);

        private static SegmentsDetails instance = null; // Singleton

        private static int[,] regions = null;
        private static int _imageHeight = 0;
        private static int _imageWidth = 0;
        private static int regionsNumber = 0;

        // physical distances between the center of each pixel, measured in mm
        private double rowSpacing;
        private double columnSpacing;
        private Dictionary<int, double> regionsSize = null;

        private Imaging.ColorFactory colorFactory = null;

        private SegmentsDetails(byte[,] intencityInput, int imageHeight, int imageWidth, Array pixelSpacing,
            int filterWidth, int intencityThreshold)
        {
            InitializeComponent();

            _imageHeight = imageHeight;
            _imageWidth = imageWidth;
            rowSpacing = (double)pixelSpacing.GetValue(0, 0);
            columnSpacing = (double)pixelSpacing.GetValue(1, 0);
            regionsSize = new Dictionary<int, double>();

            regions = new int[_imageHeight, _imageWidth];
            // current slice segmentation
            SliceSegmentation(intencityInput, filterWidth, intencityThreshold);

            // generate new color factory
            colorFactory = new Imaging.ColorFactory(regionsNumber);

            // calculate regions size
            CalculateRegionsSize();

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
            // set bitmap to picture box
            Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(regions, _imageHeight, _imageWidth, colorFactory);
            pictureBox_segmentsDetails.Image = segmentedImage;

            // set regions size to list box
            List<int> regionsSizeKeys = regionsSize.Keys.ToList();
            regionsSizeKeys.Sort(); // sort keys by ascending as the list of colors in colorFactory

            listBox_segmentSizes.Items.Add("Max sizes of each segment (except border and air):");
            foreach(int sizeKey in regionsSizeKeys)
                listBox_segmentSizes.Items.Add(string.Format("{0} region, max size = {1} mm", sizeKey, Math.Round(regionsSize[sizeKey], 2)));
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

        /// <summary>
        /// Calculates max size of each region on the slice, except regions with id = -1 and id = 0 (border and air)
        /// </summary>
        private void CalculateRegionsSize()
        {
            // from top to bottom and from left to right (min x, any y)
            Dictionary<int, List<Coordinates>> leftCoordinates = new Dictionary<int, List<Coordinates>>();
            for (int x = 0; x < _imageWidth; x++)
            {
                for (int y = 0; y < _imageHeight; y++)
                {
                    int regionId = regions[y, x];
                    if (regionId == -1 || regionId == 0)
                        continue;

                    if (!leftCoordinates.ContainsKey(regionId))
                    {
                        List<Coordinates> newCoordinates = new List<Coordinates>();
                        newCoordinates.Add(new Coordinates { X = x, Y = y });
                        leftCoordinates.Add(regionId, newCoordinates);
                    }
                    else
                    {
                        for (int i = 0; i < leftCoordinates[regionId].Count; i++)
                        {
                            if (x == leftCoordinates[regionId].ElementAt(i).X)
                            {
                                leftCoordinates[regionId].Add(new Coordinates { X = x, Y = y });
                                break;
                            }
                        }
                    }
                }
            }

            // from left to right and from top to bottom (min y, any x)
            Dictionary<int, List<Coordinates>> topCoordinates = new Dictionary<int, List<Coordinates>>();
            for (int y = 0; y < _imageHeight; y++)
            {
                for (int x = 0; x < _imageWidth; x++)
                {
                    int regionId = regions[y, x];
                    if (regionId == -1 || regionId == 0)
                        continue;

                    if (!topCoordinates.ContainsKey(regionId))
                    {
                        List<Coordinates> newCoordinates = new List<Coordinates>();
                        newCoordinates.Add(new Coordinates { X = x, Y = y });
                        topCoordinates.Add(regionId, newCoordinates);
                    }
                    else
                    {
                        for (int i = 0; i < topCoordinates[regionId].Count; i++)
                        {
                            if (y == topCoordinates[regionId].ElementAt(i).Y)
                            {
                                topCoordinates[regionId].Add(new Coordinates { X = x, Y = y });
                                break;
                            }
                        }
                    }
                }
            }

            // from top to bottom and from right to left (max x, any y)
            Dictionary<int, List<Coordinates>> rightCoordinates = new Dictionary<int, List<Coordinates>>();
            for (int x = _imageWidth - 1; x > 0; x--)
            {
                for (int y = 0; y < _imageHeight; y++)
                {
                    int regionId = regions[y, x];
                    if (regionId == -1 || regionId == 0)
                        continue;

                    if (!rightCoordinates.ContainsKey(regionId))
                    {
                        List<Coordinates> newCoordinates = new List<Coordinates>();
                        newCoordinates.Add(new Coordinates { X = x, Y = y });
                        rightCoordinates.Add(regionId, newCoordinates);
                    }
                    else
                    {
                        for (int i = 0; i < rightCoordinates[regionId].Count; i++)
                        {
                            if (x == rightCoordinates[regionId].ElementAt(i).X)
                            {
                                rightCoordinates[regionId].Add(new Coordinates { X = x, Y = y });
                                break;
                            }
                        }
                    }
                }
            }

            // from right to left and from bottom to top (max y, any x)
            Dictionary<int, List<Coordinates>> bottomCoordinates = new Dictionary<int, List<Coordinates>>();
            for (int y = _imageHeight - 1; y > 0; y--)
            {
                for (int x = _imageWidth - 1; x > 0; x--)
                {
                    int regionId = regions[y, x];
                    if (regionId == -1 || regionId == 0)
                        continue;

                    if (!bottomCoordinates.ContainsKey(regionId))
                    {
                        List<Coordinates> newCoordinates = new List<Coordinates>();
                        newCoordinates.Add(new Coordinates { X = x, Y = y });
                        bottomCoordinates.Add(regionId, newCoordinates);
                    }
                    else
                    {
                        for (int i = 0; i < bottomCoordinates[regionId].Count; i++)
                        {
                            if (y == bottomCoordinates[regionId].ElementAt(i).Y)
                            {
                                bottomCoordinates[regionId].Add(new Coordinates { X = x, Y = y });
                                break;
                            }
                        }
                    }
                }
            }

            // calculate horizontal oriented sizes
            Dictionary<int, double> horizontalSizes = new Dictionary<int, double>();
            foreach (KeyValuePair<int, List<Coordinates>> it in leftCoordinates)
            {
                if (rightCoordinates.ContainsKey(it.Key))
                {
                    List<double> regionHorizontalSizes = new List<double>();
                    for (int i = 0; i < it.Value.Count; i++)
                    {
                        for (int j = 0; j < rightCoordinates[it.Key].Count; j++)
                        {
                            regionHorizontalSizes.Add
                            (
                                Math.Sqrt(
                                    Math.Pow(((Math.Abs(it.Value.ElementAt(i).X - rightCoordinates[it.Key].ElementAt(j).X)) * rowSpacing), 2) +
                                    Math.Pow(((Math.Abs(it.Value.ElementAt(i).Y - rightCoordinates[it.Key].ElementAt(j).Y)) * columnSpacing), 2)
                                )
                            );
                        }
                    }
                    horizontalSizes.Add(it.Key, regionHorizontalSizes.Max());
                }
            }

            // calculate vertical oriented sizes
            Dictionary<int, double> verticalSizes = new Dictionary<int, double>();
            foreach (KeyValuePair<int, List<Coordinates>> it in topCoordinates)
            {
                if (bottomCoordinates.ContainsKey(it.Key))
                {
                    List<double> regionVerticalSizes = new List<double>();
                    for (int i = 0; i < it.Value.Count; i++)
                    {
                        for (int j = 0; j < bottomCoordinates[it.Key].Count; j++)
                        {
                            regionVerticalSizes.Add
                            (
                                Math.Sqrt(
                                    Math.Pow(((Math.Abs(it.Value.ElementAt(i).X - bottomCoordinates[it.Key].ElementAt(j).X)) * rowSpacing), 2) +
                                    Math.Pow(((Math.Abs(it.Value.ElementAt(i).Y - bottomCoordinates[it.Key].ElementAt(j).Y)) * columnSpacing), 2)
                                )
                            );
                        }
                    }
                    verticalSizes.Add(it.Key, regionVerticalSizes.Max());
                }
            }

            // select max size between horizontal and vertical sizes
            foreach(KeyValuePair<int, double> it in horizontalSizes)
                if (verticalSizes.ContainsKey(it.Key))
                    regionsSize.Add(it.Key, Math.Max(it.Value, verticalSizes[it.Key]));
        }

        private void listBox_segmentSizes_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (sender == null)
                return;

            e.DrawBackground();
            Graphics g = e.Graphics;

            Color rectangleColor = e.BackColor;
            if (e.Index > 0) // all segments except border and air
            {
                var regionColor = colorFactory.CreatedColors[e.Index];
                rectangleColor = Color.FromArgb(regionColor.red, regionColor.green, regionColor.blue);
            }

            g.FillRectangle(new SolidBrush(rectangleColor), e.Bounds);

            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        private void button_closeForm_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SegmentsDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            regions = null;
            colorFactory = null;
            regionsSize = null;
            instance = null;

            Dispose();
        }

        private void SegmentsDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            // call garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
