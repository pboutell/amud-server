using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace amud_server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TextWriter _writer = null;
        private Server server;

        public MainWindow()
        {
            InitializeComponent();
            server = new Server(this);
            buttonStop.IsEnabled = false;
        }

        public void updateTextBox(string message)
        {
            _writer = new ConsoleRedirect(textStatus);
            Console.SetOut(_writer);
            Console.WriteLine(message);
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            server.startServer();
            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = true;
            buttonExit.IsEnabled = false;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            server.shutdown();
            buttonStop.IsEnabled = false;
            buttonStart.IsEnabled = true;
            buttonExit.IsEnabled = true;
        }
    }
}
