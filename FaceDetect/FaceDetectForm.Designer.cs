namespace VideoSurveillance
{
   partial class FaceDetectForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaceDetectForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelDetectedPerson = new System.Windows.Forms.Label();
            this.checkBoxEnableReccognition = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageBox2 = new Emgu.CV.UI.ImageBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonTrain = new System.Windows.Forms.Button();
            this.textBoxLabel = new System.Windows.Forms.TextBox();
            this.checkBoxCapture = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.playButton = new System.Windows.Forms.ToolStripButton();
            this.detectionButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.imageBox1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.imageBox2);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(1431, 753);
            this.splitContainer1.SplitterDistance = 916;
            this.splitContainer1.TabIndex = 0;
            // 
            // imageBox1
            // 
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox1.Location = new System.Drawing.Point(0, 48);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(916, 705);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelDetectedPerson);
            this.panel1.Controls.Add(this.checkBoxEnableReccognition);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(916, 48);
            this.panel1.TabIndex = 0;
            // 
            // labelDetectedPerson
            // 
            this.labelDetectedPerson.AutoSize = true;
            this.labelDetectedPerson.Location = new System.Drawing.Point(250, 12);
            this.labelDetectedPerson.Name = "labelDetectedPerson";
            this.labelDetectedPerson.Size = new System.Drawing.Size(90, 13);
            this.labelDetectedPerson.TabIndex = 3;
            this.labelDetectedPerson.Text = "Detected Person:";
            // 
            // checkBoxEnableReccognition
            // 
            this.checkBoxEnableReccognition.AutoSize = true;
            this.checkBoxEnableReccognition.Location = new System.Drawing.Point(115, 11);
            this.checkBoxEnableReccognition.Name = "checkBoxEnableReccognition";
            this.checkBoxEnableReccognition.Size = new System.Drawing.Size(119, 17);
            this.checkBoxEnableReccognition.TabIndex = 2;
            this.checkBoxEnableReccognition.Text = "Enable Recognition";
            this.checkBoxEnableReccognition.UseVisualStyleBackColor = true;
            this.checkBoxEnableReccognition.CheckedChanged += new System.EventHandler(this.checkBoxEnableReccognition_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Camera Frame";
            // 
            // imageBox2
            // 
            this.imageBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox2.Location = new System.Drawing.Point(0, 48);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(511, 705);
            this.imageBox2.TabIndex = 2;
            this.imageBox2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonTrain);
            this.panel2.Controls.Add(this.textBoxLabel);
            this.panel2.Controls.Add(this.checkBoxCapture);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(511, 48);
            this.panel2.TabIndex = 0;
            // 
            // buttonTrain
            // 
            this.buttonTrain.Location = new System.Drawing.Point(365, 11);
            this.buttonTrain.Name = "buttonTrain";
            this.buttonTrain.Size = new System.Drawing.Size(76, 23);
            this.buttonTrain.TabIndex = 4;
            this.buttonTrain.Text = "Train";
            this.buttonTrain.UseVisualStyleBackColor = true;
            this.buttonTrain.Click += new System.EventHandler(this.buttonTrain_Click);
            // 
            // textBoxLabel
            // 
            this.textBoxLabel.Location = new System.Drawing.Point(254, 13);
            this.textBoxLabel.Name = "textBoxLabel";
            this.textBoxLabel.Size = new System.Drawing.Size(105, 20);
            this.textBoxLabel.TabIndex = 2;
            // 
            // checkBoxCapture
            // 
            this.checkBoxCapture.AutoSize = true;
            this.checkBoxCapture.Location = new System.Drawing.Point(105, 13);
            this.checkBoxCapture.Name = "checkBoxCapture";
            this.checkBoxCapture.Size = new System.Drawing.Size(100, 11);
            this.checkBoxCapture.TabIndex = 1;
            this.checkBoxCapture.Text = "Capture Images with label:";
            this.checkBoxCapture.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Forground Mask";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playButton,
            this.detectionButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1431, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // playButton
            // 
            this.playButton.Checked = true;
            this.playButton.CheckOnClick = true;
            this.playButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.playButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.playButton.Image = ((System.Drawing.Image)(resources.GetObject("playButton.Image")));
            this.playButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(69, 22);
            this.playButton.Text = "Play/Pause";
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // detectionButton
            // 
            this.detectionButton.Checked = true;
            this.detectionButton.CheckOnClick = true;
            this.detectionButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.detectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.detectionButton.Image = ((System.Drawing.Image)(resources.GetObject("detectionButton.Image")));
            this.detectionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.detectionButton.Name = "detectionButton";
            this.detectionButton.Size = new System.Drawing.Size(62, 22);
            this.detectionButton.Text = "Detection";
            this.detectionButton.ToolTipText = "Detection";
            this.detectionButton.Click += new System.EventHandler(this.detectionButton_Click);
            // 
            // FaceDetectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1431, 778);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FaceDetectForm";
            this.Text = "VideoSurveilance";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Panel panel2;
      private Emgu.CV.UI.ImageBox imageBox1;
      private System.Windows.Forms.Label label1;
      private Emgu.CV.UI.ImageBox imageBox2;
      private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton playButton;
        private System.Windows.Forms.ToolStripButton detectionButton;
        private System.Windows.Forms.TextBox textBoxLabel;
        private System.Windows.Forms.CheckBox checkBoxCapture;
        private System.Windows.Forms.Button buttonTrain;
        private System.Windows.Forms.CheckBox checkBoxEnableReccognition;
        private System.Windows.Forms.Label labelDetectedPerson;
    }
}