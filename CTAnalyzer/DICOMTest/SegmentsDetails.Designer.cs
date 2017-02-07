namespace DICOMopener
{
    partial class SegmentsDetails
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
            this.pictureBox_segmentsDetails = new System.Windows.Forms.PictureBox();
            this.label_segmentSizes = new System.Windows.Forms.Label();
            this.listBox_segmentSizes = new System.Windows.Forms.ListBox();
            this.button_closeForm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_segmentsDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_segmentsDetails
            // 
            this.pictureBox_segmentsDetails.Location = new System.Drawing.Point(109, 12);
            this.pictureBox_segmentsDetails.Name = "pictureBox_segmentsDetails";
            this.pictureBox_segmentsDetails.Size = new System.Drawing.Size(512, 512);
            this.pictureBox_segmentsDetails.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_segmentsDetails.TabIndex = 0;
            this.pictureBox_segmentsDetails.TabStop = false;
            // 
            // label_segmentSizes
            // 
            this.label_segmentSizes.AutoSize = true;
            this.label_segmentSizes.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_segmentSizes.Location = new System.Drawing.Point(8, 532);
            this.label_segmentSizes.Name = "label_segmentSizes";
            this.label_segmentSizes.Size = new System.Drawing.Size(119, 20);
            this.label_segmentSizes.TabIndex = 1;
            this.label_segmentSizes.Text = "Segment sizes:";
            // 
            // listBox_segmentSizes
            // 
            this.listBox_segmentSizes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox_segmentSizes.FormattingEnabled = true;
            this.listBox_segmentSizes.Location = new System.Drawing.Point(12, 555);
            this.listBox_segmentSizes.Name = "listBox_segmentSizes";
            this.listBox_segmentSizes.Size = new System.Drawing.Size(720, 95);
            this.listBox_segmentSizes.TabIndex = 2;
            this.listBox_segmentSizes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_segmentSizes_DrawItem);
            // 
            // button_closeForm
            // 
            this.button_closeForm.Font = new System.Drawing.Font("Georgia", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_closeForm.Location = new System.Drawing.Point(322, 670);
            this.button_closeForm.Name = "button_closeForm";
            this.button_closeForm.Size = new System.Drawing.Size(75, 36);
            this.button_closeForm.TabIndex = 3;
            this.button_closeForm.Text = "Close";
            this.button_closeForm.UseVisualStyleBackColor = true;
            this.button_closeForm.Click += new System.EventHandler(this.button_closeForm_Click);
            // 
            // SegmentsDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 718);
            this.Controls.Add(this.button_closeForm);
            this.Controls.Add(this.listBox_segmentSizes);
            this.Controls.Add(this.label_segmentSizes);
            this.Controls.Add(this.pictureBox_segmentsDetails);
            this.Name = "SegmentsDetails";
            this.Text = "Segments Details";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_segmentsDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_segmentsDetails;
        private System.Windows.Forms.Label label_segmentSizes;
        private System.Windows.Forms.ListBox listBox_segmentSizes;
        private System.Windows.Forms.Button button_closeForm;
    }
}