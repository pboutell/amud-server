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
    class Client
    {
        public event EventHandler<EventArgs> OnPlayerDisconnected;

        public ConcurrentBag<Client> clients;

        private TcpClient connection;
        private NetworkStream stream;
        private Queue<string> commandPipe = new Queue<string>();
        private StringBuilder command = new StringBuilder();
        private Logger logger = new Logger();
        private TextFilter filter = new TextFilter();
        public bool playing = false;
        public bool connecting = true;
        
        public Thread threadRef { get; set; }
        public Player player { get; private set; }

        public Client(TcpClient client, ref ConcurrentBag<Client> clients)
        {
            this.connection = client;
            this.clients = clients;
        }

        public void init(object e)
        {
            stream = connection.GetStream();

            send("This is A MUD!\n");
            send("&Y  /\\_/\\   ");
            send("&M ( '-' )  ");
            send("&C (     )  ");
            send("&B |  |  |  ");
            send("&R (__)(__) ");

            sendNoNewline("name: ");
            string name = getsInput();
            player = new Player(this, name);
           
            send("\nhi " + name + "!");
            logger.log(name + " has entered the game.");

            connecting = false;
            playing = true;
            player.parser.parse("look");

            inputLoop();
        }

        private void inputLoop()
        {
             while (connection.Connected)
             {
                 read();

                 if (commandPipe.Count > 0)
                 {
                     player.parser.parse(commandPipe.Dequeue());
                 }
             }
        }

        public void sendToAll(string text)
        {
            foreach (Client c in clients)
            {
                c.send("\r\n\n" + text);
            }
        }

        public void sendToRest(string text)
        {
            foreach (Client c in clients)
            {
                if (this != c)
                {
                    c.send("\r\n\n" + text);
                }
            }
        }

        public void send(string text)
        {
            try
            {
                if (playing || connecting)
                {
                    writeToClient(filter.filterColor(text + "\r\n"));
                }
                if (playing)
                {
                    player.prompt();
                }
            }
            catch (IOException e)
            {
                logger.log(player.name + ":" + e.Message);
                playing = false;
            }
        }

        public void sendNoNewline(string text)
        {
            try
            {
                writeToClient(filter.filterColor(text));
            }
            catch (IOException e)
            {
                logger.log(player.name + ":" + e.Message);
                playing = false;
            }
        }

        private void writeToClient(string text)
        {
            byte[] buffer;

            if (text != null && stream.CanWrite)
            {
                buffer = Encoding.ASCII.GetBytes(text);
                this.stream.Write(buffer, 0, buffer.Length);
                this.stream.Flush();
            }
        }
        
        private string getsInput()
        {
            string input = "";

            try
            {
                input = readToString();
            }
            catch (IOException e)
            {
                logger.log(player.name + ":" + e.Message);
                playing = false;
            }

            return input;
        }

        private void read()
        {
            try
            {
                readToBuffer();
            }
            catch (Exception e)
            {
                logger.log(player.name + ":" + e.Message);
            }
        }

        private string readToString()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            string line = "";

            do
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                line += Encoding.ASCII.GetString(buffer, 0, bytesRead);
            }
            while (stream.CanRead && !line.EndsWith("\r\n"));

            return line.TrimEnd('\r', '\n');
        }

        private void readToBuffer()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = 0;

            do {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                commandBuffer(Encoding.ASCII.GetString(buffer, 0, bytesRead));
            }
            while (stream.CanRead && stream.DataAvailable);
        }

        private void commandBuffer(string message)
        {
            if (message.Length > 0)
            {
                command.Append(message);
            }

            if (command.ToString().EndsWith("\r\n"))
            {
                command.ToString().TrimEnd('\r', '\n');
                if (command.Length > 0 && !command.ToString().Equals("\r\n"))
                {
                    commandPipe.Enqueue(command.ToString());
                }
                command.Clear();
            }
        }

        public void disconnect()
        {
            logger.log(player.name + " has left the game.");

            playing = false;
            player.room.removePlayer(player);

            stream.Close();
            connection.Close();

            OnPlayerDisconnected(this, new EventArgs());
        }
    }
}
