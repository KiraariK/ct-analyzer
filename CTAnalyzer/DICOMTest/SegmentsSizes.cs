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
        private static SegmentsSizes instance = null; // Singleton

        private int[][,] _ctRegions = null;
        private byte[] _segmentsDencity = null;
        private Imaging.ColorFactory _colorFactory = null;

        private int _imagesNumber = 0;
        private int _imageHeight = 0;
        private int _imageWidth = 0;

        private double rowSpacing = 0;
        private double columnSpacing = 0;


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

            FillFormControlls(defaultImageIndex);
        }

        public static SegmentsSizes GetInstance(int[][,] ctRegions, Imaging.ColorFactory colorFactory, byte[] segmentsDencity,
            int imagesNumber, int imageHeight, int imageWidth, Array pixelSpacing, int defaultImageIndex)
        {
            if (instance == null)
                instance = new SegmentsSizes(ctRegions, colorFactory, segmentsDencity, imagesNumber, 
                    imageHeight, imageWidth, pixelSpacing, defaultImageIndex);

            return instance;
        }

        private void FillFormControlls(int defaultIndex)
        {
            Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(_ctRegions[defaultIndex],
                _imageHeight, _imageWidth, _colorFactory);
            pictureBox_segmentedImage.Image = segmentedImage;

            trackBar.Maximum = _imagesNumber - 1;
            trackBar.Value = defaultIndex;
            label_trackBarValue.Text = defaultIndex.ToString();

            for (int i = 0; i < _segmentsDencity.Length; i++)
                listBox_segments.Items.Add(string.Format("Seg {0} - {1}", i, (_segmentsDencity[i]) > 0 ? "high" : "low"));
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
            var regionColor = _colorFactory.CreatedColors[e.Index];
            rectangleColor = Color.FromArgb(regionColor.red, regionColor.green, regionColor.blue);

            g.FillRectangle(new SolidBrush(rectangleColor), e.Bounds);

            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
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
            instance = null;

            Dispose();
        }

        private void SegmentsSizes_FormClosed(object sender, FormClosedEventArgs e)
        {
            // call garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
