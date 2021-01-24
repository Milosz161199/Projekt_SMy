/**
*   @author Adrian Wojcik
*   @file MainForm.cs
*   @date 02.11.17
*   @brief Main form class.
*   Based on project by Amund Gjersøe (www.codeproject.com/Articles/75770/Basic-serial-port-listening-application)
*/

/*
 * System libraries
 */
using System;
using System.Text;
using System.Windows.Forms;
using SerialPortApp.Serial;
using System.Globalization;
using System.Diagnostics;


namespace SerialPortApp
{
    /*
    * Main form class. Inherit from form Form class.
    * Partial definition -  remider of the class defined in 
    * automatically generated file MainForm.designer.cs
    */
    public partial class MainForm : Form
    {
        //! Default constructor.
        public MainForm()
        {
            InitializeComponent();
            UserInitialization();
        }

        #region Fields

        SerialPortManager _spManager; /** Custom serial port manager class object. */
        const int _spMsgSize = 3;     // size of serial port message


        /* RAGE OF CONTROL */
        int min_val = 2;              // [lux]
        int max_val = 110;            // [lux]

        /* TYPE OF CONTROL */
        bool picked_type_of_control_One_LED = true;     // just one LED
        bool picked_type_of_control_Both_LEDs = false;  // both of LEDs

        // control set vaule and control error
        UInt16 set_val = 2;              // [lux]
        float error_val = 0.0f;          // [%]
        double outValue = 0.0;           // [lux]

        // Set vaule from track bar scroll
        UInt16 _setValue_One_LED;        // just one LED
        UInt16 _setValue_Both_LEDs;      // both of LEDs

        // Control output value as a string
        string _outValue_Str;

        // Variables for drawing chart
        double _plotTimeStep = 0.1;       // [s]
        double _plotTime = 0.0;           // [s]
        const double _plotTimeMax = 10.0; // [s]

        // Variables for file operations
        bool send_just_text = true;
        string path_to_csv_file = "C:/Users/Milosz/STM32CubeIDE/workspace_1.4.0/Projekt_STM32/Aplikacja_Desk_C/SerialPortApp/bin/x64/Debug";
        string file_name = "data.csv";

        #endregion

        #region Event handlers

        /*
         * Main form window closing event handling function.
         * @param sender - contains a reference to the control/object that raised the event.
         * @param e - contains the form closing event data.
         */
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _spManager.Dispose();
        }

        /*
        * New serial port data recived event handlig function. Update of "tbDataReceive" text box.
        * @param sender - contains a reference to the control/object that raised the event.
        * @param e - contains the serial port event data.
        */
        void _spManager_NewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {
            if (this.InvokeRequired)
            {
                // Using this.Invoke causes deadlock when closing serial port, and BeginInvoke is good practice anyway.
                this.BeginInvoke(new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved), new object[] { sender, e });
                return;
            }

            int maxTextLength = 1000; // maximum text length in text box
            if (tbDataReceive.TextLength > maxTextLength)
                tbDataReceive.Text = tbDataReceive.Text.Remove(0, tbDataReceive.TextLength - maxTextLength);

            // Byte array to string
            string str = Encoding.ASCII.GetString(e.Data);

            tbDataReceive.AppendText(str);
            tbDataReceive.ScrollToCaret();
        }

        /*
        * Error handling function. Display message in groupBox "Exceptions".
        * @param sender - contains a reference to the control/object that raised the event.
        * @param e - contains the event data.
        */
        private void _spManager_ErrorHandler(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                // Using this.Invoke causes deadlock when closing serial port, and BeginInvoke is good practice anyway.
                this.BeginInvoke(new EventHandler<EventArgs>(_spManager_ErrorHandler), new object[] { sender, e });
                return;
            }
            error_label.Text = ((Exception)sender).Message;
        }

        /*
        * Handles the "Connect"-buttom click event
        * @param sender - contains a reference to the control/object that raised the event.
        * @param e - contains the event data.
        */
        private void btnStart_Click(object sender, EventArgs e)
        {
            Connect();
        }

        /*
        * Handles the "Diconnect"-buttom click event
        * @param sender - contains a reference to the control/object that raised the event.
        * @param e - contains the event data.
        */
        private void btnStop_Click(object sender, EventArgs e)
        {
            Disonnect();
        }

        /*
        * Handles the "Send"-buttom click event
        * @param sender - contains a reference to the control/object that raised the event.
        * @param e - contains the event data.
        */
        private void btnSend_Click(object sender, EventArgs e)
        {
            _spManager.Send(tbDataTransmit.Text);
        }

        /*
        * Handles the "Clear"-buttom click event
        * @param sender - contains a reference to the control/object that raised the event.
        * @param e - contains the event data.
        */
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbDataReceive.Clear();
        }

        /*
         * Receive text box 'Rx Enable' check box click event method.
         * @param sender - contains a reference to the control/object that raised the event.
         * @param e - contains the event data.
         */
        private void rxEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (rxEnableCheckBox.Checked)
                RxTextBoxEnable();
            else
                RxTextBoxDisable();
        }

        #endregion

        #region Methods

        /*
         * User custom initialization.
         */
        private void UserInitialization()
        {
            // New serial port manager
            _spManager = new SerialPortManager();

            // Read current serial port settings 
            SerialSettings mySerialSettings = _spManager.CurrentSerialSettings;

            // Bind serial port & user interface data sources with serial port settings
            serialSettingsBindingSource.DataSource = mySerialSettings;
            portNameComboBox.DataSource = mySerialSettings.PortNameCollection;
            baudRateComboBox.DataSource = mySerialSettings.BaudRateCollection;
            dataBitsComboBox.DataSource = mySerialSettings.DataBitsCollection;
            parityComboBox.DataSource = Enum.GetValues(typeof(System.IO.Ports.Parity));
            stopBitsComboBox.DataSource = Enum.GetValues(typeof(System.IO.Ports.StopBits));

            // Add evnet handling functions to serial port manager
            _spManager.NewSerialDataRecieved += new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved);
            _spManager.ErrorHandler += new EventHandler<EventArgs>(_spManager_ErrorHandler);

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            // Diable "Disconnect" button
            btnStop.Enabled = false;

            // Add empty point to make chart visible before first sample arrived
            chart1.Series[0].Points.AddXY(double.NaN, double.NaN);
        }

        /*
        * Connect procedure - open and start listening on COM port.
        */
        private void Connect()
        {
            if (_spManager.StartListening())
            {
                btnStop.Enabled = true;
                btnStart.Enabled = false;
                portNameComboBox.Enabled = false;
                baudRateComboBox.Enabled = false;
                dataBitsComboBox.Enabled = false;
                parityComboBox.Enabled = false;
                stopBitsComboBox.Enabled = false;
            }
        }

        /*
        * Disconnect procedure - close and stop listening on COM port.
        */
        private void Disonnect()
        {
            _spManager.StopListening();
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            portNameComboBox.Enabled = true;
            baudRateComboBox.Enabled = true;
            dataBitsComboBox.Enabled = true;
            parityComboBox.Enabled = true;
            stopBitsComboBox.Enabled = true;
        }
   
        /*
         * Enables receive text box.
         */
        private void RxTextBoxEnable()
        {
            rxEnableCheckBox.Checked = true;
            tbDataReceive.Enabled = true;
            _spManager.NewSerialDataRecieved += new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved);
        }

        /*
         * Disables receive text box.
         */
        private void RxTextBoxDisable()
        {
            rxEnableCheckBox.Checked = false;
            tbDataReceive.Enabled = false;
            _spManager.NewSerialDataRecieved -= new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved);
        }

        #endregion

        /* 
         * Output control to both LEDs track bar scroll event method 
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data           
         */
        private void trackBar_Both_LEDs_Scroll(object sender, EventArgs e)
        {
            if (picked_type_of_control_Both_LEDs) {
                TrackBar trackBar = (TrackBar)sender;
                _setValue_Both_LEDs = (UInt16)trackBar.Value;
                textBox_Both_LEDs.Text = _setValue_Both_LEDs.ToString();
            }
        }

        /* 
         * Output control to one LED track bar scroll event method 
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data           
         */
        private void trackBar_One_LED_Scroll(object sender, EventArgs e)
        {
            if (picked_type_of_control_One_LED) { 
                TrackBar trackBar = (TrackBar)sender;
                _setValue_One_LED = (UInt16)trackBar.Value;
                textBox_One_LED.Text = _setValue_One_LED.ToString();
            }
        }

        /*
         * Output control 'SET' button click event method for both LEDs
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_set_point_both_LED_Click(object sender, EventArgs e)
        {
            if (picked_type_of_control_Both_LEDs) {
                _spManager.Send(_setValue_Both_LEDs.ToString("X3"));
                set_val = _setValue_Both_LEDs;
            }
        }

        /*
         * Output control 'SET' button click event method for one LED
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_set_point_one_LED_Click(object sender, EventArgs e)
        {
            if (picked_type_of_control_One_LED)
            {
                _spManager.Send(_setValue_One_LED.ToString("X3"));
                set_val = _setValue_One_LED;
            }
        }

        /*
         * Picked type of control 'BOTH LEDS' - Both LEDs
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_Both_Click(object sender, EventArgs e)
        {
            picked_type_of_control_Both_LEDs = true;
            picked_type_of_control_One_LED = false;
            _spManager.Send("TWO");
            button_show_error.Enabled = true;
            button_chart_Start.Enabled = true;
            button_chart_Stop.Enabled = true;
            label11.Text = "Now you can generate the chart.";
            label11.Location = new System.Drawing.Point(120, 373);
            label_type_of_control.Text = "You are controling both of LEDs.";
            this.button_Both.BackColor = System.Drawing.Color.Blue;
            this.button_One.BackColor = System.Drawing.Color.Gray;
            max_val = 180;
        }

        /*
         * Picked type of control 'ONE LED' - One LED
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_One_Click(object sender, EventArgs e)
        {
            picked_type_of_control_Both_LEDs = false;
            picked_type_of_control_One_LED = true;
            _spManager.Send("ONE");
            button_show_error.Enabled = true;
            button_chart_Start.Enabled = true;
            button_chart_Stop.Enabled = true;
            label11.Text = "Now you can generate the chart.";
            label11.Location = new System.Drawing.Point(120, 373);
            label_type_of_control.Text = "You are controling just one LED.";
            this.button_Both.BackColor = System.Drawing.Color.Gray;
            this.button_One.BackColor = System.Drawing.Color.Blue;
            max_val = 110;
        }

        /*
         * Click to start drawing a chart
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_chart_Start_Click(object sender, EventArgs e)
        {
            RxTextBoxDisable();
            _spManager.NewSerialDataRecieved += new EventHandler<SerialDataEventArgs>(_spManager_NewPlotDataRecieved);
            this.button_chart_Start.BackColor = System.Drawing.Color.Green;
            this.button_chart_Stop.BackColor = System.Drawing.Color.Gray;
        }

        /*
         * Click to stop drawing a chart
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_chart_Stop_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            send_just_text = true;
            //req_for_set_val = false;
            // Add empty point to make chart visible before first sample arrived
            chart1.Series[0].Points.AddXY(double.NaN, double.NaN);
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = _plotTimeMax;
            _plotTime = 0;
            _spManager.NewSerialDataRecieved -= new EventHandler<SerialDataEventArgs>(_spManager_NewPlotDataRecieved);
            this.button_chart_Start.BackColor = System.Drawing.Color.Gray;
            this.button_chart_Stop.BackColor = System.Drawing.Color.Red;
        }

        /*
         * Calculat the control error value as a percentage 
         * @param min - contains a control min value 
         * @param max - contains a control max value
         * @param set - contains a set value of control
         * @param ou1 - contains a output value of control
         * @return control error value as a percentage 
         */
        private float _errorValInPercent(int min, int max, int set, float out1)
        {
            float result = 0.0f;

            if ((max - min) != 0)
                result = (System.Math.Abs(set - out1) / (max - min)) * 100;
            else
                result = -1;

            return result;
        }

        /*
         * Error control 'SHOW ERROR' button click event method for calculate error
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_show_error_Click(object sender, EventArgs e)
        {
            error_val = _errorValInPercent(min_val, max_val, set_val, (float)outValue);
            textBox_Error.Text = error_val.ToString();
        }

        /* 
         * Send data to csv file 
         * @param t - time of measurements in senconds
         * @param set_val - set value of control
         * @param out_val - out vaule of control
         * @param max - max values of control
         * @param min - min values of control
         * @param path - path to csv file  
         * @param name - csv file name 
         */
        private void _saveDataToCsvFile(double t, int set_val, double out_val, int max, int min, string path, string name)
        {
            // Open file
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path + "/" + name, true);
            if (writer != null)
            {
                if (send_just_text)
                {
                    // File header
                    DateTime Date = DateTime.Now;
                    writer.WriteLine(@"");
                    writer.WriteLine(@"Measurements of {0}", Date);
                    writer.WriteLine(@"Time [s];Set Value [lux];Output Value [lux];Max Value [lux];Min Value [lux]");
                    send_just_text = false;
                }
                else
                    writer.WriteLine(@"{0};{1};{2};{3};{4}", t, set_val, out_val, max, min);
                // Close file
                writer.Close();
            }
        }

        /*
         * Input plot new data recieved event handling function. Update of 'PlotInput' chart.
         * @param sender - contains a reference to the control/object
         * @param e - contains the serial port event data
         */
        void _spManager_NewPlotDataRecieved(object sender, SerialDataEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<SerialDataEventArgs>(_spManager_NewPlotDataRecieved), new object[] { sender, e });
                return;
            }

            // Byte array to string
            string hex = Encoding.ASCII.GetString(e.Data);
            _outValue_Str += hex;

            // if buffer holds at least one message
            if(_outValue_Str.Length >= _spMsgSize)
            {
                try
                {
                    // parse first message as hex value
                    UInt16 reg = UInt16.Parse(_outValue_Str.Substring(0, _spMsgSize), NumberStyles.HexNumber);
                     _outValue_Str = _outValue_Str.Remove(0, _spMsgSize);

                    outValue = reg;

                    if (_plotTime > _plotTimeMax)
                    {
                        chart1.Series[0].Points.RemoveAt(1);
                        chart1.ChartAreas[0].AxisX.Minimum = _plotTime - _plotTimeMax;
                        chart1.ChartAreas[0].AxisX.Maximum = _plotTime;
                    }

                    chart1.Series[0].Points.AddXY(_plotTime, outValue);

                    _saveDataToCsvFile(_plotTime, set_val, outValue, max_val, min_val, path_to_csv_file, file_name);
                    _plotTime += _plotTimeStep;
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
