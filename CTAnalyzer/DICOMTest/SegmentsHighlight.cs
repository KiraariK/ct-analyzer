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
    public partial class SegmentsHighlight : Form
    {
        private static SegmentsHighlight instanse = null; // Singleton

        private Array[] _dicomMatrices = null;
        private int[][,] _ctRegions = null;
        private byte[] _segmentsDencity = null; // contains dencity of each region except border - region with id = -1

        private int _imagesNumber = 0;
        private int _imageMatrixHeight = 0;
        private int _imageMatrixWidth = 0;

        private short _minIntencityBorder = 0;
        private short _maxIntencityBorder = 0;
        private short _minIntencityLevel = 0;
        private short _maxIntencityLevel = 0;

        private Imaging.ColorFactory _cFactory = null;

        private List<int> SelectedRegionsIndeces { get; set; }

        private SegmentsHighlight(Array[] dicomMatrices, int[][,] ctRegions, byte[] segmentsDencity,
            int imagesNumber, int imageMatrixHeight,
            int imageMatrixWidth, short minBorder, short maxBorder, short minIntencity, short maxIntencity,
            int imageDefaultIndex, Imaging.ColorFactory cFactory)
        {
            InitializeComponent();

            _dicomMatrices = dicomMatrices;
            _ctRegions = ctRegions;
            _segmentsDencity = segmentsDencity;

            _imagesNumber = imagesNumber;
            _imageMatrixHeight = imageMatrixHeight;
            _imageMatrixWidth = imageMatrixWidth;

            _minIntencityBorder = minBorder;
            _maxIntencityBorder = maxBorder;
            _minIntencityLevel = minIntencity;
            _maxIntencityLevel = maxIntencity;

            _cFactory = cFactory;

            SelectedRegionsIndeces = new List<int>();

            FillFormControls(imageDefaultIndex);
        }

        private void FillFormControls(int imageDefaultIndex)
        {
            // load DICOM image
            Bitmap dicomImage = ImageProcessing.GetBitmapFrom16Matrix(_dicomMatrices[imageDefaultIndex], _imageMatrixHeight, _imageMatrixWidth,
                _minIntencityBorder, _maxIntencityBorder, _minIntencityLevel, _maxIntencityLevel);
            pictureBox_DICOMImage.Image = dicomImage;

            // load segmented image
            Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(_ctRegions[imageDefaultIndex],
                _imageMatrixHeight, _imageMatrixWidth, _cFactory);
            pictureBox_segmentedIMage.Image = segmentedImage;

            trackBar.Maximum = _imagesNumber - 1;
            trackBar.Value = imageDefaultIndex;
            label_trackBarValue.Text = imageDefaultIndex.ToString();

            // fill listBox
            for (int i = 0; i < _segmentsDencity.Length; i++)
                listBox_segmentIndices.Items.Add(i.ToString());
        }

        public static SegmentsHighlight GetInstanse(Array[] dicomMatrices, int[][,] ctRegions, byte[] segmentsDencity,
            int imagesNumber, int imageMatrixHeight,
            int imageMatrixWidth, short minBorder, short maxBorder, short minIntencity, short maxIntencity,
            int imageDefaultIndex, Imaging.ColorFactory cFactory)
        {
            if (instanse == null)
                instanse = new SegmentsHighlight(dicomMatrices, ctRegions, segmentsDencity,
                    imagesNumber, imageMatrixHeight,
                    imageMatrixWidth, minBorder, maxBorder, minIntencity, maxIntencity,
                    imageDefaultIndex, cFactory);

            return instanse;
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

            if (_dicomMatrices != null)
            {
                if (SelectedRegionsIndeces.Count <= 0)
                {
                    Bitmap dicomImage = ImageProcessing.GetBitmapFrom16Matrix(_dicomMatrices[currentIndex], _imageMatrixHeight, _imageMatrixWidth,
                        _minIntencityBorder, _maxIntencityBorder, _minIntencityLevel, _maxIntencityLevel);
                    pictureBox_DICOMImage.Image = dicomImage;
                }
                else
                {
                    Bitmap highlightedDicomImage = ImageProcessing.GetBitmapFrom16MatrixWithHeighlights(_dicomMatrices[currentIndex],
                        _ctRegions[currentIndex], _segmentsDencity, SelectedRegionsIndeces,
                        _imageMatrixHeight, _imageMatrixWidth,
                        _minIntencityBorder, _maxIntencityBorder, _minIntencityLevel, _maxIntencityLevel);
                    pictureBox_DICOMImage.Image = highlightedDicomImage;
                }
            }

            if (_ctRegions != null)
            {
                Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(_ctRegions[currentIndex],
                    _imageMatrixHeight, _imageMatrixWidth, _cFactory);
                pictureBox_segmentedIMage.Image = segmentedImage;
            }

            label_trackBarValue.Text = currentIndex.ToString();
        }

        private void listBox_segmentIndices_DrawItem(object sender, DrawItemEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (sender == null)
                return;

            e.DrawBackground();
            Graphics g = e.Graphics;

            Color rectangleColor = e.BackColor;
            if (e.Index > 0) // because there is no region with id = 0 (air region)
            {
                var regionColor = _cFactory.CreatedColors[e.Index];
                rectangleColor = Color.FromArgb(regionColor.red, regionColor.green, regionColor.blue);
            }

            g.FillRectangle(new SolidBrush(rectangleColor), e.Bounds);

            e.Graphics.DrawString(lb.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        private void listBox_segmentIndices_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (sender == null)
                return;

            // actualize the list of selected regions indeces
            for (int i = 0; i < lb.Items.Count; i++)
            {
                if (lb.GetSelected(i) == true)
                {
                    int regionIndex = int.Parse(lb.Items[i].ToString());
                    if (!SelectedRegionsIndeces.Contains(regionIndex))
                        SelectedRegionsIndeces.Add(int.Parse(lb.Items[i].ToString()));
                }
                else
                {
                    int regionIndex = int.Parse(lb.Items[i].ToString());
                    if (SelectedRegionsIndeces.Contains(regionIndex))
                        SelectedRegionsIndeces.Remove(regionIndex);

                }
            }

            // reload images according to the list of selected regions indeces
            int currentIndex = trackBar.Value;
            Bitmap highlightedDicomImage = ImageProcessing.GetBitmapFrom16MatrixWithHeighlights(_dicomMatrices[currentIndex],
                _ctRegions[currentIndex], _segmentsDencity, SelectedRegionsIndeces,
                _imageMatrixHeight, _imageMatrixWidth,
                _minIntencityBorder, _maxIntencityBorder, _minIntencityLevel, _maxIntencityLevel);
            pictureBox_DICOMImage.Image = highlightedDicomImage;
        }

        private void button_closeForm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
