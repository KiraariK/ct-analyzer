using System;
using System.Drawing;
using System.Windows.Forms;

using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

using DICOMWorker;
using System.Collections.Generic;

namespace DICOMopener
{
    public partial class MainForm : Form
    {
        // Default min and max intencities for all CT-images
        //private static short globalMinIntencity = 0;
        //private short globalMaxIntencity = 3607;

        private string[] filenames;
        private Array[] dicomMatrices;
        private int imageMatrixHeight;
        private int imageMatrixWidth;
        private DICOMReader reader; // defining the MATLAB DICOMReader Class

        private EWindow eWindow; // defining EWindow parameters

        public MainForm()
        {
            InitializeComponent();
            pictureBox_DICOMImage.Image = null;
            trackBar.Maximum = 0;
            trackBar.Value = 0;
            toolStripStatusLabel.Text = " ";
            label_trackBarValue.Text = " ";

            filenames = null;
            dicomMatrices = null;

            reader = new DICOMReader(); // creating the instance of MATLAB DICOMReader Class. It is important to create instance here for 32 bit.

            // EWindow parameters
            eWindow = new EWindow(EWindowType.EWType.Wide);

            comboBox_EWindowType.Items.Clear();
            foreach (KeyValuePair<EWindowType.EWType, string> it in eWindow.MinLevel.EWTypeLabels)
                comboBox_EWindowType.Items.Add(it.Value);

            FillEWindowParametersView();
        }

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

        private void CurrentImageReloading()
        {
            if (dicomMatrices == null) // DICOM files are not loaded
                return;

            int currentIndex = trackBar.Value;
            Bitmap DICOMImage = ImageProcessing.GetBitmapFrom16Matrix(dicomMatrices[currentIndex], imageMatrixHeight, imageMatrixWidth,
                eWindow.MinBorder.DICOMUnit, eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
            pictureBox_DICOMImage.Image = DICOMImage;
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            pictureBox_DICOMImage.Image = null;
            trackBar.Maximum = 0;
            trackBar.Value = 0;
            toolStripStatusLabel.Text = " ";
            label_trackBarValue.Text = " ";

            filenames = null;
            dicomMatrices = null;

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

            // logging values from certain sclice
            //Logger logger = new Logger("../../Log/log_fibroso_cavernous_139_slice52.txt");
            //List<short> uniqueValues = new List<short>();
            //for (int i = 0; i < imageMatrixHeight; i++)
            //{
            //    for (int j = 0; j < imageMatrixWidth; j++)
            //    {
            //        short currentValue = (short)dicomMatrices[52].GetValue(i, j);
            //        if (!uniqueValues.Contains(currentValue))
            //        {
            //            logger.WriteLog(currentValue.ToString() + "\r\n");
            //            uniqueValues.Add(currentValue);
            //        }
            //    }
            //}

            // Creating default image from data
            int defaultIndex = 0;
            Bitmap DICOMImage = ImageProcessing.GetBitmapFrom16Matrix(dicomMatrices[defaultIndex], imageMatrixHeight, imageMatrixWidth,
                eWindow.MinBorder.DICOMUnit, eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
            pictureBox_DICOMImage.Image = DICOMImage;

            trackBar.Maximum = filenames.Length - 1;
            trackBar.Value = defaultIndex;
            toolStripStatusLabel.Text = string.Format("Loaded {0} files in {1}",
                filenames.Length, loadFilesTime);
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
            Bitmap DICOMImage = ImageProcessing.GetBitmapFrom16Matrix(dicomMatrices[currentIndex], imageMatrixHeight, imageMatrixWidth,
                eWindow.MinBorder.DICOMUnit, eWindow.MaxBorder.DICOMUnit, eWindow.MinLevel.DICOMUnit, eWindow.MaxLevel.DICOMUnit);
            pictureBox_DICOMImage.Image = DICOMImage;

            label_trackBarValue.Text = currentIndex.ToString();
        }

        private void button_saveImage_Click(object sender, EventArgs e)
        {
            if (pictureBox_DICOMImage.Image == null)
            {
                MessageBox.Show("Нечего сохранять", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            saveFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox_DICOMImage.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }

        private void button_saveAllImages_Click(object sender, EventArgs e)
        {
            if (pictureBox_DICOMImage.Image == null)
            {
                MessageBox.Show("Нечего сохранять", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        switch(strIndex.Length)
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
            CurrentImageReloading();
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
            CurrentImageReloading();
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
    }
}
