using System;
using System.Drawing;
using System.Windows.Forms;

using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

using DICOMWorker;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DICOMopener
{
    public partial class MainForm : Form
    {
        [DllImport("CTImageSegmentation.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public extern static byte* CtSegmentation(byte* intencity_input, int* regions_output, int* regions_number,
            int images_number, int image_height, int image_width, int filter_width = 9, int intencity_threshold = 60);

        private static string[] filenames = null;
        private static Array[] dicomMatrices = null; // contains DICOM units of each ct slice per array element
        private static int imageMatrixHeight = 0;
        private static int imageMatrixWidth = 0;

        private static int[][,] ctRegions = null; // contains region indexes as 2D array in an array of all slices
        private static byte[] segmentsDencity = null; // contains values (0 or 255) of regions' dencity (array's index - region number) except border - region with id = -1
        private static int filterWidth = 0; // filter width for ct image segmentation
        private static int intencityThreshold = 0; // intencity threshold for ct image segmentation
        private static int segmentsNumber = 0;
        private Imaging.ColorFactory colorFactory = null; // set of colors for segmented image visualization

        private DICOMReader reader; // defining the MATLAB DICOMReader Class

        private static EWindow eWindow; // defining EWindow parameters

        public MainForm()
        {
            InitializeComponent();
            pictureBox_DICOMImage.Image = null;
            trackBar.Maximum = 0;
            trackBar.Value = 0;
            toolStripStatusLabel.Text = " ";
            label_trackBarValue.Text = " ";

            reader = new DICOMReader(); // creating the instance of MATLAB DICOMReader Class. It is important to create instance here for 32 bit.

            // EWindow parameters
            eWindow = new EWindow(EWindowType.EWType.Wide);

            comboBox_EWindowType.Items.Clear();
            foreach (KeyValuePair<EWindowType.EWType, string> it in eWindow.MinLevel.EWTypeLabels)
                comboBox_EWindowType.Items.Add(it.Value);

            FillEWindowParametersView();
        }

        /// <summary>
        /// Fills the controls of EWindow parameters on the Form
        /// </summary>
        private void FillEWindowParametersView()
        {
            comboBox_EWindowType.SelectedItem = eWindow.WindowLabel;

            if (eWindow.isDICOMunits)
            {
                radioButton_DICOM.Checked = true;
                radioButton_Hounsfield.Checked = false;

                textBox_EWindowCenter.Text = eWindow.CenterDICOM.ToString();
                label_recomendedEWindowCenter.Text = eWindow.WindowCenterLabelDICOM;

                textBox_EWindowWidth.Text = eWindow.WidthDICOM.ToString();
                label_recomendedEWindowWidth.Text = eWindow.WindowWidthLabelDICOM;
            }
            else
            {
                radioButton_DICOM.Checked = false;
                radioButton_Hounsfield.Checked = true;

                textBox_EWindowCenter.Text = eWindow.CenterHounsfield.ToString();
                label_recomendedEWindowCenter.Text = eWindow.WindowCenterLabelHounsfield;

                textBox_EWindowWidth.Text = eWindow.WidthHounsfield.ToString();
                label_recomendedEWindowWidth.Text = eWindow.WindowWidthLabelHounsfield;
            }
        }

        /// <summary>
        /// Reloads a seleted image
        /// </summary>
        private void CurrentDICOMImageReloading()
        {
            if (dicomMatrices == null) // DICOM files are not loaded
                return;

            int currentIndex = trackBar.Value;
            Bitmap DICOMImage = ImageProcessing.GetBitmapFrom16Matrix(dicomMatrices[currentIndex], imageMatrixHeight, imageMatrixWidth,
                eWindow.MinBorder.DICOMUnit, eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
            pictureBox_DICOMImage.Image = DICOMImage;

            // remove all information about segmented images
            ClearSegmentationData();
            pictureBox_segmentedImage.Image = null;
        }

        /// <summary>
        /// Clear the data structures for DICOM data
        /// </summary>
        private void ClearDICOMData()
        {
            filenames = null;
            dicomMatrices = null;
            imageMatrixHeight = 0;
            imageMatrixWidth = 0;
        }

        /// <summary>
        /// Clear the data structures for segmentation data
        /// </summary>
        private void ClearSegmentationData()
        {
            ctRegions = null;
            segmentsDencity = null;
            filterWidth = 0;
            intencityThreshold = 0;
            segmentsNumber = 0;
            colorFactory = null;
        }

        /// <summary>
        /// Forced Garbale Collector call
        /// </summary>
        private void ClearGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Do segmentation of all ct slices in 3D. Includes creation of intencity image of a current eWindow.
        /// Function location is for reduce cost of memory using global variables.
        /// </summary>
        /// <param name="filterWidth">Width of filter kernel for median filtering during the segmentation</param>
        /// <param name="intencityThreshold">Threshold for thresholding filtration after median filtering</param>
        /// <param name="minBorder">The bottom level of DICOM velues range</param>
        /// <param name="maxBorder">The top level of DICOM values range</param>
        /// <param name="minIntencity">The bottom level of electronic window</param>
        /// <param name="maxIntencity">The top level of electronic window</param>
        unsafe private static void SegmentLungs(int filterWidth, int intencityThreshold,
            short minBorder, short maxBorder, short minIntencity, short maxIntencity)
        {
            // prepare data for using in C function
            int imagesNumber = dicomMatrices.Length;
            int imageSize = imageMatrixHeight * imageMatrixWidth;
            byte[] intencityInput = new byte[imagesNumber * imageSize];
            int[] regionsOutput = new int[imagesNumber * imageSize];
            int regionsNumber;

            // set input parameters as image intencity of current electronic window
            if (minIntencity < minBorder)
                minIntencity = minBorder;
            if (maxIntencity > maxBorder)
                maxIntencity = maxBorder;

            for (int k = 0; k < imagesNumber; k++)
            {
                for (int i = 0; i < imageMatrixHeight; i++)
                {
                    for (int j = 0; j < imageMatrixWidth; j++)
                    {
                        short valueFromArray = (short)dicomMatrices[k].GetValue(i, j);

                        byte valueForImage = 0;
                        if (valueFromArray >= minBorder && valueFromArray <= maxBorder)
                        {
                            // if value is outside of the electronic window
                            if (valueFromArray < minIntencity)
                                valueFromArray = minIntencity;
                            if (valueFromArray > maxIntencity)
                                valueFromArray = maxIntencity;

                            valueForImage = (byte)(((valueFromArray - minIntencity) / (float)(maxIntencity - minIntencity)) * 255);
                        }

                        intencityInput[(k * imageSize) + (i * imageMatrixHeight) + j] = valueForImage;
                    }
                }
            }

            // allocate memory for global regions array
            ctRegions = new int[imagesNumber][,];
            for (int i = 0; i < imagesNumber; i++)
                ctRegions[i] = new int[imageMatrixHeight, imageMatrixWidth];

            // using C function
            fixed (byte* intencityInput_ptr = intencityInput)
            {
                fixed (int* regionsOutput_ptr = regionsOutput)
                {
                    int* regionsNumber_ptr = &regionsNumber; // there is no need to fix the local variable
                    byte* regionsDencity_ptr = CtSegmentation(intencityInput_ptr, regionsOutput_ptr, regionsNumber_ptr, imagesNumber,
                        imageMatrixHeight, imageMatrixWidth, filterWidth, intencityThreshold);

                    segmentsNumber = *regionsNumber_ptr;
                    segmentsDencity = new byte[segmentsNumber];
                    for (int i = 0; i < segmentsNumber; i++)
                        segmentsDencity[i] = regionsDencity_ptr[i];

                    // write result data to C# variables
                    for (int k = 0; k < imagesNumber; k++)
                        for (int i = 0; i < imageMatrixHeight; i++)
                            for (int j = 0; j < imageMatrixWidth; j++)
                                ctRegions[k][i, j] = regionsOutput_ptr[(k * imageSize) + (i * imageMatrixHeight) + j];
                }
            }
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            pictureBox_DICOMImage.Image = null;
            pictureBox_segmentedImage.Image = null;
            trackBar.Maximum = 0;
            trackBar.Value = 0;
            toolStripStatusLabel.Text = " ";
            label_trackBarValue.Text = " ";

            ClearDICOMData();
            ClearSegmentationData();

            // Garbage Collector force call
            ClearGarbage();

            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "dicom files (*.dcm)|*.dcm|(*.dicom)|*.dicom|(*.DCM)|*.DCM|(*.DICOM)|*.DICOM";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog.FileNames.Length > 0)
                {
                    filenames = new string[openFileDialog.FileNames.Length];
                    filenames = openFileDialog.FileNames;
                }
                else
                    return;
            }
            else
                return;

            dicomMatrices = new Array[filenames.Length];

            // Loading dicom files
            DateTime startLoadFiles = DateTime.Now;
            for (int i = 0; i < dicomMatrices.Length; i++)
            {
                MWCharArray matlabFilename = new MWCharArray(new string(filenames[i].ToCharArray())); // creating a MATLAB char array

                var matlabImageMatrix = reader.dicom_read(matlabFilename); // invoking the dicom_read method of MATLAB DICOMReader Class instance. Returns a MWNumericArray value

                dicomMatrices[i] = matlabImageMatrix.ToArray();
            }
            TimeSpan loadFilesTime = DateTime.Now - startLoadFiles;

            // Getting image parameters
            ArrayProcessor aProcessor = new ArrayProcessor(dicomMatrices[0]);
            imageMatrixHeight = aProcessor.GetStringsCount();
            imageMatrixWidth = aProcessor.GetColumnsCount();

            // Creating default image from data
            int defaultIndex = 0;
            Bitmap DICOMImage = ImageProcessing.GetBitmapFrom16Matrix(dicomMatrices[defaultIndex], imageMatrixHeight, imageMatrixWidth,
                eWindow.MinBorder.DICOMUnit, eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
            pictureBox_DICOMImage.Image = DICOMImage;

            trackBar.Maximum = dicomMatrices.Length - 1;
            trackBar.Value = defaultIndex;
            toolStripStatusLabel.Text = string.Format("Loaded {0} files in {1}",
                dicomMatrices.Length, loadFilesTime);
            label_trackBarValue.Text = defaultIndex.ToString();
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

            if (dicomMatrices != null)
            {
                Bitmap DICOMImage = ImageProcessing.GetBitmapFrom16Matrix(dicomMatrices[currentIndex], imageMatrixHeight, imageMatrixWidth,
                    eWindow.MinBorder.DICOMUnit, eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
                pictureBox_DICOMImage.Image = DICOMImage;
            }

            if (ctRegions != null)
            {
                Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(ctRegions[currentIndex],
                    imageMatrixHeight, imageMatrixWidth, colorFactory);
                pictureBox_segmentedImage.Image = segmentedImage;
            }

            label_trackBarValue.Text = currentIndex.ToString();
        }

        private void button_saveImage_Click(object sender, EventArgs e)
        {
            if (pictureBox_DICOMImage.Image == null)
            {
                MessageBox.Show("Nothing to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            saveFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox_DICOMImage.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                toolStripStatusLabel.Text = string.Format("Original image '{0}' has been saved", saveFileDialog.FileName);
            }
        }

        private void button_saveAllImages_Click(object sender, EventArgs e)
        {
            if (dicomMatrices == null)
            {
                MessageBox.Show("Nothing to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (folderBrowserDialog.SelectedPath != null)
                {
                    for (int i = 0; i < dicomMatrices.Length; i++)
                    {
                        string DICOMImageName = folderBrowserDialog.SelectedPath + "\\";
                        string strIndex = i.ToString();
                        switch (strIndex.Length)
                        {
                            case 1:
                                DICOMImageName += "00" + strIndex;
                                break;
                            case 2:
                                DICOMImageName += "0" + strIndex;
                                break;
                            default:
                                DICOMImageName += strIndex;
                                break;
                        }

                        DICOMImageName += ".bmp";
                        Bitmap DICOMImage = ImageProcessing.GetBitmapFrom16Matrix(dicomMatrices[i], imageMatrixHeight, imageMatrixWidth,
                            eWindow.MinBorder.DICOMUnit, eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
                        DICOMImage.Save(DICOMImageName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    toolStripStatusLabel.Text = string.Format("{0} orignial images have been saved", dicomMatrices.Length);
                }
            }
        }

        private void comboBox_EWindowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox == null)
                return;

            EWindowType.EWType newType = EWindowType.EWType.Wide;
            foreach (KeyValuePair<EWindowType.EWType, string> it in eWindow.MinLevel.EWTypeLabels)
            {
                if (comboBox.SelectedItem.ToString() == it.Value)
                {
                    newType = it.Key;
                    break;
                }
            }
            if (newType == eWindow.MinLevel.Type) // If new Item is the same as old item
                return;

            if (eWindow.isDICOMunits)
                eWindow = new EWindow(newType, true);
            else
                eWindow = new EWindow(newType, false);

            FillEWindowParametersView();
            CurrentDICOMImageReloading();
        }

        private void radioButton_DICOM_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton == null)
                return;

            if (radioButton.Checked) // radio button DICOM is active
            {
                eWindow.isDICOMunits = true;
                FillEWindowParametersView();
            }
        }

        private void radioButton_Hounsfield_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton == null)
                return;

            if (radioButton.Checked) // radio button Hounsfield is active
            {
                eWindow.isDICOMunits = false;
                FillEWindowParametersView();
            }
        }

        private void button_EWindowParametersApply_Click(object sender, EventArgs e)
        {
            int centerValue = -1;
            if (!int.TryParse(textBox_EWindowCenter.Text, out centerValue))
            {
                MessageBox.Show("The center value must be numberic", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int widthValue = -1;
            if (!int.TryParse(textBox_EWindowWidth.Text, out widthValue))
            {
                MessageBox.Show("The width value must be numberic", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EWindowType.EWType newType = EWindowType.EWType.Wide;
            foreach (KeyValuePair<EWindowType.EWType, string> it in eWindow.MinLevel.EWTypeLabels)
            {
                if (comboBox_EWindowType.SelectedItem.ToString() == it.Value)
                {
                    newType = it.Key;
                    break;
                }
            }

            bool isDICOMunits = eWindow.isDICOMunits;

            eWindow = new EWindow(newType, (short)centerValue, (short)widthValue, isDICOMunits);

            FillEWindowParametersView();
            CurrentDICOMImageReloading();
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button_doSegmentation_Click(object sender, EventArgs e)
        {
            if (dicomMatrices == null) // DICOM files are not loaded
                return;

            if (numericUpDown_segmentationFilterWidth.Value < 1 && numericUpDown_segmentationFilterWidth.Value > 11)
            {
                MessageBox.Show("Filter width should be set between 1 and 11", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numericUpDown_segmentationIntencityThreshold.Value < 0 && numericUpDown_segmentationIntencityThreshold.Value > 255)
            {
                MessageBox.Show("Intencity threshold should be set between 0 and 255", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ctRegions != null &&
                filterWidth == (int)numericUpDown_segmentationFilterWidth.Value &&
                intencityThreshold == (int)numericUpDown_segmentationIntencityThreshold.Value)
            {
                DialogResult doSegmentationAgainResult =
                    MessageBox.Show("You have actual results of segmentation\nDo you want to do it again?",
                    "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (doSegmentationAgainResult == DialogResult.No)
                    return;
            }

            // prepare for a new segmentation
            ClearSegmentationData();
            pictureBox_segmentedImage.Image = null;

            // Garbage Collector force call
            ClearGarbage();

            // start new segmentation
            filterWidth = (int)numericUpDown_segmentationFilterWidth.Value;
            intencityThreshold = (int)numericUpDown_segmentationIntencityThreshold.Value;

            // segmentation
            DateTime startSegmentationTime = DateTime.Now;
            try
            {
                SegmentLungs(filterWidth, intencityThreshold, eWindow.MinBorder.DICOMUnit,
                    eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
            }
            catch (Exception ex)
            {
                SEHException seh_ex = (SEHException)ex;
                if (seh_ex != null)
                    MessageBox.Show("Probably, the Out of Memory Exception was occured in an external component\nTry to use less slices",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // segmentation failed; return to the previous state
                ClearSegmentationData();
                ClearGarbage();
                return;
            }
            TimeSpan segmentationTime = DateTime.Now - startSegmentationTime;

            // creating colors for segmented image visualization
            colorFactory = new Imaging.ColorFactory(segmentsNumber);

            Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(ctRegions[trackBar.Value],
                imageMatrixHeight, imageMatrixWidth, colorFactory);
            pictureBox_segmentedImage.Image = segmentedImage;

            toolStripStatusLabel.Text = string.Format("Segmentation is finished in {0}", segmentationTime);
        }

        private void button_saveSegmentedImage_Click(object sender, EventArgs e)
        {
            if (pictureBox_segmentedImage.Image == null)
            {
                MessageBox.Show("Nothing to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            saveFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox_segmentedImage.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                toolStripStatusLabel.Text = string.Format("Segmented image '{0}' has been saved", saveFileDialog.FileName);
            }
        }

        private void button_saveAllSegmentedImages_Click(object sender, EventArgs e)
        {
            if (ctRegions == null)
            {
                MessageBox.Show("Nothing to save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (folderBrowserDialog.SelectedPath != null)
                {
                    for (int i = 0; i < dicomMatrices.Length; i++)
                    {
                        string segmentedImageName = folderBrowserDialog.SelectedPath + "\\";
                        string strIndex = i.ToString();
                        switch (strIndex.Length)
                        {
                            case 1:
                                segmentedImageName += "00" + strIndex;
                                break;
                            case 2:
                                segmentedImageName += "0" + strIndex;
                                break;
                            default:
                                segmentedImageName += strIndex;
                                break;
                        }

                        segmentedImageName += ".bmp";
                        Bitmap segmentedImage = ImageProcessing.GetColoredSegmentedImage(ctRegions[i],
                            imageMatrixHeight, imageMatrixWidth, colorFactory);
                        segmentedImage.Save(segmentedImageName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    toolStripStatusLabel.Text = string.Format("{0} segmented images have been saved", dicomMatrices.Length);
                }
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            // clear interface information
            pictureBox_DICOMImage.Image = null;
            pictureBox_segmentedImage.Image = null;
            trackBar.Maximum = 0;
            trackBar.Value = 0;
            toolStripStatusLabel.Text = " ";
            label_trackBarValue.Text = " ";

            // clear DICOM data
            ClearDICOMData();

            // clear data for segmentation
            ClearSegmentationData();

            // Garbage Collector force call
            ClearGarbage();
        }

        private void button_excludeSlices_Click(object sender, EventArgs e)
        {
            if (dicomMatrices == null)
            {
                MessageBox.Show("DICOM files are not loaded", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numericUpDown_excludeSlicesFromBegin.Value < 0 || numericUpDown_excludeSlicesFromBegin.Value > dicomMatrices.Length)
            {
                MessageBox.Show("Value 'From begin' is invalid", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numericUpDown_excludeSlicesFromEnd.Value < 0 || numericUpDown_excludeSlicesFromEnd.Value > dicomMatrices.Length)
            {
                MessageBox.Show("Value 'From end' is invalid", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int fromBegin = (int)numericUpDown_excludeSlicesFromBegin.Value;
            int fromEnd = (int)numericUpDown_excludeSlicesFromEnd.Value;

            // Clear all GUI data
            pictureBox_DICOMImage.Image = null;
            pictureBox_segmentedImage.Image = null;
            trackBar.Maximum = 0;
            trackBar.Value = 0;
            toolStripStatusLabel.Text = " ";
            label_trackBarValue.Text = " ";

            // Clear segmentation data
            ClearSegmentationData();

            // Garbage Collector force call
            ClearGarbage();

            // if all sclices are excluding
            if ((fromBegin + fromEnd) >= dicomMatrices.Length)
            {
                ClearDICOMData(); // clear DICOM data

                // Garbage Collector force call
                ClearGarbage();
                return;
            }

            int newLength = dicomMatrices.Length - (fromBegin + fromEnd);

            Array[] subMatrices = new Array[newLength];
            Array.Copy(dicomMatrices, fromBegin, subMatrices, 0, newLength);
            dicomMatrices = subMatrices;

            string[] newFilenames = new string[newLength];
            Array.Copy(filenames, fromBegin, newFilenames, 0, newLength);
            filenames = newFilenames;

            // Garbage Collector force call
            ClearGarbage();

            // fill GUI elements
            int defaultIndex = 0;
            Bitmap DICOMImage = ImageProcessing.GetBitmapFrom16Matrix(dicomMatrices[defaultIndex], imageMatrixHeight, imageMatrixWidth,
                eWindow.MinBorder.DICOMUnit, eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
            pictureBox_DICOMImage.Image = DICOMImage;

            trackBar.Maximum = dicomMatrices.Length - 1;
            trackBar.Value = defaultIndex;
            toolStripStatusLabel.Text = string.Format("DICOM files recreation: {0} files loaded", dicomMatrices.Length);
            label_trackBarValue.Text = defaultIndex.ToString();
        }

        private void pictureBox_segmentedImage_DoubleClick(object sender, EventArgs e)
        {
            PictureBox pBox = (PictureBox)sender;
            if (sender == null)
                return;

            if (ctRegions == null || pBox.Image == null)
                return;

            // getting image for current electronic window
            byte[,] intencityInput = new byte[imageMatrixHeight, imageMatrixWidth];

            short minBorder = eWindow.MinBorder.DICOMUnit;
            short maxBorder = eWindow.MaxBorder.DICOMUnit;
            short minIntencity = eWindow.MinLevel.DICOMUnit;
            short maxIntencity = eWindow.MaxLevel.DICOMUnit;
            if (minIntencity < minBorder)
                minIntencity = minBorder;
            if (maxIntencity > maxBorder)
                maxIntencity = maxBorder;

            for (int i = 0; i < imageMatrixHeight; i++)
            {
                for (int j = 0; j < imageMatrixWidth; j++)
                {
                    short valueFromArray = (short)dicomMatrices[trackBar.Value].GetValue(i, j);

                    byte valueForImage = 0;
                    if (valueFromArray >= minBorder && valueFromArray <= maxBorder)
                    {
                        // if value is outside of the electronic window
                        if (valueFromArray < minIntencity)
                            valueFromArray = minIntencity;
                        if (valueFromArray > maxIntencity)
                            valueFromArray = maxIntencity;

                        valueForImage = (byte)(((valueFromArray - minIntencity) / (float)(maxIntencity - minIntencity)) * 255);
                    }

                    intencityInput[i, j] = valueForImage;
                }
            }

            MWCharArray matlabFilename = new MWCharArray(new string(filenames[trackBar.Value].ToCharArray())); // creating a MATLAB char array
            var matlabSpacingMatrix = reader.get_spacing(matlabFilename); // call MATLAB function

            var detailedForm = SegmentsDetails.GetInstance(intencityInput, imageMatrixHeight, imageMatrixWidth,
                matlabSpacingMatrix.ToArray(), filterWidth, intencityThreshold);
            detailedForm.Show();
        }

        private void button_calculateSegmentsSizes_Click(object sender, EventArgs e)
        {
            if (ctRegions == null)
                return;

            MWCharArray matlabFilename = new MWCharArray(new string(filenames[trackBar.Value].ToCharArray())); // creating a MATLAB char array
            var matlabSpacingMatrix = reader.get_spacing(matlabFilename); // call MATLAB function

            var segmentsSizesForm = SegmentsSizes.GetInstance(ctRegions, colorFactory, segmentsDencity, ctRegions.Length,
                imageMatrixHeight, imageMatrixWidth, matlabSpacingMatrix.ToArray(), trackBar.Value);
            segmentsSizesForm.Show();
        }
    }
}
