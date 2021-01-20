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
        const int _spMsgSize = 3;
        UInt16 _outValue_Both_LEDs;
        UInt16 _outValue_One_LED;
        bool picked_type_of_control_One_LED = true;
        bool picked_type_of_control_Both_LEDs = false;
        string _outValue_Str;

        double _plotTimeStep = 0.1;       // [s]
        double _plotTime = 0.0;           // [s]
        const double _plotTimeMax = 10.0; // [s]

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

        private void trackBar_Both_LEDs_Scroll(object sender, EventArgs e)
        {
            if (picked_type_of_control_Both_LEDs) {
                TrackBar trackBar = (TrackBar)sender;
                _outValue_Both_LEDs = (UInt16)trackBar.Value;
                textBox_Both_LEDs.Text = _outValue_Both_LEDs.ToString();
            }
        }

        private void trackBar_One_LED_Scroll(object sender, EventArgs e)
        {
            if (picked_type_of_control_One_LED) { 
                TrackBar trackBar = (TrackBar)sender;
                _outValue_One_LED = (UInt16)trackBar.Value;
                textBox_One_LED.Text = _outValue_One_LED.ToString();
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
                _spManager.Send(_outValue_Both_LEDs.ToString("X3"));
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
                _spManager.Send(_outValue_One_LED.ToString("X3"));
            }
        }

        /*
         * Picked type of controling - Both LEDs
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_Both_Click(object sender, EventArgs e)
        {
            picked_type_of_control_Both_LEDs = true;
            picked_type_of_control_One_LED = false;
            _spManager.Send("TWO");
            label_type_of_control.Text = "You are controling both of LEDs.";
            this.button_Both.BackColor = System.Drawing.Color.Blue;
            this.button_One.BackColor = System.Drawing.Color.Gray;
        }

        /*
         * Picked type of controling - One LED
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_One_Click(object sender, EventArgs e)
        {
            picked_type_of_control_Both_LEDs = false;
            picked_type_of_control_One_LED = true;
            _spManager.Send("ONE");
            label_type_of_control.Text = "You are controling just one LED.";
            this.button_Both.BackColor = System.Drawing.Color.Gray;
            this.button_One.BackColor = System.Drawing.Color.Blue;
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
        }

        /*
         * Click to stop drawing a chart
         * @param sender - contains a reference to the control/object
         * @param e - contains the event data
         */
        private void button_chart_Stop_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            // Add empty point to make chart visible before first sample arrived
            chart1.Series[0].Points.AddXY(double.NaN, double.NaN);
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = _plotTimeMax;
            _spManager.NewSerialDataRecieved -= new EventHandler<SerialDataEventArgs>(_spManager_NewPlotDataRecieved);
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

                    double outValue = reg;

                    if (_plotTime > _plotTimeMax)
                    {
                        chart1.Series[0].Points.RemoveAt(1);
                        chart1.ChartAreas[0].AxisX.Minimum = _plotTime - _plotTimeMax;
                        chart1.ChartAreas[0].AxisX.Maximum = _plotTime;
                    }

                    chart1.Series[0].Points.AddXY(_plotTime, outValue);
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
