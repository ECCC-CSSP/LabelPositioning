namespace LabelPositioning
{
    partial class LabelPositioning
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
            this.pictureBoxLabelPosition = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.butClearAll = new System.Windows.Forms.Button();
            this.lblClickInTheBoxBelow = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLabelPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxLabelPosition
            // 
            this.pictureBoxLabelPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLabelPosition.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLabelPosition.Name = "pictureBoxLabelPosition";
            this.pictureBoxLabelPosition.Size = new System.Drawing.Size(1377, 765);
            this.pictureBoxLabelPosition.TabIndex = 0;
            this.pictureBoxLabelPosition.TabStop = false;
            this.pictureBoxLabelPosition.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxLabelPosition_MouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblClickInTheBoxBelow);
            this.splitContainer1.Panel1.Controls.Add(this.butClearAll);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxLabelPosition);
            this.splitContainer1.Size = new System.Drawing.Size(1381, 817);
            this.splitContainer1.SplitterDistance = 44;
            this.splitContainer1.TabIndex = 1;
            // 
            // butClearAll
            // 
            this.butClearAll.Location = new System.Drawing.Point(25, 10);
            this.butClearAll.Name = "butClearAll";
            this.butClearAll.Size = new System.Drawing.Size(119, 23);
            this.butClearAll.TabIndex = 0;
            this.butClearAll.Text = "Clear all";
            this.butClearAll.UseVisualStyleBackColor = true;
            this.butClearAll.Click += new System.EventHandler(this.butClearAll_Click);
            // 
            // lblClickInTheBoxBelow
            // 
            this.lblClickInTheBoxBelow.AutoSize = true;
            this.lblClickInTheBoxBelow.Location = new System.Drawing.Point(182, 11);
            this.lblClickInTheBoxBelow.Name = "lblClickInTheBoxBelow";
            this.lblClickInTheBoxBelow.Size = new System.Drawing.Size(149, 13);
            this.lblClickInTheBoxBelow.TabIndex = 1;
            this.lblClickInTheBoxBelow.Text = "Click on the box below to start";
            // 
            // LabelPositioning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 817);
            this.Controls.Add(this.splitContainer1);
            this.Name = "LabelPositioning";
            this.Text = "Label Positioning";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLabelPosition)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLabelPosition;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button butClearAll;
        private System.Windows.Forms.Label lblClickInTheBoxBelow;
    }
}

