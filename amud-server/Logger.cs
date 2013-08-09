using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Logger
    {
        private MainWindow mainWindow = null;
        private delegate void textStatusDelegate(string message);

        public Logger()
        {
        }

        public Logger(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void log(string message) 
        {
            if (mainWindow != null)
            {
                textStatusDelegate writeStatus = msg => mainWindow.updateTextBox(msg);
                writeStatus(message);
            }
        }
    }
}
