namespace PxGraphTest
{
	partial class Form1
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			this.editPanel = new System.Windows.Forms.Panel();
			this.showLegend = new System.Windows.Forms.CheckBox();
			this.showTitle = new System.Windows.Forms.CheckBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.showCopyright = new System.Windows.Forms.CheckBox();
			this.chartTypeAdd = new System.Windows.Forms.Button();
			this.chartType = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.autoPivot = new System.Windows.Forms.CheckBox();
			this.adjust = new System.Windows.Forms.CheckBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.chartSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.chartTypesLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.dstChart = new PCAxis.Charting.DstChart();
			this.pxChart = new PCAxis.Charting.PxChart();
			this.msChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.infoLabel = new System.Windows.Forms.Label();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.editPanel.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dstChart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pxChart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.msChart)).BeginInit();
			this.SuspendLayout();
			// 
			// editPanel
			// 
			this.editPanel.Controls.Add(this.listBox1);
			this.editPanel.Controls.Add(this.showLegend);
			this.editPanel.Controls.Add(this.showTitle);
			this.editPanel.Controls.Add(this.button3);
			this.editPanel.Controls.Add(this.button2);
			this.editPanel.Controls.Add(this.showCopyright);
			this.editPanel.Controls.Add(this.chartTypeAdd);
			this.editPanel.Controls.Add(this.chartType);
			this.editPanel.Controls.Add(this.button1);
			this.editPanel.Controls.Add(this.autoPivot);
			this.editPanel.Controls.Add(this.adjust);
			this.editPanel.Enabled = false;
			this.editPanel.Location = new System.Drawing.Point(12, 27);
			this.editPanel.Name = "editPanel";
			this.editPanel.Size = new System.Drawing.Size(799, 31);
			this.editPanel.TabIndex = 7;
			// 
			// showLegend
			// 
			this.showLegend.AutoSize = true;
			this.showLegend.Checked = true;
			this.showLegend.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showLegend.Location = new System.Drawing.Point(624, 5);
			this.showLegend.Name = "showLegend";
			this.showLegend.Size = new System.Drawing.Size(62, 17);
			this.showLegend.TabIndex = 55;
			this.showLegend.Text = "&Legend";
			this.showLegend.UseVisualStyleBackColor = true;
			this.showLegend.CheckedChanged += new System.EventHandler(this.showLegend_CheckedChanged);
			// 
			// showTitle
			// 
			this.showTitle.AutoSize = true;
			this.showTitle.Checked = true;
			this.showTitle.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showTitle.Location = new System.Drawing.Point(572, 5);
			this.showTitle.Name = "showTitle";
			this.showTitle.Size = new System.Drawing.Size(46, 17);
			this.showTitle.TabIndex = 54;
			this.showTitle.Text = "&Titel";
			this.showTitle.UseVisualStyleBackColor = true;
			this.showTitle.CheckedChanged += new System.EventHandler(this.showTitle_CheckedChanged);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(692, 1);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 53;
			this.button3.Text = "&Gem";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.Location = new System.Drawing.Point(215, 0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(61, 23);
			this.button2.TabIndex = 52;
			this.button2.Text = "Dst&Chart";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// showCopyright
			// 
			this.showCopyright.AutoSize = true;
			this.showCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.showCopyright.Location = new System.Drawing.Point(529, 3);
			this.showCopyright.Name = "showCopyright";
			this.showCopyright.Size = new System.Drawing.Size(37, 21);
			this.showCopyright.TabIndex = 51;
			this.showCopyright.Text = "©";
			this.showCopyright.UseVisualStyleBackColor = true;
			this.showCopyright.CheckedChanged += new System.EventHandler(this.showCopyright_CheckedChanged);
			// 
			// chartTypeAdd
			// 
			this.chartTypeAdd.Location = new System.Drawing.Point(157, 1);
			this.chartTypeAdd.Name = "chartTypeAdd";
			this.chartTypeAdd.Size = new System.Drawing.Size(23, 23);
			this.chartTypeAdd.TabIndex = 12;
			this.chartTypeAdd.Text = "+";
			this.chartTypeAdd.UseVisualStyleBackColor = true;
			this.chartTypeAdd.Click += new System.EventHandler(this.chartTypeAdd_Click);
			// 
			// chartType
			// 
			this.chartType.FormattingEnabled = true;
			this.chartType.Location = new System.Drawing.Point(0, 3);
			this.chartType.Name = "chartType";
			this.chartType.Size = new System.Drawing.Size(151, 21);
			this.chartType.TabIndex = 10;
			this.chartType.SelectedIndexChanged += new System.EventHandler(this.chartType_SelectedIndexChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(186, 1);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(23, 23);
			this.button1.TabIndex = 14;
			this.button1.Text = "C";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// autoPivot
			// 
			this.autoPivot.AutoSize = true;
			this.autoPivot.Checked = true;
			this.autoPivot.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autoPivot.Location = new System.Drawing.Point(282, 5);
			this.autoPivot.Name = "autoPivot";
			this.autoPivot.Size = new System.Drawing.Size(71, 17);
			this.autoPivot.TabIndex = 30;
			this.autoPivot.Text = "Auto&pivot";
			this.autoPivot.UseVisualStyleBackColor = true;
			this.autoPivot.CheckedChanged += new System.EventHandler(this.autoPivot_CheckedChanged);
			// 
			// adjust
			// 
			this.adjust.AutoSize = true;
			this.adjust.Checked = true;
			this.adjust.CheckState = System.Windows.Forms.CheckState.Checked;
			this.adjust.Location = new System.Drawing.Point(440, 5);
			this.adjust.Name = "adjust";
			this.adjust.Size = new System.Drawing.Size(83, 17);
			this.adjust.TabIndex = 50;
			this.adjust.Text = "&Juster akser";
			this.adjust.UseVisualStyleBackColor = true;
			this.adjust.CheckedChanged += new System.EventHandler(this.adjust_CheckedChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip1.Size = new System.Drawing.Size(859, 25);
			this.toolStrip1.TabIndex = 9;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// openToolStripButton
			// 
			this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
			this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripButton.Name = "openToolStripButton";
			this.openToolStripButton.Size = new System.Drawing.Size(49, 22);
			this.openToolStripButton.Text = "&Åbn";
			this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chartSizeLabel,
            this.chartTypesLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 483);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(859, 22);
			this.statusStrip1.TabIndex = 12;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// chartSizeLabel
			// 
			this.chartSizeLabel.BackColor = System.Drawing.Color.Transparent;
			this.chartSizeLabel.Name = "chartSizeLabel";
			this.chartSizeLabel.Size = new System.Drawing.Size(0, 17);
			this.chartSizeLabel.Click += new System.EventHandler(this.chartSizeLabel_Click);
			// 
			// chartTypesLabel
			// 
			this.chartTypesLabel.BackColor = System.Drawing.Color.Transparent;
			this.chartTypesLabel.Name = "chartTypesLabel";
			this.chartTypesLabel.Size = new System.Drawing.Size(12, 17);
			this.chartTypesLabel.Text = "-";
			// 
			// dstChart
			// 
			this.dstChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			chartArea1.AxisX.Interval = 1D;
			chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial Narrow", 9F);
			chartArea1.AxisX.LabelStyle.Format = "N";
			chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(191)))), ((int)(((byte)(198)))));
			chartArea1.AxisX.MajorGrid.Enabled = false;
			chartArea1.AxisX.MajorTickMark.Enabled = false;
			chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial Narrow", 9F);
			chartArea1.AxisY.LabelStyle.Format = "N";
			chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(191)))), ((int)(((byte)(198)))));
			chartArea1.AxisY.LineWidth = 0;
			chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(191)))), ((int)(((byte)(198)))));
			chartArea1.AxisY.MajorTickMark.Enabled = false;
			chartArea1.Name = "Main";
			chartArea1.Position.Auto = false;
			chartArea1.Position.Height = 80F;
			chartArea1.Position.Width = 98F;
			chartArea1.Position.Y = 18F;
			chartArea2.Name = "ChartArea1";
			this.dstChart.ChartAreas.Add(chartArea1);
			this.dstChart.ChartAreas.Add(chartArea2);
			this.dstChart.LegendHeightAsPctOfChart = null;
			this.dstChart.LegendHeightInPixels = null;
			legend1.Font = new System.Drawing.Font("Arial Narrow", 9F);
			legend1.IsTextAutoFit = false;
			legend1.LegendItemOrder = System.Windows.Forms.DataVisualization.Charting.LegendItemOrder.ReversedSeriesOrder;
			legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
			legend1.Name = "Legend";
			legend1.Position.Auto = false;
			legend1.Position.Height = 10F;
			legend1.Position.Width = 98F;
			legend1.Position.Y = 8F;
			legend2.Name = "Legend1";
			this.dstChart.Legends.Add(legend1);
			this.dstChart.Legends.Add(legend2);
			this.dstChart.Location = new System.Drawing.Point(12, 64);
			this.dstChart.Name = "dstChart";
			this.dstChart.PieLabelStyle = "Disabled";
			this.dstChart.PXMeta = null;
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.dstChart.Series.Add(series1);
			this.dstChart.Size = new System.Drawing.Size(835, 416);
			this.dstChart.TabIndex = 15;
			this.dstChart.Text = "dstChart";
			this.dstChart.Title = "";
			title1.Alignment = System.Drawing.ContentAlignment.TopLeft;
			title1.Font = new System.Drawing.Font("Arial Narrow", 11F);
			title1.Name = "Title1";
			title1.Position.Auto = false;
			title1.Position.Height = 8F;
			title1.Position.Width = 100F;
			this.dstChart.Titles.Add(title1);
			//this.dstChart.Units = "";
			// 
			// pxChart
			// 
			this.pxChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			chartArea3.AxisX.Interval = 1D;
			chartArea3.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 9F);
			chartArea3.AxisX.LabelStyle.Format = "N";
			chartArea3.AxisX.MajorGrid.Enabled = false;
			chartArea3.AxisX.MajorTickMark.Enabled = false;
			chartArea3.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 9F);
			chartArea3.AxisY.LabelStyle.Format = "N";
			chartArea3.AxisY.LineWidth = 0;
			chartArea3.AxisY.MajorTickMark.Enabled = false;
			chartArea3.Name = "Main";
			chartArea3.Position.Auto = false;
			chartArea3.Position.Height = 63F;
			chartArea3.Position.Width = 100F;
			chartArea3.Position.Y = 12F;
			chartArea4.Name = "ChartArea1";
			this.pxChart.ChartAreas.Add(chartArea3);
			this.pxChart.ChartAreas.Add(chartArea4);
			this.pxChart.LegendHeightAsPctOfChart = null;
			this.pxChart.LegendHeightInPixels = null;
			legend3.LegendItemOrder = System.Windows.Forms.DataVisualization.Charting.LegendItemOrder.ReversedSeriesOrder;
			legend3.Name = "Legend";
			legend3.Position.Auto = false;
			legend3.Position.Height = 25F;
			legend3.Position.Width = 100F;
			legend3.Position.Y = 75F;
			legend4.Name = "Legend1";
			this.pxChart.Legends.Add(legend3);
			this.pxChart.Legends.Add(legend4);
			this.pxChart.Location = new System.Drawing.Point(12, 64);
			this.pxChart.Name = "pxChart";
			this.pxChart.PieLabelStyle = "Disabled";
			series2.ChartArea = "ChartArea1";
			series2.Legend = "Legend1";
			series2.Name = "Series1";
			this.pxChart.Series.Add(series2);
			this.pxChart.Size = new System.Drawing.Size(835, 416);
			this.pxChart.TabIndex = 13;
			this.pxChart.Text = "pxChart1";
			this.pxChart.Title = "";
			title2.Alignment = System.Drawing.ContentAlignment.TopLeft;
			title2.Font = new System.Drawing.Font("Arial", 11F);
			title2.Name = "Title1";
			title2.Position.Auto = false;
			title2.Position.Height = 14F;
			title2.Position.Width = 100F;
			this.pxChart.Titles.Add(title2);
			// 
			// msChart
			// 
			this.msChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			chartArea5.Name = "ChartArea1";
			this.msChart.ChartAreas.Add(chartArea5);
			legend5.Name = "Legend1";
			this.msChart.Legends.Add(legend5);
			this.msChart.Location = new System.Drawing.Point(12, 64);
			this.msChart.Name = "msChart";
			this.msChart.Size = new System.Drawing.Size(835, 401);
			this.msChart.TabIndex = 16;
			this.msChart.Visible = false;
			// 
			// infoLabel
			// 
			this.infoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.infoLabel.Location = new System.Drawing.Point(12, 64);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(835, 416);
			this.infoLabel.TabIndex = 14;
			this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Items.AddRange(new object[] {
            "Descending",
            "Ascending"});
			this.listBox1.Location = new System.Drawing.Point(358, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(66, 30);
			this.listBox1.TabIndex = 17;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(859, 505);
			this.Controls.Add(this.infoLabel);
			this.Controls.Add(this.dstChart);
			this.Controls.Add(this.pxChart);
			this.Controls.Add(this.msChart);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.editPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.editPanel.ResumeLayout(false);
			this.editPanel.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dstChart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pxChart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.msChart)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel editPanel;
		private System.Windows.Forms.CheckBox adjust;
		private System.Windows.Forms.ComboBox chartType;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton openToolStripButton;
		private System.Windows.Forms.CheckBox autoPivot;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel chartSizeLabel;
		private System.Windows.Forms.Button chartTypeAdd;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ToolStripStatusLabel chartTypesLabel;
		private PCAxis.Charting.PxChart pxChart;
		private PCAxis.Charting.DstChart dstChart;
		private System.Windows.Forms.DataVisualization.Charting.Chart msChart;
		private System.Windows.Forms.CheckBox showCopyright;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.CheckBox showTitle;
		private System.Windows.Forms.CheckBox showLegend;
		private System.Windows.Forms.ListBox listBox1;
	}
}

