﻿using System;
using System.Windows.Forms;

namespace QRST_DI_TS_Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
        
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           Application.Run(new Form1());
          //   Application.Run(new ConsoleForm());
        }
    }
}
