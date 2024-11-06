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
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            canvas = new PictureBox();
            menuStrip1 = new MenuStrip();
            loadSurfaceToolStripMenuItem = new ToolStripMenuItem();
            loadSurfaceButton = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            loadNormalMapButton = new ToolStripMenuItem();
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
            label3 = new Label();
            resolutionSlider = new TrackBar();
            tableLayoutPanel3 = new TableLayoutPanel();
            groupBox3 = new GroupBox();
            textureRadio = new RadioButton();
            solidColorRadio = new RadioButton();
            objectColorSelect = new Panel();
            normalMapCheckbox = new CheckBox();
            groupBox5 = new GroupBox();
            lightAnimationCheckbox = new CheckBox();
            lightColorSelect = new Panel();
            mLabel = new Label();
            ksLabel = new Label();
            kdLabel = new Label();
            label5 = new Label();
            label11 = new Label();
            mSlider = new TrackBar();
            label9 = new Label();
            label4 = new Label();
            ksSlider = new TrackBar();
            kdSlider = new TrackBar();
            bindingSource1 = new BindingSource(components);
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            menuStrip1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)betaSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)alphaSlider).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)resolutionSlider).BeginInit();
            groupBox3.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ksSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kdSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel1.Controls.Add(canvas, 0, 1);
            tableLayoutPanel1.Controls.Add(menuStrip1, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1084, 661);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // canvas
            // 
            canvas.BackColor = Color.White;
            canvas.Dock = DockStyle.Fill;
            canvas.Location = new Point(0, 24);
            canvas.Margin = new Padding(0);
            canvas.Name = "canvas";
            canvas.Size = new Size(784, 637);
            canvas.TabIndex = 1;
            canvas.TabStop = false;
            canvas.Paint += Canvas_Paint;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { loadSurfaceToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(784, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // loadSurfaceToolStripMenuItem
            // 
            loadSurfaceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadSurfaceButton, toolStripMenuItem1, loadNormalMapButton });
            loadSurfaceToolStripMenuItem.Name = "loadSurfaceToolStripMenuItem";
            loadSurfaceToolStripMenuItem.Size = new Size(37, 20);
            loadSurfaceToolStripMenuItem.Text = "File";
            // 
            // loadSurfaceButton
            // 
            loadSurfaceButton.Name = "loadSurfaceButton";
            loadSurfaceButton.Size = new Size(168, 22);
            loadSurfaceButton.Text = "Load surface";
            loadSurfaceButton.Click += LoadSurfaceButton_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(168, 22);
            toolStripMenuItem1.Text = "Load texture";
            // 
            // loadNormalMapButton
            // 
            loadNormalMapButton.Name = "loadNormalMapButton";
            loadNormalMapButton.Size = new Size(168, 22);
            loadNormalMapButton.Text = "Load normal map";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel2.Controls.Add(groupBox2, 0, 2);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Controls.Add(groupBox3, 0, 5);
            tableLayoutPanel2.Controls.Add(groupBox5, 0, 3);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(787, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 7;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel2, 2);
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(294, 655);
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
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(288, 84);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Rotation";
            // 
            // betaLabel
            // 
            betaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            betaLabel.BackColor = Color.Transparent;
            betaLabel.Location = new Point(247, 55);
            betaLabel.Margin = new Padding(0);
            betaLabel.Name = "betaLabel";
            betaLabel.Size = new Size(61, 15);
            betaLabel.TabIndex = 3;
            // 
            // alphaLabel
            // 
            alphaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            alphaLabel.BackColor = Color.Transparent;
            alphaLabel.Location = new Point(247, 21);
            alphaLabel.Margin = new Padding(0);
            alphaLabel.Name = "alphaLabel";
            alphaLabel.Size = new Size(56, 16);
            alphaLabel.TabIndex = 3;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(18, 55);
            label2.Name = "label2";
            label2.Size = new Size(30, 15);
            label2.TabIndex = 2;
            label2.Text = "Beta";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(10, 21);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 2;
            label1.Text = "Alpha";
            // 
            // betaSlider
            // 
            betaSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            betaSlider.AutoSize = false;
            betaSlider.LargeChange = 10;
            betaSlider.Location = new Point(52, 53);
            betaSlider.Maximum = 1200;
            betaSlider.Name = "betaSlider";
            betaSlider.Size = new Size(192, 23);
            betaSlider.SmallChange = 5;
            betaSlider.TabIndex = 0;
            betaSlider.TickStyle = TickStyle.None;
            betaSlider.Value = 500;
            betaSlider.Scroll += AngleSlider_Scroll;
            // 
            // alphaSlider
            // 
            alphaSlider.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            alphaSlider.AutoSize = false;
            alphaSlider.LargeChange = 100;
            alphaSlider.Location = new Point(52, 20);
            alphaSlider.Maximum = 900;
            alphaSlider.Minimum = -900;
            alphaSlider.Name = "alphaSlider";
            alphaSlider.Size = new Size(192, 27);
            alphaSlider.SmallChange = 10;
            alphaSlider.TabIndex = 0;
            alphaSlider.TickStyle = TickStyle.None;
            alphaSlider.Scroll += AngleSlider_Scroll;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(resolutionLabel);
            groupBox2.Controls.Add(triangulationCheckbox);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(resolutionSlider);
            groupBox2.Location = new Point(3, 93);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(288, 79);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Triangulation";
            // 
            // resolutionLabel
            // 
            resolutionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resolutionLabel.BackColor = Color.Transparent;
            resolutionLabel.Location = new Point(247, 50);
            resolutionLabel.Margin = new Padding(0);
            resolutionLabel.Name = "resolutionLabel";
            resolutionLabel.Size = new Size(61, 15);
            resolutionLabel.TabIndex = 3;
            // 
            // triangulationCheckbox
            // 
            triangulationCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            triangulationCheckbox.AutoSize = true;
            triangulationCheckbox.Location = new Point(10, 22);
            triangulationCheckbox.Name = "triangulationCheckbox";
            triangulationCheckbox.Size = new Size(126, 19);
            triangulationCheckbox.TabIndex = 0;
            triangulationCheckbox.Text = "Show triangulation";
            triangulationCheckbox.UseVisualStyleBackColor = true;
            triangulationCheckbox.CheckedChanged += triangulationCheckbox_CheckedChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(8, 50);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 2;
            label3.Text = "Density";
            // 
            // resolutionSlider
            // 
            resolutionSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resolutionSlider.AutoSize = false;
            resolutionSlider.LargeChange = 20;
            resolutionSlider.Location = new Point(52, 47);
            resolutionSlider.Maximum = 100;
            resolutionSlider.Minimum = 2;
            resolutionSlider.Name = "resolutionSlider";
            resolutionSlider.Size = new Size(192, 23);
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
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(294, 1);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textureRadio);
            groupBox3.Controls.Add(solidColorRadio);
            groupBox3.Controls.Add(objectColorSelect);
            groupBox3.Controls.Add(normalMapCheckbox);
            groupBox3.Location = new Point(3, 332);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(288, 80);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Object";
            // 
            // textureRadio
            // 
            textureRadio.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textureRadio.AutoSize = true;
            textureRadio.Location = new Point(10, 49);
            textureRadio.Name = "textureRadio";
            textureRadio.Size = new Size(63, 19);
            textureRadio.TabIndex = 5;
            textureRadio.Text = "Texture";
            textureRadio.UseVisualStyleBackColor = true;
            // 
            // solidColorRadio
            // 
            solidColorRadio.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            solidColorRadio.AutoSize = true;
            solidColorRadio.Checked = true;
            solidColorRadio.Location = new Point(10, 24);
            solidColorRadio.Name = "solidColorRadio";
            solidColorRadio.Size = new Size(81, 19);
            solidColorRadio.TabIndex = 5;
            solidColorRadio.TabStop = true;
            solidColorRadio.Text = "Solid color";
            solidColorRadio.UseVisualStyleBackColor = true;
            // 
            // objectColorSelect
            // 
            objectColorSelect.BackColor = Color.Blue;
            objectColorSelect.Cursor = Cursors.Hand;
            objectColorSelect.Location = new Point(95, 19);
            objectColorSelect.Name = "objectColorSelect";
            objectColorSelect.Size = new Size(30, 30);
            objectColorSelect.TabIndex = 4;
            // 
            // normalMapCheckbox
            // 
            normalMapCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            normalMapCheckbox.AutoSize = true;
            normalMapCheckbox.Location = new Point(154, 24);
            normalMapCheckbox.Name = "normalMapCheckbox";
            normalMapCheckbox.Size = new Size(113, 19);
            normalMapCheckbox.TabIndex = 0;
            normalMapCheckbox.Text = "Use normal map";
            normalMapCheckbox.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(lightAnimationCheckbox);
            groupBox5.Controls.Add(lightColorSelect);
            groupBox5.Controls.Add(mLabel);
            groupBox5.Controls.Add(ksLabel);
            groupBox5.Controls.Add(kdLabel);
            groupBox5.Controls.Add(label5);
            groupBox5.Controls.Add(label11);
            groupBox5.Controls.Add(mSlider);
            groupBox5.Controls.Add(label9);
            groupBox5.Controls.Add(label4);
            groupBox5.Controls.Add(ksSlider);
            groupBox5.Controls.Add(kdSlider);
            groupBox5.Location = new Point(3, 178);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(288, 148);
            groupBox5.TabIndex = 6;
            groupBox5.TabStop = false;
            groupBox5.Text = "Lighting";
            // 
            // lightAnimationCheckbox
            // 
            lightAnimationCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lightAnimationCheckbox.AutoSize = true;
            lightAnimationCheckbox.Location = new Point(154, 115);
            lightAnimationCheckbox.Name = "lightAnimationCheckbox";
            lightAnimationCheckbox.Size = new Size(82, 19);
            lightAnimationCheckbox.TabIndex = 5;
            lightAnimationCheckbox.Text = "Animation";
            lightAnimationCheckbox.UseVisualStyleBackColor = true;
            // 
            // lightColorSelect
            // 
            lightColorSelect.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lightColorSelect.BackColor = Color.White;
            lightColorSelect.Cursor = Cursors.Hand;
            lightColorSelect.Location = new Point(18, 108);
            lightColorSelect.Name = "lightColorSelect";
            lightColorSelect.Size = new Size(30, 30);
            lightColorSelect.TabIndex = 4;
            // 
            // mLabel
            // 
            mLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mLabel.BackColor = Color.Transparent;
            mLabel.Location = new Point(247, 80);
            mLabel.Margin = new Padding(0);
            mLabel.Name = "mLabel";
            mLabel.Size = new Size(61, 15);
            mLabel.TabIndex = 3;
            // 
            // ksLabel
            // 
            ksLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ksLabel.BackColor = Color.Transparent;
            ksLabel.Location = new Point(247, 51);
            ksLabel.Margin = new Padding(0);
            ksLabel.Name = "ksLabel";
            ksLabel.Size = new Size(61, 15);
            ksLabel.TabIndex = 3;
            // 
            // kdLabel
            // 
            kdLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            kdLabel.BackColor = Color.Transparent;
            kdLabel.Location = new Point(247, 24);
            kdLabel.Margin = new Padding(0);
            kdLabel.Name = "kdLabel";
            kdLabel.Size = new Size(61, 15);
            kdLabel.TabIndex = 3;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(54, 116);
            label5.Name = "label5";
            label5.Size = new Size(36, 15);
            label5.TabIndex = 2;
            label5.Text = "Color";
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Location = new Point(28, 80);
            label11.Name = "label11";
            label11.Size = new Size(18, 15);
            label11.TabIndex = 2;
            label11.Text = "m";
            // 
            // mSlider
            // 
            mSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mSlider.AutoSize = false;
            mSlider.LargeChange = 20;
            mSlider.Location = new Point(52, 78);
            mSlider.Maximum = 100;
            mSlider.Minimum = 1;
            mSlider.Name = "mSlider";
            mSlider.Size = new Size(192, 23);
            mSlider.SmallChange = 10;
            mSlider.TabIndex = 0;
            mSlider.TickStyle = TickStyle.None;
            mSlider.Value = 10;
            mSlider.Scroll += LightingSlider_Scroll;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Location = new Point(28, 51);
            label9.Name = "label9";
            label9.Size = new Size(18, 15);
            label9.TabIndex = 2;
            label9.Text = "ks";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(28, 24);
            label4.Name = "label4";
            label4.Size = new Size(20, 15);
            label4.TabIndex = 2;
            label4.Text = "kd";
            // 
            // ksSlider
            // 
            ksSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ksSlider.AutoSize = false;
            ksSlider.LargeChange = 20;
            ksSlider.Location = new Point(52, 50);
            ksSlider.Maximum = 100;
            ksSlider.Name = "ksSlider";
            ksSlider.Size = new Size(192, 23);
            ksSlider.SmallChange = 10;
            ksSlider.TabIndex = 0;
            ksSlider.TickStyle = TickStyle.None;
            ksSlider.Value = 30;
            ksSlider.Scroll += LightingSlider_Scroll;
            // 
            // kdSlider
            // 
            kdSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            kdSlider.AutoSize = false;
            kdSlider.LargeChange = 20;
            kdSlider.Location = new Point(52, 21);
            kdSlider.Maximum = 100;
            kdSlider.Name = "kdSlider";
            kdSlider.Size = new Size(192, 23);
            kdSlider.SmallChange = 10;
            kdSlider.TabIndex = 0;
            kdSlider.TickStyle = TickStyle.None;
            kdSlider.Value = 90;
            kdSlider.Scroll += LightingSlider_Scroll;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 661);
            Controls.Add(tableLayoutPanel1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(600, 400);
            Name = "MainWindow";
            Text = "MeshFiller";
            Resize += MainWindow_Resize;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)betaSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)alphaSlider).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)resolutionSlider).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)ksSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)kdSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TrackBar alphaSlider;
        private PictureBox canvas;
        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private TrackBar betaSlider;
        private Label betaLabel;
        private Label alphaLabel;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox3;
        private CheckBox normalMapCheckbox;
        private CheckBox triangulationCheckbox;
        private TrackBar mSlider;
        private Label mLabel;
        private TrackBar kdSlider;
        private Label kdLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem loadSurfaceToolStripMenuItem;
        private ToolStripMenuItem loadSurfaceButton;
        private ToolStripMenuItem loadNormalMapButton;
        private GroupBox groupBox5;
        private Label label4;
        private Label ksLabel;
        private Label label11;
        private Label label9;
        private TrackBar ksSlider;
        private TrackBar trackBar2;
        private Label resolutionLabel;
        private Label label3;
        private TrackBar resolutionSlider;
        private ToolStripMenuItem toolStripMenuItem1;
        private Panel lightColorSelect;
        private RadioButton textureRadio;
        private RadioButton solidColorRadio;
        private Panel objectColorSelect;
        private Label label5;
        private BindingSource bindingSource1;
        private CheckBox lightAnimationCheckbox;
    }
}
