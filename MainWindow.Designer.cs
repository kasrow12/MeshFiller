namespace MeshFiller
{
    partial class MainWindow
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            betaLabel = new Label();
            alphaLabel = new Label();
            label2 = new Label();
            label1 = new Label();
            betaSlider = new TrackBar();
            alphaSlider = new TrackBar();
            groupBox2 = new GroupBox();
            resolutionLabel = new Label();
            triangulationCheckbox = new CheckBox();
            label6 = new Label();
            resolutionSlider = new TrackBar();
            tableLayoutPanel3 = new TableLayoutPanel();
            button1 = new Button();
            button3 = new Button();
            groupBox3 = new GroupBox();
            button2 = new Button();
            checkBox1 = new CheckBox();
            canvas = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)betaSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)alphaSlider).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)resolutionSlider).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Controls.Add(canvas, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel2.Controls.Add(groupBox2, 0, 2);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Controls.Add(groupBox3, 0, 4);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(591, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 6;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(206, 444);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(betaLabel);
            groupBox1.Controls.Add(alphaLabel);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(betaSlider);
            groupBox1.Controls.Add(alphaSlider);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 36);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(200, 110);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Rotation";
            // 
            // betaLabel
            // 
            betaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            betaLabel.Location = new Point(148, 67);
            betaLabel.Name = "betaLabel";
            betaLabel.Size = new Size(46, 15);
            betaLabel.TabIndex = 3;
            betaLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // alphaLabel
            // 
            alphaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            alphaLabel.Location = new Point(148, 19);
            alphaLabel.Name = "alphaLabel";
            alphaLabel.Size = new Size(46, 15);
            alphaLabel.TabIndex = 3;
            alphaLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 67);
            label2.Name = "label2";
            label2.Size = new Size(30, 15);
            label2.TabIndex = 2;
            label2.Text = "Beta";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 19);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "Alpha";
            // 
            // betaSlider
            // 
            betaSlider.AutoSize = false;
            betaSlider.LargeChange = 10;
            betaSlider.Location = new Point(4, 82);
            betaSlider.Maximum = 1200;
            betaSlider.Name = "betaSlider";
            betaSlider.Size = new Size(190, 23);
            betaSlider.SmallChange = 5;
            betaSlider.TabIndex = 0;
            betaSlider.TickStyle = TickStyle.None;
            betaSlider.Value = 500;
            betaSlider.Scroll += BetaSlider_Scroll;
            // 
            // alphaSlider
            // 
            alphaSlider.AutoSize = false;
            alphaSlider.LargeChange = 100;
            alphaSlider.Location = new Point(4, 37);
            alphaSlider.Maximum = 900;
            alphaSlider.Minimum = -900;
            alphaSlider.Name = "alphaSlider";
            alphaSlider.Size = new Size(190, 27);
            alphaSlider.SmallChange = 10;
            alphaSlider.TabIndex = 0;
            alphaSlider.TickStyle = TickStyle.None;
            alphaSlider.Scroll += AlphaSlider_Scroll;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(resolutionLabel);
            groupBox2.Controls.Add(triangulationCheckbox);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(resolutionSlider);
            groupBox2.Location = new Point(3, 152);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(200, 96);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Triangulation";
            // 
            // resolutionLabel
            // 
            resolutionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            resolutionLabel.Location = new Point(148, 44);
            resolutionLabel.Name = "resolutionLabel";
            resolutionLabel.Size = new Size(46, 15);
            resolutionLabel.TabIndex = 3;
            resolutionLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // triangulationCheckbox
            // 
            triangulationCheckbox.AutoSize = true;
            triangulationCheckbox.Location = new Point(6, 22);
            triangulationCheckbox.Name = "triangulationCheckbox";
            triangulationCheckbox.Size = new Size(126, 19);
            triangulationCheckbox.TabIndex = 0;
            triangulationCheckbox.Text = "Show triangulation";
            triangulationCheckbox.UseVisualStyleBackColor = true;
            triangulationCheckbox.CheckedChanged += triangulationCheckbox_CheckedChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 44);
            label6.Name = "label6";
            label6.Size = new Size(63, 15);
            label6.TabIndex = 2;
            label6.Text = "Resolution";
            // 
            // resolutionSlider
            // 
            resolutionSlider.AutoSize = false;
            resolutionSlider.LargeChange = 100;
            resolutionSlider.Location = new Point(4, 62);
            resolutionSlider.Maximum = 100;
            resolutionSlider.Minimum = 2;
            resolutionSlider.Name = "resolutionSlider";
            resolutionSlider.Size = new Size(190, 28);
            resolutionSlider.SmallChange = 10;
            resolutionSlider.TabIndex = 0;
            resolutionSlider.TickStyle = TickStyle.None;
            resolutionSlider.Value = 10;
            resolutionSlider.Scroll += ResolutionSlider_Scroll;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(button1, 0, 0);
            tableLayoutPanel3.Controls.Add(button3, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(206, 33);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(97, 27);
            button1.TabIndex = 1;
            button1.Text = "Load surface";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button3
            // 
            button3.Location = new Point(106, 3);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 2;
            button3.Text = "anim";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button2);
            groupBox3.Controls.Add(checkBox1);
            groupBox3.Location = new Point(3, 274);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(200, 76);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Normals";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button2.Location = new Point(4, 47);
            button2.Name = "button2";
            button2.Size = new Size(192, 23);
            button2.TabIndex = 1;
            button2.Text = "Load normal map";
            button2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(6, 22);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(136, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Custom normal map";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // canvas
            // 
            canvas.BackColor = Color.White;
            canvas.Dock = DockStyle.Fill;
            canvas.Location = new Point(0, 0);
            canvas.Margin = new Padding(0);
            canvas.Name = "canvas";
            canvas.Size = new Size(588, 450);
            canvas.TabIndex = 1;
            canvas.TabStop = false;
            canvas.Paint += Canvas_Paint;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            MinimumSize = new Size(600, 400);
            Name = "MainWindow";
            Text = "MeshFiller";
            Resize += MainWindow_Resize;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)betaSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)alphaSlider).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)resolutionSlider).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TrackBar alphaSlider;
        private Button button1;
        private PictureBox canvas;
        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private TrackBar betaSlider;
        private Label betaLabel;
        private Label alphaLabel;
        private GroupBox groupBox2;
        private Label resolutionLabel;
        private Label label6;
        private TrackBar resolutionSlider;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox3;
        private CheckBox checkBox1;
        private Button button2;
        private Button button3;
        private CheckBox triangulationCheckbox;
    }
}
