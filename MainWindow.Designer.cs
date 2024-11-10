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
            alphaLabel = new Label();
            alphaSlider = new TrackBar();
            betaLabel = new Label();
            label2 = new Label();
            label1 = new Label();
            betaSlider = new TrackBar();
            groupBox2 = new GroupBox();
            resolutionLabel = new Label();
            triangulationCheckbox = new CheckBox();
            label3 = new Label();
            resolutionSlider = new TrackBar();
            tableLayoutPanel3 = new TableLayoutPanel();
            groupBox3 = new GroupBox();
            textureRadio = new RadioButton();
            solidColorRadio = new RadioButton();
            textureSelect = new Panel();
            normalMapSelect = new Panel();
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
            button1 = new Button();
            canvas = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)alphaSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)betaSlider).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)resolutionSlider).BeginInit();
            groupBox3.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ksSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)kdSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 343F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Controls.Add(canvas, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1239, 881);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(groupBox1, 0, 2);
            tableLayoutPanel2.Controls.Add(groupBox2, 0, 3);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel2.Controls.Add(groupBox3, 0, 6);
            tableLayoutPanel2.Controls.Add(groupBox5, 0, 4);
            tableLayoutPanel2.Controls.Add(button1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(899, 4);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 8;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel2.Size = new Size(337, 873);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(alphaLabel);
            groupBox1.Controls.Add(alphaSlider);
            groupBox1.Controls.Add(betaLabel);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(betaSlider);
            groupBox1.Location = new Point(3, 43);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(329, 112);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Rotation";
            // 
            // alphaLabel
            // 
            alphaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            alphaLabel.BackColor = Color.Transparent;
            alphaLabel.Location = new Point(275, 28);
            alphaLabel.Margin = new Padding(0);
            alphaLabel.Name = "alphaLabel";
            alphaLabel.Size = new Size(50, 20);
            alphaLabel.TabIndex = 3;
            // 
            // alphaSlider
            // 
            alphaSlider.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            alphaSlider.AutoSize = false;
            alphaSlider.LargeChange = 100;
            alphaSlider.Location = new Point(59, 27);
            alphaSlider.Margin = new Padding(3, 4, 3, 4);
            alphaSlider.Maximum = 900;
            alphaSlider.Minimum = -900;
            alphaSlider.Name = "alphaSlider";
            alphaSlider.Size = new Size(219, 36);
            alphaSlider.SmallChange = 10;
            alphaSlider.TabIndex = 0;
            alphaSlider.TickStyle = TickStyle.None;
            alphaSlider.Scroll += AngleSlider_Scroll;
            // 
            // betaLabel
            // 
            betaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            betaLabel.BackColor = Color.Transparent;
            betaLabel.Location = new Point(275, 73);
            betaLabel.Margin = new Padding(0);
            betaLabel.Name = "betaLabel";
            betaLabel.Size = new Size(50, 20);
            betaLabel.TabIndex = 3;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(21, 73);
            label2.Name = "label2";
            label2.Size = new Size(39, 20);
            label2.TabIndex = 2;
            label2.Text = "Beta";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(11, 28);
            label1.Name = "label1";
            label1.Size = new Size(48, 20);
            label1.TabIndex = 2;
            label1.Text = "Alpha";
            // 
            // betaSlider
            // 
            betaSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            betaSlider.AutoSize = false;
            betaSlider.LargeChange = 10;
            betaSlider.Location = new Point(59, 71);
            betaSlider.Margin = new Padding(3, 4, 3, 4);
            betaSlider.Maximum = 1200;
            betaSlider.Name = "betaSlider";
            betaSlider.Size = new Size(219, 31);
            betaSlider.SmallChange = 5;
            betaSlider.TabIndex = 0;
            betaSlider.TickStyle = TickStyle.None;
            betaSlider.Scroll += AngleSlider_Scroll;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(resolutionLabel);
            groupBox2.Controls.Add(triangulationCheckbox);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(resolutionSlider);
            groupBox2.Location = new Point(3, 163);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(329, 105);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Triangulation";
            // 
            // resolutionLabel
            // 
            resolutionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resolutionLabel.BackColor = Color.Transparent;
            resolutionLabel.Location = new Point(275, 67);
            resolutionLabel.Margin = new Padding(0);
            resolutionLabel.Name = "resolutionLabel";
            resolutionLabel.Size = new Size(50, 20);
            resolutionLabel.TabIndex = 3;
            // 
            // triangulationCheckbox
            // 
            triangulationCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            triangulationCheckbox.AutoSize = true;
            triangulationCheckbox.Location = new Point(11, 29);
            triangulationCheckbox.Margin = new Padding(3, 4, 3, 4);
            triangulationCheckbox.Name = "triangulationCheckbox";
            triangulationCheckbox.Size = new Size(156, 24);
            triangulationCheckbox.TabIndex = 0;
            triangulationCheckbox.Text = "Show triangulation";
            triangulationCheckbox.UseVisualStyleBackColor = true;
            triangulationCheckbox.CheckedChanged += TriangulationCheckbox_CheckedChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(9, 67);
            label3.Name = "label3";
            label3.Size = new Size(58, 20);
            label3.TabIndex = 2;
            label3.Text = "Density";
            // 
            // resolutionSlider
            // 
            resolutionSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            resolutionSlider.AutoSize = false;
            resolutionSlider.LargeChange = 20;
            resolutionSlider.Location = new Point(59, 63);
            resolutionSlider.Margin = new Padding(3, 4, 3, 4);
            resolutionSlider.Maximum = 64;
            resolutionSlider.Minimum = 2;
            resolutionSlider.Name = "resolutionSlider";
            resolutionSlider.Size = new Size(219, 31);
            resolutionSlider.SmallChange = 8;
            resolutionSlider.TabIndex = 0;
            resolutionSlider.TickStyle = TickStyle.None;
            resolutionSlider.Value = 16;
            resolutionSlider.Scroll += ResolutionSlider_Scroll;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 39);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(337, 1);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textureRadio);
            groupBox3.Controls.Add(solidColorRadio);
            groupBox3.Controls.Add(textureSelect);
            groupBox3.Controls.Add(normalMapSelect);
            groupBox3.Controls.Add(objectColorSelect);
            groupBox3.Controls.Add(normalMapCheckbox);
            groupBox3.Location = new Point(3, 481);
            groupBox3.Margin = new Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 4, 3, 4);
            groupBox3.Size = new Size(329, 189);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Object";
            // 
            // textureRadio
            // 
            textureRadio.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textureRadio.AutoSize = true;
            textureRadio.Location = new Point(65, 87);
            textureRadio.Margin = new Padding(3, 4, 3, 4);
            textureRadio.Name = "textureRadio";
            textureRadio.Size = new Size(78, 24);
            textureRadio.TabIndex = 5;
            textureRadio.Text = "Texture";
            textureRadio.UseVisualStyleBackColor = true;
            textureRadio.CheckedChanged += UseTextureRadio_CheckedChanged;
            // 
            // solidColorRadio
            // 
            solidColorRadio.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            solidColorRadio.AutoSize = true;
            solidColorRadio.BackColor = Color.Transparent;
            solidColorRadio.Checked = true;
            solidColorRadio.Location = new Point(65, 36);
            solidColorRadio.Margin = new Padding(3, 4, 3, 4);
            solidColorRadio.Name = "solidColorRadio";
            solidColorRadio.Size = new Size(102, 24);
            solidColorRadio.TabIndex = 5;
            solidColorRadio.TabStop = true;
            solidColorRadio.Text = "Solid color";
            solidColorRadio.UseVisualStyleBackColor = false;
            solidColorRadio.CheckedChanged += UseTextureRadio_CheckedChanged;
            // 
            // textureSelect
            // 
            textureSelect.BackColor = Color.Transparent;
            textureSelect.BackgroundImageLayout = ImageLayout.Stretch;
            textureSelect.BorderStyle = BorderStyle.FixedSingle;
            textureSelect.Cursor = Cursors.Hand;
            textureSelect.Location = new Point(21, 79);
            textureSelect.Margin = new Padding(3, 4, 3, 4);
            textureSelect.Name = "textureSelect";
            textureSelect.Size = new Size(34, 39);
            textureSelect.TabIndex = 4;
            textureSelect.Click += TextureSelect_Click;
            // 
            // normalMapSelect
            // 
            normalMapSelect.BackColor = Color.Transparent;
            normalMapSelect.BackgroundImageLayout = ImageLayout.Stretch;
            normalMapSelect.BorderStyle = BorderStyle.FixedSingle;
            normalMapSelect.Cursor = Cursors.Hand;
            normalMapSelect.Location = new Point(21, 141);
            normalMapSelect.Margin = new Padding(3, 4, 3, 4);
            normalMapSelect.Name = "normalMapSelect";
            normalMapSelect.Size = new Size(34, 39);
            normalMapSelect.TabIndex = 4;
            normalMapSelect.Click += NormalMapSelect_Click;
            // 
            // objectColorSelect
            // 
            objectColorSelect.BackColor = Color.Red;
            objectColorSelect.Cursor = Cursors.Hand;
            objectColorSelect.Location = new Point(21, 29);
            objectColorSelect.Margin = new Padding(3, 4, 3, 4);
            objectColorSelect.Name = "objectColorSelect";
            objectColorSelect.Size = new Size(34, 40);
            objectColorSelect.TabIndex = 4;
            objectColorSelect.Click += ObjectColorSelect_Click;
            // 
            // normalMapCheckbox
            // 
            normalMapCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            normalMapCheckbox.AutoSize = true;
            normalMapCheckbox.Location = new Point(65, 151);
            normalMapCheckbox.Margin = new Padding(3, 4, 3, 4);
            normalMapCheckbox.Name = "normalMapCheckbox";
            normalMapCheckbox.Size = new Size(115, 24);
            normalMapCheckbox.TabIndex = 0;
            normalMapCheckbox.Text = "Normal map";
            normalMapCheckbox.UseVisualStyleBackColor = true;
            normalMapCheckbox.CheckedChanged += NormalMapCheckbox_CheckedChanged;
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
            groupBox5.Location = new Point(3, 276);
            groupBox5.Margin = new Padding(3, 4, 3, 4);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(3, 4, 3, 4);
            groupBox5.Size = new Size(329, 197);
            groupBox5.TabIndex = 6;
            groupBox5.TabStop = false;
            groupBox5.Text = "Lighting";
            // 
            // lightAnimationCheckbox
            // 
            lightAnimationCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lightAnimationCheckbox.AutoSize = true;
            lightAnimationCheckbox.Location = new Point(176, 153);
            lightAnimationCheckbox.Margin = new Padding(3, 4, 3, 4);
            lightAnimationCheckbox.Name = "lightAnimationCheckbox";
            lightAnimationCheckbox.Size = new Size(100, 24);
            lightAnimationCheckbox.TabIndex = 5;
            lightAnimationCheckbox.Text = "Animation";
            lightAnimationCheckbox.UseVisualStyleBackColor = true;
            lightAnimationCheckbox.CheckedChanged += lightAnimationCheckbox_CheckedChanged;
            // 
            // lightColorSelect
            // 
            lightColorSelect.BackColor = Color.White;
            lightColorSelect.Cursor = Cursors.Hand;
            lightColorSelect.Location = new Point(21, 144);
            lightColorSelect.Margin = new Padding(3, 4, 3, 4);
            lightColorSelect.Name = "lightColorSelect";
            lightColorSelect.Size = new Size(34, 40);
            lightColorSelect.TabIndex = 4;
            lightColorSelect.Click += LightColorSelect_Click;
            // 
            // mLabel
            // 
            mLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mLabel.BackColor = Color.Transparent;
            mLabel.Location = new Point(275, 107);
            mLabel.Margin = new Padding(0);
            mLabel.Name = "mLabel";
            mLabel.Size = new Size(50, 20);
            mLabel.TabIndex = 3;
            // 
            // ksLabel
            // 
            ksLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ksLabel.BackColor = Color.Transparent;
            ksLabel.Location = new Point(275, 68);
            ksLabel.Margin = new Padding(0);
            ksLabel.Name = "ksLabel";
            ksLabel.Size = new Size(50, 20);
            ksLabel.TabIndex = 3;
            // 
            // kdLabel
            // 
            kdLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            kdLabel.BackColor = Color.Transparent;
            kdLabel.Location = new Point(275, 32);
            kdLabel.Margin = new Padding(0);
            kdLabel.Name = "kdLabel";
            kdLabel.Size = new Size(50, 20);
            kdLabel.TabIndex = 3;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(62, 155);
            label5.Name = "label5";
            label5.Size = new Size(80, 20);
            label5.TabIndex = 2;
            label5.Text = "Light color";
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Location = new Point(32, 107);
            label11.Name = "label11";
            label11.Size = new Size(22, 20);
            label11.TabIndex = 2;
            label11.Text = "m";
            // 
            // mSlider
            // 
            mSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mSlider.AutoSize = false;
            mSlider.LargeChange = 20;
            mSlider.Location = new Point(59, 104);
            mSlider.Margin = new Padding(3, 4, 3, 4);
            mSlider.Maximum = 100;
            mSlider.Minimum = 1;
            mSlider.Name = "mSlider";
            mSlider.Size = new Size(219, 31);
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
            label9.Location = new Point(32, 68);
            label9.Name = "label9";
            label9.Size = new Size(22, 20);
            label9.TabIndex = 2;
            label9.Text = "ks";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(32, 32);
            label4.Name = "label4";
            label4.Size = new Size(25, 20);
            label4.TabIndex = 2;
            label4.Text = "kd";
            // 
            // ksSlider
            // 
            ksSlider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ksSlider.AutoSize = false;
            ksSlider.LargeChange = 20;
            ksSlider.Location = new Point(59, 67);
            ksSlider.Margin = new Padding(3, 4, 3, 4);
            ksSlider.Maximum = 100;
            ksSlider.Name = "ksSlider";
            ksSlider.Size = new Size(219, 31);
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
            kdSlider.Location = new Point(59, 28);
            kdSlider.Margin = new Padding(3, 4, 3, 4);
            kdSlider.Maximum = 100;
            kdSlider.Name = "kdSlider";
            kdSlider.Size = new Size(219, 31);
            kdSlider.SmallChange = 10;
            kdSlider.TabIndex = 0;
            kdSlider.TickStyle = TickStyle.None;
            kdSlider.Value = 90;
            kdSlider.Scroll += LightingSlider_Scroll;
            // 
            // button1
            // 
            button1.Location = new Point(3, 4);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(137, 31);
            button1.TabIndex = 7;
            button1.Text = "Load surface";
            button1.UseVisualStyleBackColor = true;
            button1.Click += LoadSurfaceButton_Click;
            // 
            // canvas
            // 
            canvas.BackColor = Color.White;
            canvas.Dock = DockStyle.Fill;
            canvas.Location = new Point(0, 0);
            canvas.Margin = new Padding(0);
            canvas.Name = "canvas";
            canvas.Size = new Size(896, 881);
            canvas.TabIndex = 1;
            canvas.TabStop = false;
            canvas.Paint += Canvas_Paint;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1239, 881);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(683, 518);
            Name = "MainWindow";
            Text = "MeshFiller";
            Resize += MainWindow_Resize;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)alphaSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)betaSlider).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox canvas;
        private TrackBar trackBar2;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox groupBox1;
        private Label betaLabel;
        private Label alphaLabel;
        private Label label2;
        private Label label1;
        private TrackBar betaSlider;
        private TrackBar alphaSlider;
        private GroupBox groupBox2;
        private Label resolutionLabel;
        private CheckBox triangulationCheckbox;
        private Label label3;
        private TrackBar resolutionSlider;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox3;
        private RadioButton textureRadio;
        private RadioButton solidColorRadio;
        private Panel textureSelect;
        private Panel normalMapSelect;
        private Panel objectColorSelect;
        private CheckBox normalMapCheckbox;
        private GroupBox groupBox5;
        private CheckBox lightAnimationCheckbox;
        private Panel lightColorSelect;
        private Label mLabel;
        private Label ksLabel;
        private Label kdLabel;
        private Label label5;
        private Label label11;
        private TrackBar mSlider;
        private Label label9;
        private Label label4;
        private TrackBar ksSlider;
        private TrackBar kdSlider;
        private Button button1;
    }
}
