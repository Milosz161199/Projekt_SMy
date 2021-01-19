/**
*   @author Adrian Wojcik
*   @file SerialPortManager.cs
*   @date 02.11.17
*   @brief Serial port manager class - support for receiving and sending data.
*   Based on project by Amund Gjersøe (www.codeproject.com/Articles/75770/Basic-serial-port-listening-application)
*/

//#define SELECT_FIRST_FOUND 

/*
 * System libraries
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Reflection;
using System.ComponentModel;
using System.Threading;
using System.IO;

namespace SerialPortApp.Serial
{
    /**
    * Manager for serial port class. Inherits from IDisposable interface.
    */
    public class SerialPortManager : IDisposable
    {
        //! Default constructor
        public SerialPortManager()
        {
            // Finding installed serial ports on hardware
            _currentSerialSettings.PortNameCollection = SerialPort.GetPortNames();
            _currentSerialSettings.PropertyChanged += new PropertyChangedEventHandler(_currentSerialSettings_PropertyChanged);

            // If serial ports is found, we select the first found
            #if SELECT_FIRST_FOUND
            if (_currentSerialSettings.PortNameCollection.Length > 0)
                _currentSerialSettings.PortName = _currentSerialSettings.PortNameCollection[0];
            #endif
        }
    
        //! Destructor
        ~SerialPortManager()
        {
            Dispose(false);
        }

  
        #region Fields

        private SerialPort _serialPort;                                       /**< System serial port class object. */
        private SerialSettings _currentSerialSettings = new SerialSettings(); /**< Custom serial port settings class object. */
        private string _latestRecieved = String.Empty;                        /**< Recieved data string container. */

        public event EventHandler<SerialDataEventArgs> NewSerialDataRecieved; /**< New data recieving routine event handler. */
        public event EventHandler<EventArgs> ErrorHandler;                    /**< Exception handling event. */

        #endregion

        #region Properties

        /*
         * Gets or sets the current serial port settings
         */
        public SerialSettings CurrentSerialSettings
        {
            get { return _currentSerialSettings; }
            set { _currentSerialSettings = value; }
        }

        #endregion

        #region Event handlers

        /*
         * Serial port settings class property changed handling function.
         * @param sender - contains a reference to the control/object that raised the event.
         * @param e - contains the event data.
         */
        void _currentSerialSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if serial port is changed, a new baud query is issued
            if (e.PropertyName.Equals("PortName"))
                UpdateBaudRateCollection();
        }

        /*
        * Serial port data receiving routine. 
        * @param sender - contains a reference to the control/object that raised the event.
        * @param e - contains the event data.
        */
        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int dataLength = 0;

            try
            {
                dataLength = _serialPort.BytesToRead;
            }
            catch (Exception exc)
            {
                ErrorHandler(exc, new EventArgs());
                return;
            }

            byte[] data = new byte[dataLength];
            
            // Read received data
            int nbrDataRead = _serialPort.Read(data, 0, dataLength);

            if (nbrDataRead == 0)
                return;
            
            // Send data to whom ever interested
            if (NewSerialDataRecieved != null)
                NewSerialDataRecieved(this, new SerialDataEventArgs(data));
        }

        #endregion

        #region Methods

        /*
         * Connects to a serial port defined through the current settings.
         * return true if success, false if cannot start listening.
         */
        public bool StartListening()
        {
            if (_serialPort == null) return false;

            // Closing serial port if it is open
            if (_serialPort.IsOpen)  _serialPort.Close();

            // Setting serial port settings
            _serialPort = new SerialPort(
                _currentSerialSettings.PortName,
                _currentSerialSettings.BaudRate,
                _currentSerialSettings.Parity,
                _currentSerialSettings.DataBits,
                _currentSerialSettings.StopBits);

            // Subscribe to event and open serial port for data
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);

            // Try to open selected serial port
            try
            {
                _serialPort.Open();
                return true;
            }
            catch(Exception e)
            {
                ErrorHandler(e, new EventArgs());
                return false;
            }
        }

        /*
        * Closes current serial port.
        */
        public void StopListening()
        {
            _serialPort.Close();
        }

        /*
        * Retrieves the current selected device's COMMPROP structure, and extracts the dwSettableBaud property.
        * Called each time selected serial port name is changed. 
        */
        private void UpdateBaudRateCollection()
        {
            _serialPort = new SerialPort(_currentSerialSettings.PortName);

            try
            {
                _serialPort.Open();

                // Extractsing the dwSettableBaud property
                object p = _serialPort.BaseStream.GetType().GetField("commProp", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_serialPort.BaseStream);
                Int32 dwSettableBaud = (Int32)p.GetType().GetField("dwSettableBaud", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(p);

                _serialPort.Close();
                _currentSerialSettings.UpdateBaudRateCollection(dwSettableBaud);
            }
            catch(Exception e)
            {
                ErrorHandler(e, new EventArgs());
            }
        }

        /*
        * Call to release serial port with "true" input argument value.
        */
        public void Dispose()
        {
            Dispose(true);
        }

        /*
        * Part of basic design pattern for implementing Dispose
        * @param disposing - if equals true, the method has been called directly
        *   or indirectly by a user's code. Managed and unmanaged resources
        *   can be disposed. If equals false, the method has been called by the
        *   runtime from inside the finalizer and you should not reference
        *   other objects. Only unmanaged resources can be disposed.
        */
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_serialPort != null)
                {
                    _serialPort.DataReceived -= new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                }
            }
            // Releasing serial port (and other unmanaged objects)
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Close();

                _serialPort.Dispose();
            }
        }

        /*
        * Send string via serial port.
        * @param data - transmitted data. 
        */
        public void Send(string data)
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                    _serialPort.Write(data);
            }
        }

        #endregion
    }

    /**
    * Event arguments for serial port data. Inherits from EventArgs class.
    */
    public class SerialDataEventArgs : EventArgs
    {
        /*
        * Parametric contructor.
        * \param dataInByteArray - recieved data as byte array.
        */
        public SerialDataEventArgs(byte[] dataInByteArray)
        {
            Data = dataInByteArray;
        }

        public byte[] Data; /**< Byte array containing data from serial port */
    }
}
