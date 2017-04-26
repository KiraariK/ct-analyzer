using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DICOMopener
{
    public partial class SegmentsSizes : Form
    {
        public struct Coordinates
        {
            public int X;
            public int Y;
        }

        private static SegmentsSizes instance = null; // Singleton

        private int[][,] _ctRegions = null;
        private byte[] _segmentsDencity = null; // contains dencity of each region except border - region with id = -1
        private Imaging.ColorFactory _colorFactory = null;

        private int _imagesNumber = 0;
        private int _imageHeight = 0;
        private int _imageWidth = 0;

        // physical distances between the center of each pixel, measured in mm
        private double rowSpacing = 0;
        private double columnSpacing = 0;
        private Dictionary<int, List<double>> regionsSizes = null; // max size of a region on each slice

        private SegmentsSizes(int[][,] ctRegions, Imaging.ColorFactory colorFactory, byte[] segmentsDencity,
            int imagesNumber, int imageHeight, int imageWidth, Array pixelSpacing, int defaultImageIndex)
        {
            InitializeComponent();

            _ctRegions = ctRegions;
            _colorFactory = colorFactory;
            _segmentsDencity = segmentsDencity;

            _imagesNumber = imagesNumber;
            _imageHeight = imageHeight;
            _imageWidth = imageWidth;

            rowSpacing = (double)pixelSpacing.GetValue(0, 0);
            columnSpacing = (double)pixelSpacing.GetValue(1, 0);
            regionsSizes = new Dictionary<int, List<double>>();

            // calculate sizes of each region on each slice
            DateTime startCalc = DateTime.Now;
            CalculateRegionsSizes();
            TimeSpan calcDuration = DateTime.Now - startCalc;

            FillFormControlls(defaultImageIndex, calcDuration);
        }

        public static SegmentsSizes GetInstance(int[][,] ctRegions, Imaging.ColorFactory colorFactory, byte[] segmentsDencity,
            int imagesNumber, int imageHeight, int imageWidth, Array pixelSpacing, int defaultImageIndex)
        {
            if (instance == null)
                instance = new SegmentsSizes(ctRegions, colorFactory, segmentsDencity, imagesNumber, 
                    imageHeight, imageWidth, pixelSpacing, defaultImageIndex);

            return instance;
        }

        /// <summary>
        /// Calculates all max sizes of regions on each slice (except region with id = -1 - border and id = 0 - air)
        /// </summary>
        private void CalculateRegionsSizes()
        {
            foreach (int[,] regions in _ctRegions)
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
                foreach (KeyValuePair<int, double> it in horizontalSizes)
                {
                    if (verticalSizes.ContainsKey(it.Key))
                    {
                        if (regionsSizes.ContainsKey(it.Key))
                            regionsSizes[it.Key].Add(Math.Max(it.Value, verticalSizes[it.Key]));
                        else
                        {
                            List<double> regionSizesList = new List<double>();
                            regionSizesList.Add(Math.Max(it.Value, verticalSizes[it.Key]));
                            regionsSizes.Add(it.Key, regionSizesList);
                        }
                    }
                }
            }
        }

        private void FillFormControlls(int defaultIndex, TimeSpan calculationDuration)
        {
            Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(_ctRegions[defaultIndex],
                _imageHeight, _imageWidth, _colorFactory);
            pictureBox_segmentedImage.Image = segmentedImage;

            trackBar.Maximum = _imagesNumber - 1;
            trackBar.Value = defaultIndex;
            label_trackBarValue.Text = defaultIndex.ToString();

            // get list of regions id
            List<int> regionsSizesKeys = regionsSizes.Keys.ToList();
            regionsSizesKeys.Sort(); // sort keys by ascending as the list of colors in colorFactory

            listBox_segments.Items.Add("Seg and dencity");
            foreach (int regionId in regionsSizesKeys)
                listBox_segments.Items.Add(string.Format("Seg {0} - {1}", regionId, (_segmentsDencity[regionId]) > 0 ? "high" : "low"));

            toolStripStatusLabel.Text = "Calculation time is " + calculationDuration.ToString();
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            TrackBar bar = (TrackBar)sender;
            if (bar == null)
                return;

            label_trackBarValue.Text = bar.Value.ToString();
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar bar = (TrackBar)sender;
            if (bar == null)
                return;

            int currentIndex = bar.Value;

            if (_ctRegions != null)
            {
                Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(_ctRegions[currentIndex],
                    _imageHeight, _imageWidth, _colorFactory);
                pictureBox_segmentedImage.Image = segmentedImage;
            }

            label_trackBarValue.Text = currentIndex.ToString();
        }

        private void listBox_segments_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (sender == null)
                return;

            e.DrawBackground();
            Graphics g = e.Graphics;

            Color rectangleColor = e.BackColor;
            if (e.Index > 0) // because there is no region with id = 0 (air region)
            {
                var regionColor = _colorFactory.CreatedColors[e.Index];
                rectangleColor = Color.FromArgb(regionColor.red, regionColor.green, regionColor.blue);
            }

            g.FillRectangle(new SolidBrush(rectangleColor), e.Bounds);

            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        private void listBox_segments_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (sender == null)
                return;

            if (lb.SelectedIndex == 0)
                return;

            string regionId = lb.SelectedItem.ToString().Split(' ')[1];

            listBox_sizes.Items.Clear();

            foreach (double size in regionsSizes[int.Parse(regionId)])
                listBox_sizes.Items.Add(string.Format("{0} mm", Math.Round(size, 2)));
        }

        private void button_closeForm_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SegmentsSizes_FormClosing(object sender, FormClosingEventArgs e)
        {
            _ctRegions = null;
            _segmentsDencity = null;
            _colorFactory = null;
            regionsSizes = null;
            instance = null;

            Dispose();
        }

        private void SegmentsSizes_FormClosed(object sender, FormClosedEventArgs e)
        {
            // call garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void SegmentsSizes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (trackBar.Maximum == 0)
                return;

            if (e.KeyChar == (char)Keys.Right)
            {
                if (trackBar.Value >= trackBar.Maximum)
                    return;

                trackBar.Value += 1;
                return;
            }

            if (e.KeyChar == (char)Keys.Left)
            {
                if (trackBar.Value <= 0)
                    return;

                trackBar.Value -= 1;
                return;
            }
        }
    }
}
