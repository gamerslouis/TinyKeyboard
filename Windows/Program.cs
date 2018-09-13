﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyKeyboard
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            CenterControl cc;
            try
            {
                cc = new CenterControl();
            }
            catch
            {
                return;
            }

            Form1 Form1 = new Form1(new CenterControlGUIMessage(cc));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
        }
    }
}
