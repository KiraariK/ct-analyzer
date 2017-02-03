namespace DICOMopener
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_open = new System.Windows.Forms.Button();
            this.pictureBox_DICOMImage = new System.Windows.Forms.PictureBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label_trackBarValue = new System.Windows.Forms.Label();
            this.button_saveImage = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.button_saveAllImages = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel_EWindowParameters = new System.Windows.Forms.Panel();
            this.panel_CenterWidth = new System.Windows.Forms.Panel();
            this.label_EWindowCenter = new System.Windows.Forms.Label();
            this.label_recomendedEWindowWidth = new System.Windows.Forms.Label();
            this.textBox_EWindowCenter = new System.Windows.Forms.TextBox();
            this.label_recomendedEWindowCenter = new System.Windows.Forms.Label();
            this.textBox_EWindowWidth = new System.Windows.Forms.TextBox();
            this.button_EWindowParametersApply = new System.Windows.Forms.Button();
            this.label_EWindowWidth = new System.Windows.Forms.Label();
            this.radioButton_Hounsfield = new System.Windows.Forms.RadioButton();
            this.radioButton_DICOM = new System.Windows.Forms.RadioButton();
            this.label_units = new System.Windows.Forms.Label();
            this.comboBox_EWindowType = new System.Windows.Forms.ComboBox();
            this.label_EWindowType = new System.Windows.Forms.Label();
            this.pictureBox_segmentedImage = new System.Windows.Forms.PictureBox();
            this.button_saveSegmentedImage = new System.Windows.Forms.Button();
            this.button_saveAllSegmentedImages = new System.Windows.Forms.Button();
            this.panel_segmentationParameters = new System.Windows.Forms.Panel();
            this.button_doSegmentation = new System.Windows.Forms.Button();
            this.numericUpDown_segmentationIntencityThreshold = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_segmentationFilterWidth = new System.Windows.Forms.NumericUpDown();
            this.label_segmentationIntencityThreshold = new System.Windows.Forms.Label();
            this.label_segmentationFilterWidth = new System.Windows.Forms.Label();
            this.button_close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DICOMImage)).BeginInit();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.panel_EWindowParameters.SuspendLayout();
            this.panel_CenterWidth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_segmentedImage)).BeginInit();
            this.panel_segmentationParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_segmentationIntencityThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_segmentationFilterWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // button_open
            // 
            this.button_open.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_open.Location = new System.Drawing.Point(13, 11);
            this.button_open.Margin = new System.Windows.Forms.Padding(4);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(231, 43);
            this.button_open.TabIndex = 0;
            this.button_open.Text = "open DICOM files";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // pictureBox_DICOMImage
            // 
            this.pictureBox_DICOMImage.Location = new System.Drawing.Point(419, 61);
            this.pictureBox_DICOMImage.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_DICOMImage.Name = "pictureBox_DICOMImage";
            this.pictureBox_DICOMImage.Size = new System.Drawing.Size(440, 440);
            this.pictureBox_DICOMImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_DICOMImage.TabIndex = 1;
            this.pictureBox_DICOMImage.TabStop = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 657);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(1549, 25);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(13, 20);
            this.toolStripStatusLabel.Text = " ";
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(16, 559);
            this.trackBar.Margin = new System.Windows.Forms.Padding(4);
            this.trackBar.Maximum = 0;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(1503, 56);
            this.trackBar.TabIndex = 3;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // label_trackBarValue
            // 
            this.label_trackBarValue.AutoSize = true;
            this.label_trackBarValue.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_trackBarValue.Location = new System.Drawing.Point(774, 624);
            this.label_trackBarValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_trackBarValue.Name = "label_trackBarValue";
            this.label_trackBarValue.Size = new System.Drawing.Size(13, 20);
            this.label_trackBarValue.TabIndex = 4;
            this.label_trackBarValue.Text = " ";
            // 
            // button_saveImage
            // 
            this.button_saveImage.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_saveImage.Location = new System.Drawing.Point(419, 509);
            this.button_saveImage.Margin = new System.Windows.Forms.Padding(4);
            this.button_saveImage.Name = "button_saveImage";
            this.button_saveImage.Size = new System.Drawing.Size(144, 44);
            this.button_saveImage.TabIndex = 5;
            this.button_saveImage.Text = "Save image";
            this.button_saveImage.UseVisualStyleBackColor = true;
            this.button_saveImage.Click += new System.EventHandler(this.button_saveImage_Click);
            // 
            // button_saveAllImages
            // 
            this.button_saveAllImages.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_saveAllImages.Location = new System.Drawing.Point(571, 509);
            this.button_saveAllImages.Margin = new System.Windows.Forms.Padding(4);
            this.button_saveAllImages.Name = "button_saveAllImages";
            this.button_saveAllImages.Size = new System.Drawing.Size(184, 44);
            this.button_saveAllImages.TabIndex = 6;
            this.button_saveAllImages.Text = "Save all images";
            this.button_saveAllImages.UseVisualStyleBackColor = true;
            this.button_saveAllImages.Click += new System.EventHandler(this.button_saveAllImages_Click);
            // 
            // panel_EWindowParameters
            // 
            this.panel_EWindowParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_EWindowParameters.Controls.Add(this.panel_CenterWidth);
            this.panel_EWindowParameters.Controls.Add(this.radioButton_Hounsfield);
            this.panel_EWindowParameters.Controls.Add(this.radioButton_DICOM);
            this.panel_EWindowParameters.Controls.Add(this.label_units);
            this.panel_EWindowParameters.Controls.Add(this.comboBox_EWindowType);
            this.panel_EWindowParameters.Controls.Add(this.label_EWindowType);
            this.panel_EWindowParameters.Location = new System.Drawing.Point(12, 61);
            this.panel_EWindowParameters.Name = "panel_EWindowParameters";
            this.panel_EWindowParameters.Size = new System.Drawing.Size(400, 289);
            this.panel_EWindowParameters.TabIndex = 7;
            // 
            // panel_CenterWidth
            // 
            this.panel_CenterWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_CenterWidth.Controls.Add(this.label_EWindowCenter);
            this.panel_CenterWidth.Controls.Add(this.label_recomendedEWindowWidth);
            this.panel_CenterWidth.Controls.Add(this.textBox_EWindowCenter);
            this.panel_CenterWidth.Controls.Add(this.label_recomendedEWindowCenter);
            this.panel_CenterWidth.Controls.Add(this.textBox_EWindowWidth);
            this.panel_CenterWidth.Controls.Add(this.button_EWindowParametersApply);
            this.panel_CenterWidth.Controls.Add(this.label_EWindowWidth);
            this.panel_CenterWidth.Location = new System.Drawing.Point(10, 104);
            this.panel_CenterWidth.Name = "panel_CenterWidth";
            this.panel_CenterWidth.Size = new System.Drawing.Size(378, 170);
            this.panel_CenterWidth.TabIndex = 8;
            // 
            // label_EWindowCenter
            // 
            this.label_EWindowCenter.AutoSize = true;
            this.label_EWindowCenter.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_EWindowCenter.Location = new System.Drawing.Point(3, 13);
            this.label_EWindowCenter.Name = "label_EWindowCenter";
            this.label_EWindowCenter.Size = new System.Drawing.Size(129, 20);
            this.label_EWindowCenter.TabIndex = 5;
            this.label_EWindowCenter.Text = "Window center:";
            // 
            // label_recomendedEWindowWidth
            // 
            this.label_recomendedEWindowWidth.AutoSize = true;
            this.label_recomendedEWindowWidth.Location = new System.Drawing.Point(156, 90);
            this.label_recomendedEWindowWidth.Name = "label_recomendedEWindowWidth";
            this.label_recomendedEWindowWidth.Size = new System.Drawing.Size(216, 17);
            this.label_recomendedEWindowWidth.TabIndex = 11;
            this.label_recomendedEWindowWidth.Text = "Recomended width:   1000..2000";
            // 
            // textBox_EWindowCenter
            // 
            this.textBox_EWindowCenter.Location = new System.Drawing.Point(218, 11);
            this.textBox_EWindowCenter.Name = "textBox_EWindowCenter";
            this.textBox_EWindowCenter.Size = new System.Drawing.Size(120, 22);
            this.textBox_EWindowCenter.TabIndex = 6;
            // 
            // label_recomendedEWindowCenter
            // 
            this.label_recomendedEWindowCenter.AutoSize = true;
            this.label_recomendedEWindowCenter.Location = new System.Drawing.Point(156, 36);
            this.label_recomendedEWindowCenter.Name = "label_recomendedEWindowCenter";
            this.label_recomendedEWindowCenter.Size = new System.Drawing.Size(216, 17);
            this.label_recomendedEWindowCenter.TabIndex = 10;
            this.label_recomendedEWindowCenter.Text = "Recomended center: 1024..2064";
            // 
            // textBox_EWindowWidth
            // 
            this.textBox_EWindowWidth.Location = new System.Drawing.Point(218, 65);
            this.textBox_EWindowWidth.Name = "textBox_EWindowWidth";
            this.textBox_EWindowWidth.Size = new System.Drawing.Size(120, 22);
            this.textBox_EWindowWidth.TabIndex = 7;
            // 
            // button_EWindowParametersApply
            // 
            this.button_EWindowParametersApply.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_EWindowParametersApply.Location = new System.Drawing.Point(6, 118);
            this.button_EWindowParametersApply.Name = "button_EWindowParametersApply";
            this.button_EWindowParametersApply.Size = new System.Drawing.Size(106, 36);
            this.button_EWindowParametersApply.TabIndex = 9;
            this.button_EWindowParametersApply.Text = "Apply";
            this.button_EWindowParametersApply.UseVisualStyleBackColor = true;
            this.button_EWindowParametersApply.Click += new System.EventHandler(this.button_EWindowParametersApply_Click);
            // 
            // label_EWindowWidth
            // 
            this.label_EWindowWidth.AutoSize = true;
            this.label_EWindowWidth.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_EWindowWidth.Location = new System.Drawing.Point(4, 67);
            this.label_EWindowWidth.Name = "label_EWindowWidth";
            this.label_EWindowWidth.Size = new System.Drawing.Size(126, 20);
            this.label_EWindowWidth.TabIndex = 8;
            this.label_EWindowWidth.Text = "Window width:";
            // 
            // radioButton_Hounsfield
            // 
            this.radioButton_Hounsfield.AutoSize = true;
            this.radioButton_Hounsfield.Location = new System.Drawing.Point(251, 56);
            this.radioButton_Hounsfield.Name = "radioButton_Hounsfield";
            this.radioButton_Hounsfield.Size = new System.Drawing.Size(130, 21);
            this.radioButton_Hounsfield.TabIndex = 4;
            this.radioButton_Hounsfield.TabStop = true;
            this.radioButton_Hounsfield.Text = "Hounsfield units";
            this.radioButton_Hounsfield.UseVisualStyleBackColor = true;
            this.radioButton_Hounsfield.CheckedChanged += new System.EventHandler(this.radioButton_Hounsfield_CheckedChanged);
            // 
            // radioButton_DICOM
            // 
            this.radioButton_DICOM.AutoSize = true;
            this.radioButton_DICOM.Location = new System.Drawing.Point(109, 56);
            this.radioButton_DICOM.Name = "radioButton_DICOM";
            this.radioButton_DICOM.Size = new System.Drawing.Size(107, 21);
            this.radioButton_DICOM.TabIndex = 3;
            this.radioButton_DICOM.TabStop = true;
            this.radioButton_DICOM.Text = "DICOM units";
            this.radioButton_DICOM.UseVisualStyleBackColor = true;
            this.radioButton_DICOM.CheckedChanged += new System.EventHandler(this.radioButton_DICOM_CheckedChanged);
            // 
            // label_units
            // 
            this.label_units.AutoSize = true;
            this.label_units.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_units.Location = new System.Drawing.Point(13, 57);
            this.label_units.Name = "label_units";
            this.label_units.Size = new System.Drawing.Size(55, 20);
            this.label_units.TabIndex = 2;
            this.label_units.Text = "Units:";
            // 
            // comboBox_EWindowType
            // 
            this.comboBox_EWindowType.FormattingEnabled = true;
            this.comboBox_EWindowType.Location = new System.Drawing.Point(229, 14);
            this.comboBox_EWindowType.Name = "comboBox_EWindowType";
            this.comboBox_EWindowType.Size = new System.Drawing.Size(120, 24);
            this.comboBox_EWindowType.TabIndex = 1;
            this.comboBox_EWindowType.SelectedIndexChanged += new System.EventHandler(this.comboBox_EWindowType_SelectedIndexChanged);
            // 
            // label_EWindowType
            // 
            this.label_EWindowType.AutoSize = true;
            this.label_EWindowType.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_EWindowType.Location = new System.Drawing.Point(12, 14);
            this.label_EWindowType.Name = "label_EWindowType";
            this.label_EWindowType.Size = new System.Drawing.Size(191, 20);
            this.label_EWindowType.TabIndex = 0;
            this.label_EWindowType.Text = "Electronic window type:";
            // 
            // pictureBox_segmentedImage
            // 
            this.pictureBox_segmentedImage.Location = new System.Drawing.Point(1079, 61);
            this.pictureBox_segmentedImage.Name = "pictureBox_segmentedImage";
            this.pictureBox_segmentedImage.Size = new System.Drawing.Size(440, 440);
            this.pictureBox_segmentedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_segmentedImage.TabIndex = 8;
            this.pictureBox_segmentedImage.TabStop = false;
            // 
            // button_saveSegmentedImage
            // 
            this.button_saveSegmentedImage.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_saveSegmentedImage.Location = new System.Drawing.Point(1375, 507);
            this.button_saveSegmentedImage.Name = "button_saveSegmentedImage";
            this.button_saveSegmentedImage.Size = new System.Drawing.Size(144, 44);
            this.button_saveSegmentedImage.TabIndex = 9;
            this.button_saveSegmentedImage.Text = "Save image";
            this.button_saveSegmentedImage.UseVisualStyleBackColor = true;
            this.button_saveSegmentedImage.Click += new System.EventHandler(this.button_saveSegmentedImage_Click);
            // 
            // button_saveAllSegmentedImages
            // 
            this.button_saveAllSegmentedImages.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_saveAllSegmentedImages.Location = new System.Drawing.Point(1185, 507);
            this.button_saveAllSegmentedImages.Name = "button_saveAllSegmentedImages";
            this.button_saveAllSegmentedImages.Size = new System.Drawing.Size(184, 44);
            this.button_saveAllSegmentedImages.TabIndex = 10;
            this.button_saveAllSegmentedImages.Text = "Save all images";
            this.button_saveAllSegmentedImages.UseVisualStyleBackColor = true;
            this.button_saveAllSegmentedImages.Click += new System.EventHandler(this.button_saveAllSegmentedImages_Click);
            // 
            // panel_segmentationParameters
            // 
            this.panel_segmentationParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_segmentationParameters.Controls.Add(this.button_doSegmentation);
            this.panel_segmentationParameters.Controls.Add(this.numericUpDown_segmentationIntencityThreshold);
            this.panel_segmentationParameters.Controls.Add(this.numericUpDown_segmentationFilterWidth);
            this.panel_segmentationParameters.Controls.Add(this.label_segmentationIntencityThreshold);
            this.panel_segmentationParameters.Controls.Add(this.label_segmentationFilterWidth);
            this.panel_segmentationParameters.Location = new System.Drawing.Point(865, 61);
            this.panel_segmentationParameters.Name = "panel_segmentationParameters";
            this.panel_segmentationParameters.Size = new System.Drawing.Size(208, 440);
            this.panel_segmentationParameters.TabIndex = 11;
            // 
            // button_doSegmentation
            // 
            this.button_doSegmentation.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_doSegmentation.Location = new System.Drawing.Point(11, 273);
            this.button_doSegmentation.Name = "button_doSegmentation";
            this.button_doSegmentation.Size = new System.Drawing.Size(183, 44);
            this.button_doSegmentation.TabIndex = 4;
            this.button_doSegmentation.Text = "Segmentation";
            this.button_doSegmentation.UseVisualStyleBackColor = true;
            this.button_doSegmentation.Click += new System.EventHandler(this.button_doSegmentation_Click);
            // 
            // numericUpDown_segmentationIntencityThreshold
            // 
            this.numericUpDown_segmentationIntencityThreshold.Location = new System.Drawing.Point(11, 219);
            this.numericUpDown_segmentationIntencityThreshold.Name = "numericUpDown_segmentationIntencityThreshold";
            this.numericUpDown_segmentationIntencityThreshold.Size = new System.Drawing.Size(183, 22);
            this.numericUpDown_segmentationIntencityThreshold.TabIndex = 3;
            this.numericUpDown_segmentationIntencityThreshold.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // numericUpDown_segmentationFilterWidth
            // 
            this.numericUpDown_segmentationFilterWidth.Location = new System.Drawing.Point(11, 139);
            this.numericUpDown_segmentationFilterWidth.Name = "numericUpDown_segmentationFilterWidth";
            this.numericUpDown_segmentationFilterWidth.Size = new System.Drawing.Size(183, 22);
            this.numericUpDown_segmentationFilterWidth.TabIndex = 2;
            this.numericUpDown_segmentationFilterWidth.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // label_segmentationIntencityThreshold
            // 
            this.label_segmentationIntencityThreshold.AutoSize = true;
            this.label_segmentationIntencityThreshold.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_segmentationIntencityThreshold.Location = new System.Drawing.Point(7, 187);
            this.label_segmentationIntencityThreshold.Name = "label_segmentationIntencityThreshold";
            this.label_segmentationIntencityThreshold.Size = new System.Drawing.Size(153, 20);
            this.label_segmentationIntencityThreshold.TabIndex = 1;
            this.label_segmentationIntencityThreshold.Text = "Intencity threshold";
            // 
            // label_segmentationFilterWidth
            // 
            this.label_segmentationFilterWidth.AutoSize = true;
            this.label_segmentationFilterWidth.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_segmentationFilterWidth.Location = new System.Drawing.Point(7, 106);
            this.label_segmentationFilterWidth.Name = "label_segmentationFilterWidth";
            this.label_segmentationFilterWidth.Size = new System.Drawing.Size(98, 20);
            this.label_segmentationFilterWidth.TabIndex = 0;
            this.label_segmentationFilterWidth.Text = "Filter width";
            // 
            // button_close
            // 
            this.button_close.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_close.Location = new System.Drawing.Point(1444, 12);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 43);
            this.button_close.TabIndex = 12;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1549, 682);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.panel_segmentationParameters);
            this.Controls.Add(this.button_saveAllSegmentedImages);
            this.Controls.Add(this.button_saveSegmentedImage);
            this.Controls.Add(this.pictureBox_segmentedImage);
            this.Controls.Add(this.panel_EWindowParameters);
            this.Controls.Add(this.button_saveAllImages);
            this.Controls.Add(this.button_saveImage);
            this.Controls.Add(this.label_trackBarValue);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pictureBox_DICOMImage);
            this.Controls.Add(this.button_open);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DICOMViewer";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DICOMImage)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.panel_EWindowParameters.ResumeLayout(false);
            this.panel_EWindowParameters.PerformLayout();
            this.panel_CenterWidth.ResumeLayout(false);
            this.panel_CenterWidth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_segmentedImage)).EndInit();
            this.panel_segmentationParameters.ResumeLayout(false);
            this.panel_segmentationParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_segmentationIntencityThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_segmentationFilterWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.PictureBox pictureBox_DICOMImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label_trackBarValue;
        private System.Windows.Forms.Button button_saveImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button button_saveAllImages;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Panel panel_EWindowParameters;
        private System.Windows.Forms.ComboBox comboBox_EWindowType;
        private System.Windows.Forms.Label label_EWindowType;
        private System.Windows.Forms.Label label_units;
        private System.Windows.Forms.RadioButton radioButton_DICOM;
        private System.Windows.Forms.RadioButton radioButton_Hounsfield;
        private System.Windows.Forms.Label label_EWindowCenter;
        private System.Windows.Forms.TextBox textBox_EWindowCenter;
        private System.Windows.Forms.Label label_EWindowWidth;
        private System.Windows.Forms.TextBox textBox_EWindowWidth;
        private System.Windows.Forms.Button button_EWindowParametersApply;
        private System.Windows.Forms.Label label_recomendedEWindowCenter;
        private System.Windows.Forms.Label label_recomendedEWindowWidth;
        private System.Windows.Forms.Panel panel_CenterWidth;
        private System.Windows.Forms.PictureBox pictureBox_segmentedImage;
        private System.Windows.Forms.Button button_saveSegmentedImage;
        private System.Windows.Forms.Button button_saveAllSegmentedImages;
        private System.Windows.Forms.Panel panel_segmentationParameters;
        private System.Windows.Forms.Label label_segmentationIntencityThreshold;
        private System.Windows.Forms.Label label_segmentationFilterWidth;
        private System.Windows.Forms.NumericUpDown numericUpDown_segmentationFilterWidth;
        private System.Windows.Forms.NumericUpDown numericUpDown_segmentationIntencityThreshold;
        private System.Windows.Forms.Button button_doSegmentation;
        private System.Windows.Forms.Button button_close;
    }
}

