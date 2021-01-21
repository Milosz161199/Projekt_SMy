namespace SerialPortApp
{
    partial class MainForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.baudRateLabel = new System.Windows.Forms.Label();
            this.dataBitsLabel = new System.Windows.Forms.Label();
            this.parityLabel = new System.Windows.Forms.Label();
            this.portNameLabel = new System.Windows.Forms.Label();
            this.stopBitsLabel = new System.Windows.Forms.Label();
            this.baudRateComboBox = new System.Windows.Forms.ComboBox();
            this.serialSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataBitsComboBox = new System.Windows.Forms.ComboBox();
            this.parityComboBox = new System.Windows.Forms.ComboBox();
            this.portNameComboBox = new System.Windows.Forms.ComboBox();
            this.stopBitsComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox_settings = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.tbDataReceive = new System.Windows.Forms.TextBox();
            this.groupBox_receive = new System.Windows.Forms.GroupBox();
            this.rxEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox_transmit = new System.Windows.Forms.GroupBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbDataTransmit = new System.Windows.Forms.TextBox();
            this.groupBox_exceptions = new System.Windows.Forms.GroupBox();
            this.error_label = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabSerialPort = new System.Windows.Forms.TabPage();
            this.tabInput = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_Error = new System.Windows.Forms.TextBox();
            this.button_chart_Stop = new System.Windows.Forms.Button();
            this.button_chart_Start = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl = new System.Windows.Forms.TabPage();
            this.label_type_of_control = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_One_LED = new System.Windows.Forms.TextBox();
            this.textBox_Both_LEDs = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_One = new System.Windows.Forms.Button();
            this.button_Both = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_set_point_one_LED = new System.Windows.Forms.Button();
            this.button_set_point_both_LED = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar_One_LED = new System.Windows.Forms.TrackBar();
            this.trackBar_Both_LEDs = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.serialSettingsBindingSource)).BeginInit();
            this.groupBox_settings.SuspendLayout();
            this.groupBox_receive.SuspendLayout();
            this.groupBox_transmit.SuspendLayout();
            this.groupBox_exceptions.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabSerialPort.SuspendLayout();
            this.tabInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_One_LED)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Both_LEDs)).BeginInit();
            this.SuspendLayout();
            // 
            // baudRateLabel
            // 
            this.baudRateLabel.AutoSize = true;
            this.baudRateLabel.Location = new System.Drawing.Point(10, 59);
            this.baudRateLabel.Name = "baudRateLabel";
            this.baudRateLabel.Size = new System.Drawing.Size(61, 13);
            this.baudRateLabel.TabIndex = 1;
            this.baudRateLabel.Text = "Baud Rate:";
            // 
            // dataBitsLabel
            // 
            this.dataBitsLabel.AutoSize = true;
            this.dataBitsLabel.Location = new System.Drawing.Point(10, 86);
            this.dataBitsLabel.Name = "dataBitsLabel";
            this.dataBitsLabel.Size = new System.Drawing.Size(53, 13);
            this.dataBitsLabel.TabIndex = 3;
            this.dataBitsLabel.Text = "Data Bits:";
            // 
            // parityLabel
            // 
            this.parityLabel.AutoSize = true;
            this.parityLabel.Location = new System.Drawing.Point(10, 113);
            this.parityLabel.Name = "parityLabel";
            this.parityLabel.Size = new System.Drawing.Size(36, 13);
            this.parityLabel.TabIndex = 5;
            this.parityLabel.Text = "Parity:";
            // 
            // portNameLabel
            // 
            this.portNameLabel.AutoSize = true;
            this.portNameLabel.Location = new System.Drawing.Point(10, 32);
            this.portNameLabel.Name = "portNameLabel";
            this.portNameLabel.Size = new System.Drawing.Size(60, 13);
            this.portNameLabel.TabIndex = 7;
            this.portNameLabel.Text = "Port Name:";
            // 
            // stopBitsLabel
            // 
            this.stopBitsLabel.AutoSize = true;
            this.stopBitsLabel.Location = new System.Drawing.Point(10, 140);
            this.stopBitsLabel.Name = "stopBitsLabel";
            this.stopBitsLabel.Size = new System.Drawing.Size(52, 13);
            this.stopBitsLabel.TabIndex = 9;
            this.stopBitsLabel.Text = "Stop Bits:";
            // 
            // baudRateComboBox
            // 
            this.baudRateComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "BaudRate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.baudRateComboBox.FormattingEnabled = true;
            this.baudRateComboBox.Location = new System.Drawing.Point(77, 56);
            this.baudRateComboBox.Name = "baudRateComboBox";
            this.baudRateComboBox.Size = new System.Drawing.Size(153, 21);
            this.baudRateComboBox.TabIndex = 2;
            // 
            // dataBitsComboBox
            // 
            this.dataBitsComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "DataBits", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dataBitsComboBox.FormattingEnabled = true;
            this.dataBitsComboBox.Location = new System.Drawing.Point(77, 83);
            this.dataBitsComboBox.Name = "dataBitsComboBox";
            this.dataBitsComboBox.Size = new System.Drawing.Size(153, 21);
            this.dataBitsComboBox.TabIndex = 4;
            // 
            // parityComboBox
            // 
            this.parityComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "Parity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.parityComboBox.FormattingEnabled = true;
            this.parityComboBox.Location = new System.Drawing.Point(77, 110);
            this.parityComboBox.Name = "parityComboBox";
            this.parityComboBox.Size = new System.Drawing.Size(153, 21);
            this.parityComboBox.TabIndex = 6;
            // 
            // portNameComboBox
            // 
            this.portNameComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "PortName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.portNameComboBox.FormattingEnabled = true;
            this.portNameComboBox.Location = new System.Drawing.Point(77, 29);
            this.portNameComboBox.Name = "portNameComboBox";
            this.portNameComboBox.Size = new System.Drawing.Size(153, 21);
            this.portNameComboBox.TabIndex = 8;
            // 
            // stopBitsComboBox
            // 
            this.stopBitsComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serialSettingsBindingSource, "StopBits", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.stopBitsComboBox.FormattingEnabled = true;
            this.stopBitsComboBox.Location = new System.Drawing.Point(77, 137);
            this.stopBitsComboBox.Name = "stopBitsComboBox";
            this.stopBitsComboBox.Size = new System.Drawing.Size(153, 21);
            this.stopBitsComboBox.TabIndex = 10;
            // 
            // groupBox_settings
            // 
            this.groupBox_settings.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_settings.Controls.Add(this.baudRateComboBox);
            this.groupBox_settings.Controls.Add(this.btnStop);
            this.groupBox_settings.Controls.Add(this.baudRateLabel);
            this.groupBox_settings.Controls.Add(this.btnStart);
            this.groupBox_settings.Controls.Add(this.stopBitsComboBox);
            this.groupBox_settings.Controls.Add(this.stopBitsLabel);
            this.groupBox_settings.Controls.Add(this.dataBitsLabel);
            this.groupBox_settings.Controls.Add(this.portNameComboBox);
            this.groupBox_settings.Controls.Add(this.dataBitsComboBox);
            this.groupBox_settings.Controls.Add(this.portNameLabel);
            this.groupBox_settings.Controls.Add(this.parityLabel);
            this.groupBox_settings.Controls.Add(this.parityComboBox);
            this.groupBox_settings.Location = new System.Drawing.Point(8, 6);
            this.groupBox_settings.Name = "groupBox_settings";
            this.groupBox_settings.Size = new System.Drawing.Size(244, 200);
            this.groupBox_settings.TabIndex = 11;
            this.groupBox_settings.TabStop = false;
            this.groupBox_settings.Text = "Serial Port Settings";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(157, 164);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(73, 23);
            this.btnStop.TabIndex = 12;
            this.btnStop.Text = "Disconnect";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(77, 164);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(74, 23);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "Connect";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tbDataReceive
            // 
            this.tbDataReceive.Location = new System.Drawing.Point(11, 19);
            this.tbDataReceive.Multiline = true;
            this.tbDataReceive.Name = "tbDataReceive";
            this.tbDataReceive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbDataReceive.Size = new System.Drawing.Size(516, 97);
            this.tbDataReceive.TabIndex = 13;
            // 
            // groupBox_receive
            // 
            this.groupBox_receive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_receive.Controls.Add(this.rxEnableCheckBox);
            this.groupBox_receive.Controls.Add(this.btnClear);
            this.groupBox_receive.Controls.Add(this.tbDataReceive);
            this.groupBox_receive.Location = new System.Drawing.Point(9, 212);
            this.groupBox_receive.Name = "groupBox_receive";
            this.groupBox_receive.Size = new System.Drawing.Size(533, 159);
            this.groupBox_receive.TabIndex = 14;
            this.groupBox_receive.TabStop = false;
            this.groupBox_receive.Text = "Receive";
            // 
            // rxEnableCheckBox
            // 
            this.rxEnableCheckBox.AutoSize = true;
            this.rxEnableCheckBox.Checked = true;
            this.rxEnableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rxEnableCheckBox.Location = new System.Drawing.Point(11, 121);
            this.rxEnableCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.rxEnableCheckBox.Name = "rxEnableCheckBox";
            this.rxEnableCheckBox.Size = new System.Drawing.Size(75, 17);
            this.rxEnableCheckBox.TabIndex = 15;
            this.rxEnableCheckBox.Text = "Rx Enable";
            this.rxEnableCheckBox.UseVisualStyleBackColor = true;
            this.rxEnableCheckBox.CheckedChanged += new System.EventHandler(this.rxEnableCheckBox_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(442, 121);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 23);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // groupBox_transmit
            // 
            this.groupBox_transmit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_transmit.Controls.Add(this.btnSend);
            this.groupBox_transmit.Controls.Add(this.tbDataTransmit);
            this.groupBox_transmit.Location = new System.Drawing.Point(8, 369);
            this.groupBox_transmit.Name = "groupBox_transmit";
            this.groupBox_transmit.Size = new System.Drawing.Size(533, 81);
            this.groupBox_transmit.TabIndex = 15;
            this.groupBox_transmit.TabStop = false;
            this.groupBox_transmit.Text = "Transmit";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(442, 45);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(85, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbDataTransmit
            // 
            this.tbDataTransmit.Location = new System.Drawing.Point(12, 19);
            this.tbDataTransmit.Name = "tbDataTransmit";
            this.tbDataTransmit.Size = new System.Drawing.Size(515, 20);
            this.tbDataTransmit.TabIndex = 0;
            // 
            // groupBox_exceptions
            // 
            this.groupBox_exceptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_exceptions.Controls.Add(this.error_label);
            this.groupBox_exceptions.Location = new System.Drawing.Point(258, 6);
            this.groupBox_exceptions.Name = "groupBox_exceptions";
            this.groupBox_exceptions.Size = new System.Drawing.Size(283, 209);
            this.groupBox_exceptions.TabIndex = 16;
            this.groupBox_exceptions.TabStop = false;
            this.groupBox_exceptions.Text = "Exceptions";
            // 
            // error_label
            // 
            this.error_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.error_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.error_label.ForeColor = System.Drawing.Color.Red;
            this.error_label.Location = new System.Drawing.Point(3, 16);
            this.error_label.Name = "error_label";
            this.error_label.Size = new System.Drawing.Size(277, 190);
            this.error_label.TabIndex = 0;
            this.error_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabSerialPort);
            this.tabMain.Controls.Add(this.tabInput);
            this.tabMain.Controls.Add(this.tabControl);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(554, 481);
            this.tabMain.TabIndex = 17;
            // 
            // tabSerialPort
            // 
            this.tabSerialPort.BackColor = System.Drawing.SystemColors.HighlightText;
            this.tabSerialPort.Controls.Add(this.groupBox_settings);
            this.tabSerialPort.Controls.Add(this.groupBox_exceptions);
            this.tabSerialPort.Controls.Add(this.groupBox_transmit);
            this.tabSerialPort.Controls.Add(this.groupBox_receive);
            this.tabSerialPort.Location = new System.Drawing.Point(4, 22);
            this.tabSerialPort.Name = "tabSerialPort";
            this.tabSerialPort.Padding = new System.Windows.Forms.Padding(3);
            this.tabSerialPort.Size = new System.Drawing.Size(546, 455);
            this.tabSerialPort.TabIndex = 0;
            this.tabSerialPort.Text = "Serial Port";
            // 
            // tabInput
            // 
            this.tabInput.Controls.Add(this.label10);
            this.tabInput.Controls.Add(this.label9);
            this.tabInput.Controls.Add(this.textBox_Error);
            this.tabInput.Controls.Add(this.button_chart_Stop);
            this.tabInput.Controls.Add(this.button_chart_Start);
            this.tabInput.Controls.Add(this.chart1);
            this.tabInput.Location = new System.Drawing.Point(4, 22);
            this.tabInput.Margin = new System.Windows.Forms.Padding(2);
            this.tabInput.Name = "tabInput";
            this.tabInput.Padding = new System.Windows.Forms.Padding(2);
            this.tabInput.Size = new System.Drawing.Size(546, 455);
            this.tabInput.TabIndex = 1;
            this.tabInput.Text = "Input plot";
            this.tabInput.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(434, 393);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "[ % ]";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(338, 393);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "ERROR: ";
            // 
            // textBox_Error
            // 
            this.textBox_Error.Enabled = false;
            this.textBox_Error.Location = new System.Drawing.Point(396, 390);
            this.textBox_Error.Name = "textBox_Error";
            this.textBox_Error.Size = new System.Drawing.Size(32, 20);
            this.textBox_Error.TabIndex = 14;
            this.textBox_Error.Text = "0.00";
            // 
            // button_chart_Stop
            // 
            this.button_chart_Stop.BackColor = System.Drawing.Color.Gray;
            this.button_chart_Stop.Location = new System.Drawing.Point(218, 382);
            this.button_chart_Stop.Name = "button_chart_Stop";
            this.button_chart_Stop.Size = new System.Drawing.Size(93, 35);
            this.button_chart_Stop.TabIndex = 2;
            this.button_chart_Stop.Text = "STOP";
            this.button_chart_Stop.UseVisualStyleBackColor = false;
            this.button_chart_Stop.Click += new System.EventHandler(this.button_chart_Stop_Click);
            // 
            // button_chart_Start
            // 
            this.button_chart_Start.BackColor = System.Drawing.Color.Gray;
            this.button_chart_Start.Location = new System.Drawing.Point(89, 382);
            this.button_chart_Start.Name = "button_chart_Start";
            this.button_chart_Start.Size = new System.Drawing.Size(93, 35);
            this.button_chart_Start.TabIndex = 1;
            this.button_chart_Start.Text = "START";
            this.button_chart_Start.UseVisualStyleBackColor = false;
            this.button_chart_Start.Click += new System.EventHandler(this.button_chart_Start_Click);
            // 
            // chart1
            // 
            this.chart1.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.BottomLeft;
            chartArea2.AxisX.LabelStyle.Format = "0.0";
            chartArea2.AxisX.Maximum = 10D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.Title = "Time [s]";
            chartArea2.AxisY.Interval = 20D;
            chartArea2.AxisY.Maximum = 200D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.AxisY.Title = "Light intensity [lux]";
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(-9, 0);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Output value";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(559, 373);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.label_type_of_control);
            this.tabControl.Controls.Add(this.label8);
            this.tabControl.Controls.Add(this.textBox_One_LED);
            this.tabControl.Controls.Add(this.textBox_Both_LEDs);
            this.tabControl.Controls.Add(this.label7);
            this.tabControl.Controls.Add(this.label6);
            this.tabControl.Controls.Add(this.label5);
            this.tabControl.Controls.Add(this.button_One);
            this.tabControl.Controls.Add(this.button_Both);
            this.tabControl.Controls.Add(this.label4);
            this.tabControl.Controls.Add(this.label3);
            this.tabControl.Controls.Add(this.button_set_point_one_LED);
            this.tabControl.Controls.Add(this.button_set_point_both_LED);
            this.tabControl.Controls.Add(this.label2);
            this.tabControl.Controls.Add(this.label1);
            this.tabControl.Controls.Add(this.trackBar_One_LED);
            this.tabControl.Controls.Add(this.trackBar_Both_LEDs);
            this.tabControl.Location = new System.Drawing.Point(4, 22);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Windows.Forms.Padding(2);
            this.tabControl.Size = new System.Drawing.Size(546, 455);
            this.tabControl.TabIndex = 2;
            this.tabControl.Text = "Output control";
            this.tabControl.UseVisualStyleBackColor = true;
            // 
            // label_type_of_control
            // 
            this.label_type_of_control.AutoSize = true;
            this.label_type_of_control.Location = new System.Drawing.Point(200, 98);
            this.label_type_of_control.Name = "label_type_of_control";
            this.label_type_of_control.Size = new System.Drawing.Size(0, 13);
            this.label_type_of_control.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(110, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Type of control: ";
            // 
            // textBox_One_LED
            // 
            this.textBox_One_LED.Enabled = false;
            this.textBox_One_LED.Location = new System.Drawing.Point(68, 363);
            this.textBox_One_LED.Name = "textBox_One_LED";
            this.textBox_One_LED.Size = new System.Drawing.Size(40, 20);
            this.textBox_One_LED.TabIndex = 14;
            this.textBox_One_LED.Text = "2";
            // 
            // textBox_Both_LEDs
            // 
            this.textBox_Both_LEDs.Enabled = false;
            this.textBox_Both_LEDs.Location = new System.Drawing.Point(68, 219);
            this.textBox_Both_LEDs.Name = "textBox_Both_LEDs";
            this.textBox_Both_LEDs.Size = new System.Drawing.Size(40, 20);
            this.textBox_Both_LEDs.TabIndex = 13;
            this.textBox_Both_LEDs.Text = "2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(114, 366);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "[ lux ]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(114, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "[ lux ]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(191, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "CONTROL ONE OR BOTH LEDs";
            // 
            // button_One
            // 
            this.button_One.BackColor = System.Drawing.Color.Gray;
            this.button_One.Location = new System.Drawing.Point(280, 35);
            this.button_One.Name = "button_One";
            this.button_One.Size = new System.Drawing.Size(100, 40);
            this.button_One.TabIndex = 9;
            this.button_One.Text = "ONE LED";
            this.button_One.UseVisualStyleBackColor = false;
            this.button_One.Click += new System.EventHandler(this.button_One_Click);
            // 
            // button_Both
            // 
            this.button_Both.BackColor = System.Drawing.Color.Gray;
            this.button_Both.Location = new System.Drawing.Point(165, 35);
            this.button_Both.Name = "button_Both";
            this.button_Both.Size = new System.Drawing.Size(100, 40);
            this.button_Both.TabIndex = 8;
            this.button_Both.Text = "BOTH LEDS";
            this.button_Both.UseVisualStyleBackColor = false;
            this.button_Both.Click += new System.EventHandler(this.button_Both_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 366);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "VALUE: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "VALUE: ";
            // 
            // button_set_point_one_LED
            // 
            this.button_set_point_one_LED.Location = new System.Drawing.Point(463, 356);
            this.button_set_point_one_LED.Name = "button_set_point_one_LED";
            this.button_set_point_one_LED.Size = new System.Drawing.Size(75, 23);
            this.button_set_point_one_LED.TabIndex = 5;
            this.button_set_point_one_LED.Text = "SET";
            this.button_set_point_one_LED.UseVisualStyleBackColor = true;
            this.button_set_point_one_LED.Click += new System.EventHandler(this.button_set_point_one_LED_Click);
            // 
            // button_set_point_both_LED
            // 
            this.button_set_point_both_LED.Location = new System.Drawing.Point(463, 211);
            this.button_set_point_both_LED.Name = "button_set_point_both_LED";
            this.button_set_point_both_LED.Size = new System.Drawing.Size(75, 23);
            this.button_set_point_both_LED.TabIndex = 4;
            this.button_set_point_both_LED.Text = "SET";
            this.button_set_point_both_LED.UseVisualStyleBackColor = true;
            this.button_set_point_both_LED.Click += new System.EventHandler(this.button_set_point_both_LED_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "ONE LED SET_POINT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "BOTH LEDs SET_POINT";
            // 
            // trackBar_One_LED
            // 
            this.trackBar_One_LED.Location = new System.Drawing.Point(17, 297);
            this.trackBar_One_LED.Maximum = 110;
            this.trackBar_One_LED.Minimum = 2;
            this.trackBar_One_LED.Name = "trackBar_One_LED";
            this.trackBar_One_LED.Size = new System.Drawing.Size(521, 45);
            this.trackBar_One_LED.TabIndex = 1;
            this.trackBar_One_LED.Value = 2;
            this.trackBar_One_LED.Scroll += new System.EventHandler(this.trackBar_One_LED_Scroll);
            // 
            // trackBar_Both_LEDs
            // 
            this.trackBar_Both_LEDs.Location = new System.Drawing.Point(17, 156);
            this.trackBar_Both_LEDs.Maximum = 180;
            this.trackBar_Both_LEDs.Minimum = 2;
            this.trackBar_Both_LEDs.Name = "trackBar_Both_LEDs";
            this.trackBar_Both_LEDs.Size = new System.Drawing.Size(521, 45);
            this.trackBar_Both_LEDs.TabIndex = 0;
            this.trackBar_Both_LEDs.Value = 2;
            this.trackBar_Both_LEDs.Scroll += new System.EventHandler(this.trackBar_Both_LEDs_Scroll);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(554, 481);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "C# Serial Port App";
            ((System.ComponentModel.ISupportInitialize)(this.serialSettingsBindingSource)).EndInit();
            this.groupBox_settings.ResumeLayout(false);
            this.groupBox_settings.PerformLayout();
            this.groupBox_receive.ResumeLayout(false);
            this.groupBox_receive.PerformLayout();
            this.groupBox_transmit.ResumeLayout(false);
            this.groupBox_transmit.PerformLayout();
            this.groupBox_exceptions.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabSerialPort.ResumeLayout(false);
            this.tabInput.ResumeLayout(false);
            this.tabInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_One_LED)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Both_LEDs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource serialSettingsBindingSource;
        private System.Windows.Forms.ComboBox baudRateComboBox;
        private System.Windows.Forms.ComboBox dataBitsComboBox;
        private System.Windows.Forms.ComboBox parityComboBox;
        private System.Windows.Forms.ComboBox portNameComboBox;
        private System.Windows.Forms.ComboBox stopBitsComboBox;
        private System.Windows.Forms.GroupBox groupBox_settings;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox tbDataReceive;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label baudRateLabel;
        private System.Windows.Forms.Label dataBitsLabel;
        private System.Windows.Forms.Label parityLabel;
        private System.Windows.Forms.Label portNameLabel;
        private System.Windows.Forms.Label stopBitsLabel;
        private System.Windows.Forms.GroupBox groupBox_receive;
        private System.Windows.Forms.GroupBox groupBox_transmit;
        private System.Windows.Forms.TextBox tbDataTransmit;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox groupBox_exceptions;
        private System.Windows.Forms.Label error_label;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabSerialPort;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TabPage tabInput;
        private System.Windows.Forms.TabPage tabControl;
        private System.Windows.Forms.CheckBox rxEnableCheckBox;
        private System.Windows.Forms.Button button_chart_Stop;
        private System.Windows.Forms.Button button_chart_Start;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button_set_point_one_LED;
        private System.Windows.Forms.Button button_set_point_both_LED;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar_One_LED;
        private System.Windows.Forms.TrackBar trackBar_Both_LEDs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_One;
        private System.Windows.Forms.Button button_Both;
        private System.Windows.Forms.TextBox textBox_One_LED;
        private System.Windows.Forms.TextBox textBox_Both_LEDs;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_type_of_control;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_Error;
    }
}

