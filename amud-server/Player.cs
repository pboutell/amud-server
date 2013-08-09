using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows;
using System.IO;
using System.Threading;

namespace amud_server
{
    class Player
    {
        public event EventHandler<EventArgs> OnPlayerDisconnected;

        public ConcurrentBag<Player> players;

        private TcpClient client;
        private NetworkStream stream;
        private ASCIIEncoding encoding;
        private CommandParser parser;
        private Logger _logger = new Logger();
        private bool isConnected = false;

        public Thread ThreadRef { get; set; }
        public string Name { get; private set; }

        private delegate void textStatusDelegate(string message);

        public Player(TcpClient client, ref ConcurrentBag<Player> players)
        {
            this.client = client;
            this.players = players;
        }

        public void init(object e)
        {
            this.stream = client.GetStream();
            this.encoding = new ASCIIEncoding();
            this.isConnected = true;

            sendToPlayer("\n\nThis is A MUD!\r\n\n");
            sendToPlayer("  /\\_/\\   \n\r");
            sendToPlayer(" ( '-' )  \n\r");
            sendToPlayer(" (     )  \n\r");
            sendToPlayer(" |  |  |  \n\r");
            sendToPlayer(" (__)(__) \n\r");
            sendToPlayer("\nsee..\n\n\n\r");
            sendToPlayer("name plz: ");
            Name = getsInput();
            sendToPlayer("hi " + Name + "!\n\n\r");

            parser = new CommandParser(this);
            inputLoop();
        }

        private void inputLoop()
        {
            while (isConnected)
            {
                parser.parse(getsInput());
            }
        }

        public void sendToAll(string text)
        {
            foreach (Player p in players)
            {
                p.sendToPlayer(text);
            }
        }

        public void sendToRest(string text)
        {
            foreach (Player p in players)
            {
                if (this != p)
                {
                    p.sendToPlayer(text);
                }
            }
        }

        public void sendToPlayer(string text)
        {
            try
            {
                writeToClient(text);
            }
            catch (Exception e)
            {
                _logger.log(e.Message + e.StackTrace);
            }
        }

        private void writeToClient(string text)
        {
            byte[] buffer;

            if (text != null && this.isConnected)
            {
                buffer = this.encoding.GetBytes(text);
                this.stream.Write(buffer, 0, buffer.Length);
                this.stream.Flush();
            }
        }
        
        private string getsInput()
        {
            string input = "";

            try
            {
                input = readFromPlayer();
            }
            catch (Exception e)
            {
                _logger.log(e.Message + e.StackTrace);
            }

            return input;
        }

        private string readFromPlayer()
        {
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            string message = "";

            while (!message.EndsWith("\r\n") && this.isConnected)
            {
                bytesRead = this.stream.Read(buffer, 0, 4096);
                message += this.encoding.GetString(buffer, 0, bytesRead);
            }

            return message.TrimEnd('\r', '\n');
        }

        public void disconnect()
        {
            isConnected = false;
            client.Close();
            
            OnPlayerDisconnected(this, new EventArgs());
        }
    }
}
