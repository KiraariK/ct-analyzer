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
    public partial class SegmentsDetails : Form
    {
        private static SegmentsDetails instance; // Singleton

        private int[,] _regions;
        private int _imageHeight;
        private int _imageWidth;
        private Imaging.ColorFactory _cFactory;
        private Array _pixelSpacing;

        private SegmentsDetails(int[,] regions, int imageHeight, int imageWidth, 
            Imaging.ColorFactory cFactory, Array pixelSpacing)
        {
            InitializeComponent();

            _regions = regions;
            _imageHeight = imageHeight;
            _imageWidth = imageWidth;
            _cFactory = cFactory;
            _pixelSpacing = pixelSpacing;

            FillFormControlls();
        }

        public static SegmentsDetails GetInstance(int[,] regions, int imageHeight, int imageWidth,
            Imaging.ColorFactory cFactory, Array pixelSpacing)
        {
            if (instance == null)
                instance = new SegmentsDetails(regions, imageHeight, imageWidth, cFactory, pixelSpacing);

            return instance;
            
        }

        private void FillFormControlls()
        {
            Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(_regions, _imageHeight, _imageWidth, _cFactory);
            pictureBox_segmentsDetails.Image = segmentedImage;
        }

        private void listBox_segmentSizes_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void button_closeForm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
