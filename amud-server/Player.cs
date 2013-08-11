﻿using System;
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
        
        public Thread ThreadRef { get; set; }
        public string Name { get; private set; }
        public Room room { get; set; }

        public Player(TcpClient client, ref ConcurrentBag<Player> players)
        {
            this.client = client;
            this.players = players;
        }

        public void init(object e)
        {
            stream = client.GetStream();

            sendToPlayer("\n\nThis is A MUD!\r\n\n");
            sendToPlayer("  /\\_/\\   \n\r");
            sendToPlayer(" ( '-' )  \n\r");
            sendToPlayer(" (     )  \n\r");
            sendToPlayer(" |  |  |  \n\r");
            sendToPlayer(" (__)(__) \n\r");

            sendToPlayer("name: ");

            Name = getsInput().TrimEnd('\n', '\r');
            sendToPlayer("hi " + Name + "!\n\n\r");

            logger.log(Name + " has entered the game.");
            room = World.rooms.First();

            parser = new CommandParser(this);
            parser.parse("look");

            inputLoop();
        }

        private void inputLoop()
        {
            try
            {
                while (client.Connected && stream.CanRead)
                {
                    readFromClient();

                    if (commandPipe.Count > 0)
                    {
                        parser.parse(commandPipe.Dequeue());
                    }
                }
            }
            catch (IOException e)
            {
                logger.log(e.Message);
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
            catch (IOException e)
            {
                logger.log(e.Message);
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
            try
            {
                do {
                    readFromClient();
                } while (commandPipe.Count < 1);
            }
            catch (IOException e)
            {
                logger.log(e.Message);
            }

            if (commandPipe.Peek().Length > 0)
                return commandPipe.Dequeue();
            else
                return "";
        }

        private void readFromClient()
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
            command.Append(message);

            if (command.ToString().EndsWith("\r\n") && command.Length > 0)
            {
                command.ToString().TrimEnd('\r', '\n');
                commandPipe.Enqueue(command.ToString());
                command.Clear();
            }
        }

        public void disconnect()
        {
            logger.log(Name + " has left the game.");
            stream.Close();
            client.Close();
            OnPlayerDisconnected(this, new EventArgs());
        }
    }
}
