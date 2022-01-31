namespace ANNShell
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.nnChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.nnProgressBar = new System.Windows.Forms.ProgressBar();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.Run4000 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.checkBoxGraph = new System.Windows.Forms.CheckBox();
            this.textBoxETA = new System.Windows.Forms.TextBox();
            this.textBoxHidden = new System.Windows.Forms.TextBox();
            this.textBoxEpocs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxHidden1 = new System.Windows.Forms.TextBox();
            this.textBoxHidden2 = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nnChart)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1318, 7);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Quit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1317, 115);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 1;
            this.button2.Text = "Test 1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnTest1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(13, 254);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(735, 580);
            this.textBox1.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1317, 151);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 3;
            this.button3.Text = "Test 2";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnTest2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(13, 6);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(140, 28);
            this.button4.TabIndex = 4;
            this.button4.Text = "Run Cancer";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnIris_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(355, 6);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(844, 196);
            this.textBox2.TabIndex = 5;
            // 
            // nnChart
            // 
            this.nnChart.BorderlineColor = System.Drawing.Color.Gray;
            this.nnChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.nnChart.ChartAreas.Add(chartArea1);
            legend1.BorderColor = System.Drawing.Color.Black;
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.IsDockedInsideChartArea = false;
            legend1.Name = "Legend1";
            legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
            this.nnChart.Legends.Add(legend1);
            this.nnChart.Location = new System.Drawing.Point(756, 254);
            this.nnChart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nnChart.Name = "nnChart";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series1.Legend = "Legend1";
            series1.Name = "Training";
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            series2.Legend = "Legend1";
            series2.Name = "Testing";
            this.nnChart.Series.Add(series1);
            this.nnChart.Series.Add(series2);
            this.nnChart.Size = new System.Drawing.Size(663, 580);
            this.nnChart.TabIndex = 6;
            this.nnChart.Text = "chart1";
            title1.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            title1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            title1.Name = "Epochs";
            title1.Text = "Epochs";
            title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            title2.Name = "Title1";
            title2.Text = "Accuracy %";
            this.nnChart.Titles.Add(title1);
            this.nnChart.Titles.Add(title2);
            // 
            // nnProgressBar
            // 
            this.nnProgressBar.Location = new System.Drawing.Point(13, 221);
            this.nnProgressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nnProgressBar.Name = "nnProgressBar";
            this.nnProgressBar.Size = new System.Drawing.Size(1406, 27);
            this.nnProgressBar.TabIndex = 7;
            this.nnProgressBar.Click += new System.EventHandler(this.nnProgressBar_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1307, 41);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(111, 28);
            this.button5.TabIndex = 8;
            this.button5.Text = "Run Iris";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(13, 112);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(140, 28);
            this.button6.TabIndex = 8;
            this.button6.Text = "Run Abalone";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Run4000
            // 
            this.Run4000.Location = new System.Drawing.Point(1306, 76);
            this.Run4000.Margin = new System.Windows.Forms.Padding(4);
            this.Run4000.Name = "Run4000";
            this.Run4000.Size = new System.Drawing.Size(111, 28);
            this.Run4000.TabIndex = 8;
            this.Run4000.Text = "Run 4000e10";
            this.Run4000.UseVisualStyleBackColor = true;
            this.Run4000.Click += new System.EventHandler(this.Run4000_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(1318, 187);
            this.button8.Margin = new System.Windows.Forms.Padding(4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(100, 28);
            this.button8.TabIndex = 9;
            this.button8.Text = "Test 3";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // checkBoxGraph
            // 
            this.checkBoxGraph.AutoSize = true;
            this.checkBoxGraph.Checked = true;
            this.checkBoxGraph.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGraph.Location = new System.Drawing.Point(236, 8);
            this.checkBoxGraph.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxGraph.Name = "checkBoxGraph";
            this.checkBoxGraph.Size = new System.Drawing.Size(70, 21);
            this.checkBoxGraph.TabIndex = 11;
            this.checkBoxGraph.Text = "Graph";
            this.checkBoxGraph.UseVisualStyleBackColor = true;
            this.checkBoxGraph.CheckedChanged += new System.EventHandler(this.checkBoxGraph_CheckedChanged);
            // 
            // textBoxETA
            // 
            this.textBoxETA.Location = new System.Drawing.Point(285, 70);
            this.textBoxETA.Name = "textBoxETA";
            this.textBoxETA.Size = new System.Drawing.Size(64, 22);
            this.textBoxETA.TabIndex = 12;
            this.textBoxETA.Text = "0.1";
            this.textBoxETA.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBoxHidden
            // 
            this.textBoxHidden.Location = new System.Drawing.Point(285, 42);
            this.textBoxHidden.Name = "textBoxHidden";
            this.textBoxHidden.Size = new System.Drawing.Size(64, 22);
            this.textBoxHidden.TabIndex = 13;
            this.textBoxHidden.Text = "7";
            this.textBoxHidden.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // textBoxEpocs
            // 
            this.textBoxEpocs.Location = new System.Drawing.Point(285, 98);
            this.textBoxEpocs.Name = "textBoxEpocs";
            this.textBoxEpocs.Size = new System.Drawing.Size(64, 22);
            this.textBoxEpocs.TabIndex = 14;
            this.textBoxEpocs.Text = "2000";
            this.textBoxEpocs.TextChanged += new System.EventHandler(this.textBoxOutput_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(198, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "HIdden";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "ETA";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 17;
            this.label3.Text = "Epocs";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(182, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 18;
            this.label4.Text = "Random1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(182, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "Random2";
            // 
            // textBoxHidden1
            // 
            this.textBoxHidden1.Location = new System.Drawing.Point(285, 126);
            this.textBoxHidden1.Name = "textBoxHidden1";
            this.textBoxHidden1.Size = new System.Drawing.Size(64, 22);
            this.textBoxHidden1.TabIndex = 20;
            this.textBoxHidden1.Text = "102";
            this.textBoxHidden1.TextChanged += new System.EventHandler(this.textBox3_TextChanged_1);
            // 
            // textBoxHidden2
            // 
            this.textBoxHidden2.Location = new System.Drawing.Point(285, 154);
            this.textBoxHidden2.Name = "textBoxHidden2";
            this.textBoxHidden2.Size = new System.Drawing.Size(64, 22);
            this.textBoxHidden2.TabIndex = 21;
            this.textBoxHidden2.Text = "103";
            this.textBoxHidden2.TextChanged += new System.EventHandler(this.textBoxHidden2_TextChanged);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(13, 41);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(140, 29);
            this.button7.TabIndex = 22;
            this.button7.Text = "Run Card";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(13, 76);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(140, 29);
            this.button9.TabIndex = 23;
            this.button9.Text = "Run AbaloneBaby";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(13, 144);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(141, 27);
            this.button10.TabIndex = 24;
            this.button10.Text = "Run Wine";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(13, 171);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(141, 31);
            this.button11.TabIndex = 25;
            this.button11.Text = "Run Wine (66-33)";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1430, 845);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.textBoxHidden2);
            this.Controls.Add(this.textBoxHidden1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxEpocs);
            this.Controls.Add(this.textBoxHidden);
            this.Controls.Add(this.textBoxETA);
            this.Controls.Add(this.checkBoxGraph);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.Run4000);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.nnProgressBar);
            this.Controls.Add(this.nnChart);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Ann Test v5.0.00b";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nnChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart nnChart;
        private System.Windows.Forms.ProgressBar nnProgressBar;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button Run4000;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.CheckBox checkBoxGraph;
        private System.Windows.Forms.TextBox textBoxInputs;
        private System.Windows.Forms.TextBox textBoxHiddens;
        private System.Windows.Forms.TextBox textBoxOutputs;
        private System.Windows.Forms.TextBox textBoxETA;
        private System.Windows.Forms.TextBox textBoxHidden;
        private System.Windows.Forms.TextBox textBoxEpocs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxHidden1;
        private System.Windows.Forms.TextBox textBoxHidden2;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
    }
}

