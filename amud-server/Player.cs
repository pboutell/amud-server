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
        public CommandParser parser;

        private TcpClient client;
        private NetworkStream stream;
        private Queue<string> commandPipe = new Queue<string>();
        private StringBuilder command = new StringBuilder();
        private Logger logger = new Logger();
        
        public Thread threadRef { get; set; }
        public string name { get; private set; }
        public Room room { get; set; }

        public Player(TcpClient client, ref ConcurrentBag<Player> players)
        {
            this.client = client;
            this.players = players;
        }

        public void init(object e)
        {
            stream = client.GetStream();

            sendToPlayer("\nThis is A MUD!\n");
            sendToPlayer("  /\\_/\\   ");
            sendToPlayer(" ( '-' )  ");
            sendToPlayer(" (     )  ");
            sendToPlayer(" |  |  |  ");
            sendToPlayer(" (__)(__) ");

            sendNoNewline("name: ");

            name = getsInput();
            sendToPlayer("\nhi " + name + "!");

            logger.log(name + " has entered the game.");
            room = World.rooms.First();

            parser = new CommandParser(this);
            parser.parse("look");

            inputLoop();
        }

        private void inputLoop()
        {
             while (client.Connected)
             {
                 read();

                 if (commandPipe.Count > 0)
                 {
                     parser.parse(commandPipe.Dequeue());
                 }
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
                writeToClient(text + "\r\n");
            }
            catch (IOException e)
            {
                logger.log(name + ":" + e.Message);
            }
        }

        public void sendNoNewline(string text)
        {
            try
            {
                writeToClient(text);
            }
            catch (IOException e)
            {
                logger.log(name + ":" + e.Message);
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
                logger.log(name + ":" + e.Message);
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
                logger.log(name + ":" + e.Message);
            }
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
            logger.log(name + " has left the game.");

            stream.Close();
            client.Close();

            OnPlayerDisconnected(this, new EventArgs());
        }
    }
}
