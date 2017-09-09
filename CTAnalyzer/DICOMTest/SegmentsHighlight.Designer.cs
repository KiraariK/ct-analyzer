namespace DICOMopener
{
    partial class SegmentsHighlight
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox_DICOMImage = new System.Windows.Forms.PictureBox();
            this.pictureBox_segmentedIMage = new System.Windows.Forms.PictureBox();
            this.listBox_segmentIndices = new System.Windows.Forms.ListBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label_trackBarValue = new System.Windows.Forms.Label();
            this.button_closeForm = new System.Windows.Forms.Button();
            this.button_saveImage = new System.Windows.Forms.Button();
            this.button_saveAllImages = new System.Windows.Forms.Button();
            this.button_saveSegmentedImage = new System.Windows.Forms.Button();
            this.button_saveAllSegmentedImages = new System.Windows.Forms.Button();
            this.label_segmentIndices = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DICOMImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_segmentedIMage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_DICOMImage
            // 
            this.pictureBox_DICOMImage.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_DICOMImage.Name = "pictureBox_DICOMImage";
            this.pictureBox_DICOMImage.Size = new System.Drawing.Size(440, 440);
            this.pictureBox_DICOMImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_DICOMImage.TabIndex = 0;
            this.pictureBox_DICOMImage.TabStop = false;
            // 
            // pictureBox_segmentedIMage
            // 
            this.pictureBox_segmentedIMage.Location = new System.Drawing.Point(721, 12);
            this.pictureBox_segmentedIMage.Name = "pictureBox_segmentedIMage";
            this.pictureBox_segmentedIMage.Size = new System.Drawing.Size(440, 440);
            this.pictureBox_segmentedIMage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_segmentedIMage.TabIndex = 1;
            this.pictureBox_segmentedIMage.TabStop = false;
            // 
            // listBox_segmentIndices
            // 
            this.listBox_segmentIndices.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox_segmentIndices.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox_segmentIndices.FormattingEnabled = true;
            this.listBox_segmentIndices.ItemHeight = 18;
            this.listBox_segmentIndices.Location = new System.Drawing.Point(470, 52);
            this.listBox_segmentIndices.Name = "listBox_segmentIndices";
            this.listBox_segmentIndices.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_segmentIndices.Size = new System.Drawing.Size(232, 364);
            this.listBox_segmentIndices.TabIndex = 2;
            this.listBox_segmentIndices.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_segmentIndices_DrawItem);
            this.listBox_segmentIndices.SelectedIndexChanged += new System.EventHandler(this.listBox_segmentIndices_SelectedIndexChanged);
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(12, 522);
            this.trackBar.Maximum = 0;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(1156, 56);
            this.trackBar.TabIndex = 3;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // label_trackBarValue
            // 
            this.label_trackBarValue.AutoSize = true;
            this.label_trackBarValue.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_trackBarValue.Location = new System.Drawing.Point(582, 592);
            this.label_trackBarValue.Name = "label_trackBarValue";
            this.label_trackBarValue.Size = new System.Drawing.Size(13, 20);
            this.label_trackBarValue.TabIndex = 4;
            this.label_trackBarValue.Text = " ";
            // 
            // button_closeForm
            // 
            this.button_closeForm.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_closeForm.Location = new System.Drawing.Point(553, 653);
            this.button_closeForm.Name = "button_closeForm";
            this.button_closeForm.Size = new System.Drawing.Size(75, 44);
            this.button_closeForm.TabIndex = 5;
            this.button_closeForm.Text = "Close";
            this.button_closeForm.UseVisualStyleBackColor = true;
            this.button_closeForm.Click += new System.EventHandler(this.button_closeForm_Click);
            // 
            // button_saveImage
            // 
            this.button_saveImage.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_saveImage.Location = new System.Drawing.Point(12, 458);
            this.button_saveImage.Name = "button_saveImage";
            this.button_saveImage.Size = new System.Drawing.Size(120, 44);
            this.button_saveImage.TabIndex = 6;
            this.button_saveImage.Text = "Save image";
            this.button_saveImage.UseVisualStyleBackColor = true;
            // 
            // button_saveAllImages
            // 
            this.button_saveAllImages.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_saveAllImages.Location = new System.Drawing.Point(138, 458);
            this.button_saveAllImages.Name = "button_saveAllImages";
            this.button_saveAllImages.Size = new System.Drawing.Size(152, 44);
            this.button_saveAllImages.TabIndex = 7;
            this.button_saveAllImages.Text = "Save all images";
            this.button_saveAllImages.UseVisualStyleBackColor = true;
            // 
            // button_saveSegmentedImage
            // 
            this.button_saveSegmentedImage.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_saveSegmentedImage.Location = new System.Drawing.Point(1043, 458);
            this.button_saveSegmentedImage.Name = "button_saveSegmentedImage";
            this.button_saveSegmentedImage.Size = new System.Drawing.Size(118, 44);
            this.button_saveSegmentedImage.TabIndex = 8;
            this.button_saveSegmentedImage.Text = "Save image";
            this.button_saveSegmentedImage.UseVisualStyleBackColor = true;
            // 
            // button_saveAllSegmentedImages
            // 
            this.button_saveAllSegmentedImages.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_saveAllSegmentedImages.Location = new System.Drawing.Point(885, 458);
            this.button_saveAllSegmentedImages.Name = "button_saveAllSegmentedImages";
            this.button_saveAllSegmentedImages.Size = new System.Drawing.Size(152, 44);
            this.button_saveAllSegmentedImages.TabIndex = 9;
            this.button_saveAllSegmentedImages.Text = "Save all images";
            this.button_saveAllSegmentedImages.UseVisualStyleBackColor = true;
            // 
            // label_segmentIndices
            // 
            this.label_segmentIndices.AutoSize = true;
            this.label_segmentIndices.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_segmentIndices.Location = new System.Drawing.Point(466, 29);
            this.label_segmentIndices.Name = "label_segmentIndices";
            this.label_segmentIndices.Size = new System.Drawing.Size(137, 20);
            this.label_segmentIndices.TabIndex = 10;
            this.label_segmentIndices.Text = "Segment indices:";
            // 
            // SegmentsHighlight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1177, 712);
            this.Controls.Add(this.label_segmentIndices);
            this.Controls.Add(this.button_saveAllSegmentedImages);
            this.Controls.Add(this.button_saveSegmentedImage);
            this.Controls.Add(this.button_saveAllImages);
            this.Controls.Add(this.button_saveImage);
            this.Controls.Add(this.button_closeForm);
            this.Controls.Add(this.label_trackBarValue);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.listBox_segmentIndices);
            this.Controls.Add(this.pictureBox_segmentedIMage);
            this.Controls.Add(this.pictureBox_DICOMImage);
            this.Name = "SegmentsHighlight";
            this.Text = "SegmentsHighlight";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DICOMImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_segmentedIMage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_DICOMImage;
        private System.Windows.Forms.PictureBox pictureBox_segmentedIMage;
        private System.Windows.Forms.ListBox listBox_segmentIndices;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label_trackBarValue;
        private System.Windows.Forms.Button button_closeForm;
        private System.Windows.Forms.Button button_saveImage;
        private System.Windows.Forms.Button button_saveAllImages;
        private System.Windows.Forms.Button button_saveSegmentedImage;
        private System.Windows.Forms.Button button_saveAllSegmentedImages;
        private System.Windows.Forms.Label label_segmentIndices;
    }
}