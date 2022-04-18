
namespace lab_1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kanalskeSlikeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.embossLaplacianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gamaFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edgeDetectDifferenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displacementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.podesavanjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.velicinaUndoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.MBbufferSizePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.MBbufferSizeInput = new System.Windows.Forms.TextBox();
            this.MBbufferSizeOKbtn = new System.Windows.Forms.Button();
            this.GamaLabel = new System.Windows.Forms.Label();
            this.GamaTrackBar = new System.Windows.Forms.TrackBar();
            this.GamaValueLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.MBbufferSizePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GamaTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(12, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 358);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(485, 353);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.filtersToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(588, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kanalskeSlikeToolStripMenuItem,
            this.invertFilterToolStripMenuItem,
            this.embossLaplacianToolStripMenuItem,
            this.gamaFilterToolStripMenuItem,
            this.edgeDetectDifferenceToolStripMenuItem,
            this.displacementToolStripMenuItem});
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.filtersToolStripMenuItem.Text = "Filters";
            // 
            // kanalskeSlikeToolStripMenuItem
            // 
            this.kanalskeSlikeToolStripMenuItem.Name = "kanalskeSlikeToolStripMenuItem";
            this.kanalskeSlikeToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.kanalskeSlikeToolStripMenuItem.Text = "Kanalske slike";
            this.kanalskeSlikeToolStripMenuItem.Click += new System.EventHandler(this.kanalskeSlikeToolStripMenuItem_Click);
            // 
            // invertFilterToolStripMenuItem
            // 
            this.invertFilterToolStripMenuItem.Name = "invertFilterToolStripMenuItem";
            this.invertFilterToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.invertFilterToolStripMenuItem.Text = "Invert filter";
            this.invertFilterToolStripMenuItem.Click += new System.EventHandler(this.invertFilterToolStripMenuItem_Click);
            // 
            // embossLaplacianToolStripMenuItem
            // 
            this.embossLaplacianToolStripMenuItem.Name = "embossLaplacianToolStripMenuItem";
            this.embossLaplacianToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.embossLaplacianToolStripMenuItem.Text = "Emboss Laplacian";
            this.embossLaplacianToolStripMenuItem.Click += new System.EventHandler(this.embossLaplacianToolStripMenuItem_Click);
            // 
            // gamaFilterToolStripMenuItem
            // 
            this.gamaFilterToolStripMenuItem.Name = "gamaFilterToolStripMenuItem";
            this.gamaFilterToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.gamaFilterToolStripMenuItem.Text = "Gama filter";
            this.gamaFilterToolStripMenuItem.Click += new System.EventHandler(this.gamaFilterToolStripMenuItem_Click);
            // 
            // edgeDetectDifferenceToolStripMenuItem
            // 
            this.edgeDetectDifferenceToolStripMenuItem.Name = "edgeDetectDifferenceToolStripMenuItem";
            this.edgeDetectDifferenceToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.edgeDetectDifferenceToolStripMenuItem.Text = "Edge Detect Difference";
            this.edgeDetectDifferenceToolStripMenuItem.Click += new System.EventHandler(this.edgeDetectDifferenceToolStripMenuItem_Click);
            // 
            // displacementToolStripMenuItem
            // 
            this.displacementToolStripMenuItem.Name = "displacementToolStripMenuItem";
            this.displacementToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.displacementToolStripMenuItem.Text = "Random Jiter";
            this.displacementToolStripMenuItem.Click += new System.EventHandler(this.displacementToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.podesavanjeToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // podesavanjeToolStripMenuItem
            // 
            this.podesavanjeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.velicinaUndoToolStripMenuItem});
            this.podesavanjeToolStripMenuItem.Name = "podesavanjeToolStripMenuItem";
            this.podesavanjeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.podesavanjeToolStripMenuItem.Text = "Podesavanje";
            // 
            // velicinaUndoToolStripMenuItem
            // 
            this.velicinaUndoToolStripMenuItem.Name = "velicinaUndoToolStripMenuItem";
            this.velicinaUndoToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.velicinaUndoToolStripMenuItem.Text = "Velicina undo";
            this.velicinaUndoToolStripMenuItem.Click += new System.EventHandler(this.velicinaUndoToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Controls.Add(this.pictureBox5);
            this.panel2.Controls.Add(this.pictureBox4);
            this.panel2.Controls.Add(this.pictureBox3);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(12, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(494, 358);
            this.panel2.TabIndex = 2;
            this.panel2.Visible = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Location = new System.Drawing.Point(252, 161);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(235, 138);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Location = new System.Drawing.Point(11, 161);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(235, 138);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(252, 17);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(235, 138);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(11, 17);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(235, 138);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // MBbufferSizePanel
            // 
            this.MBbufferSizePanel.Controls.Add(this.label1);
            this.MBbufferSizePanel.Controls.Add(this.MBbufferSizeInput);
            this.MBbufferSizePanel.Controls.Add(this.MBbufferSizeOKbtn);
            this.MBbufferSizePanel.Location = new System.Drawing.Point(12, 27);
            this.MBbufferSizePanel.Name = "MBbufferSizePanel";
            this.MBbufferSizePanel.Size = new System.Drawing.Size(491, 358);
            this.MBbufferSizePanel.TabIndex = 3;
            this.MBbufferSizePanel.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input undo buffer size in MB";
            // 
            // MBbufferSizeInput
            // 
            this.MBbufferSizeInput.Location = new System.Drawing.Point(139, 134);
            this.MBbufferSizeInput.Name = "MBbufferSizeInput";
            this.MBbufferSizeInput.Size = new System.Drawing.Size(210, 23);
            this.MBbufferSizeInput.TabIndex = 1;
            // 
            // MBbufferSizeOKbtn
            // 
            this.MBbufferSizeOKbtn.Location = new System.Drawing.Point(139, 209);
            this.MBbufferSizeOKbtn.Name = "MBbufferSizeOKbtn";
            this.MBbufferSizeOKbtn.Size = new System.Drawing.Size(210, 23);
            this.MBbufferSizeOKbtn.TabIndex = 0;
            this.MBbufferSizeOKbtn.Text = "OK";
            this.MBbufferSizeOKbtn.UseVisualStyleBackColor = true;
            // 
            // GamaLabel
            // 
            this.GamaLabel.AutoSize = true;
            this.GamaLabel.Location = new System.Drawing.Point(34, 421);
            this.GamaLabel.Name = "GamaLabel";
            this.GamaLabel.Size = new System.Drawing.Size(38, 15);
            this.GamaLabel.TabIndex = 5;
            this.GamaLabel.Text = "Gama";
            this.GamaLabel.Visible = false;
            // 
            // GamaTrackBar
            // 
            this.GamaTrackBar.Location = new System.Drawing.Point(78, 411);
            this.GamaTrackBar.Minimum = 1;
            this.GamaTrackBar.Name = "GamaTrackBar";
            this.GamaTrackBar.Size = new System.Drawing.Size(231, 45);
            this.GamaTrackBar.TabIndex = 6;
            this.GamaTrackBar.Value = 1;
            this.GamaTrackBar.Visible = false;
            this.GamaTrackBar.Scroll += new System.EventHandler(this.GamaTrackBar_Scroll_1);
            // 
            // GamaValueLabel
            // 
            this.GamaValueLabel.AutoSize = true;
            this.GamaValueLabel.Location = new System.Drawing.Point(316, 420);
            this.GamaValueLabel.Name = "GamaValueLabel";
            this.GamaValueLabel.Size = new System.Drawing.Size(13, 15);
            this.GamaValueLabel.TabIndex = 7;
            this.GamaValueLabel.Text = "1";
            this.GamaValueLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 477);
            this.Controls.Add(this.GamaValueLabel);
            this.Controls.Add(this.GamaTrackBar);
            this.Controls.Add(this.GamaLabel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.MBbufferSizePanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.MBbufferSizePanel.ResumeLayout(false);
            this.MBbufferSizePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GamaTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kanalskeSlikeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem embossLaplacianToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gamaFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem podesavanjeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem velicinaUndoToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel MBbufferSizePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MBbufferSizeInput;
        private System.Windows.Forms.Button MBbufferSizeOKbtn;
        private System.Windows.Forms.Label GamaLabel;
        private System.Windows.Forms.TrackBar GamaTrackBar;
        private System.Windows.Forms.Label GamaValueLabel;
        private System.Windows.Forms.ToolStripMenuItem edgeDetectDifferenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displacementToolStripMenuItem;
    }
}

