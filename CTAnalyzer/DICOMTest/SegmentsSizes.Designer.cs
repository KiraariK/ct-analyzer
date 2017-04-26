namespace DICOMopener
{
    partial class SegmentsSizes
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
            this.pictureBox_segmentedImage = new System.Windows.Forms.PictureBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label_trackBarValue = new System.Windows.Forms.Label();
            this.listBox_segments = new System.Windows.Forms.ListBox();
            this.listBox_sizes = new System.Windows.Forms.ListBox();
            this.label_segments = new System.Windows.Forms.Label();
            this.label_segmentsSizes = new System.Windows.Forms.Label();
            this.button_closeForm = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_segmentedImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_segmentedImage
            // 
            this.pictureBox_segmentedImage.Location = new System.Drawing.Point(219, 12);
            this.pictureBox_segmentedImage.Name = "pictureBox_segmentedImage";
            this.pictureBox_segmentedImage.Size = new System.Drawing.Size(440, 440);
            this.pictureBox_segmentedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_segmentedImage.TabIndex = 0;
            this.pictureBox_segmentedImage.TabStop = false;
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(12, 458);
            this.trackBar.Maximum = 0;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(1240, 56);
            this.trackBar.TabIndex = 1;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // label_trackBarValue
            // 
            this.label_trackBarValue.AutoSize = true;
            this.label_trackBarValue.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_trackBarValue.Location = new System.Drawing.Point(600, 531);
            this.label_trackBarValue.Name = "label_trackBarValue";
            this.label_trackBarValue.Size = new System.Drawing.Size(13, 20);
            this.label_trackBarValue.TabIndex = 2;
            this.label_trackBarValue.Text = " ";
            // 
            // listBox_segments
            // 
            this.listBox_segments.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox_segments.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox_segments.FormattingEnabled = true;
            this.listBox_segments.ItemHeight = 18;
            this.listBox_segments.Location = new System.Drawing.Point(675, 32);
            this.listBox_segments.Name = "listBox_segments";
            this.listBox_segments.Size = new System.Drawing.Size(172, 418);
            this.listBox_segments.TabIndex = 3;
            this.listBox_segments.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_segments_DrawItem);
            this.listBox_segments.SelectedIndexChanged += new System.EventHandler(this.listBox_segments_SelectedIndexChanged);
            // 
            // listBox_sizes
            // 
            this.listBox_sizes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox_sizes.FormattingEnabled = true;
            this.listBox_sizes.ItemHeight = 20;
            this.listBox_sizes.Location = new System.Drawing.Point(853, 32);
            this.listBox_sizes.Name = "listBox_sizes";
            this.listBox_sizes.Size = new System.Drawing.Size(172, 424);
            this.listBox_sizes.TabIndex = 4;
            // 
            // label_segments
            // 
            this.label_segments.AutoSize = true;
            this.label_segments.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_segments.Location = new System.Drawing.Point(672, 8);
            this.label_segments.Name = "label_segments";
            this.label_segments.Size = new System.Drawing.Size(87, 20);
            this.label_segments.TabIndex = 5;
            this.label_segments.Text = "Segments:";
            // 
            // label_segmentsSizes
            // 
            this.label_segmentsSizes.AutoSize = true;
            this.label_segmentsSizes.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_segmentsSizes.Location = new System.Drawing.Point(853, 8);
            this.label_segmentsSizes.Name = "label_segmentsSizes";
            this.label_segmentsSizes.Size = new System.Drawing.Size(166, 20);
            this.label_segmentsSizes.TabIndex = 6;
            this.label_segmentsSizes.Text = "Sizes of the segment:";
            // 
            // button_closeForm
            // 
            this.button_closeForm.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_closeForm.Location = new System.Drawing.Point(584, 566);
            this.button_closeForm.Name = "button_closeForm";
            this.button_closeForm.Size = new System.Drawing.Size(75, 44);
            this.button_closeForm.TabIndex = 7;
            this.button_closeForm.Text = "Close";
            this.button_closeForm.UseVisualStyleBackColor = true;
            this.button_closeForm.Click += new System.EventHandler(this.button_closeForm_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 623);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1264, 25);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(13, 20);
            this.toolStripStatusLabel.Text = " ";
            // 
            // SegmentsSizes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 648);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button_closeForm);
            this.Controls.Add(this.label_segmentsSizes);
            this.Controls.Add(this.label_segments);
            this.Controls.Add(this.listBox_sizes);
            this.Controls.Add(this.listBox_segments);
            this.Controls.Add(this.label_trackBarValue);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.pictureBox_segmentedImage);
            this.Name = "SegmentsSizes";
            this.Text = "Segments Sizes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SegmentsSizes_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SegmentsSizes_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SegmentsSizes_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_segmentedImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_segmentedImage;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label_trackBarValue;
        private System.Windows.Forms.ListBox listBox_segments;
        private System.Windows.Forms.ListBox listBox_sizes;
        private System.Windows.Forms.Label label_segments;
        private System.Windows.Forms.Label label_segmentsSizes;
        private System.Windows.Forms.Button button_closeForm;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}