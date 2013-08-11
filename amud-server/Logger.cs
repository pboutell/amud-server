using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace amud_server
{
    class Logger
    {
        public void log(string message) 
        {
            MainWindow.mainWindow.Dispatcher.BeginInvoke(new Action(delegate()
            {
                MainWindow.mainWindow.textStatus.AppendText(message + "\n");
            }));
        }
    }
}
