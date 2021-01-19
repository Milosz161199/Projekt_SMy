/**
*   @author Adrian Wojcik
*   @file Program.cs
*   @date 02.11.17
*   @brief Program class. 
*   Based on project by Amund Gjersøe (www.codeproject.com/Articles/75770/Basic-serial-port-listening-application)
*/

/*
 * System libraries
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SerialPortApp
{
    /*
    * Program class.
    */
    static class Program
    {
        /*
         * The main entry point for the application.
         * STAThread attribute indicates that the COM threading model for the application is single-threaded apartment. 
         * This attribute must be present on the entry point of any application that uses Windows Forms; if it is omitted, 
         * the Windows components might not work correctly. If the attribute is not present, the application uses the 
         * multithreaded apartment model, which is not supported for Windows Forms.
         */
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
