using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Player
    {
        public CommandParser parser;
        public Client client;
        public string name { get; private set; }
        public Room room { get; set; }

        private StringBuilder writeBuffer = new StringBuilder();

        public Player (Client client, string name)
        {
            this.client = client;
            this.name = name;
            World.rooms.First().addPlayer(this);
            this.parser = new CommandParser(this);
        }

        public void prompt()
        {
            client.sendNoNewline(name + "# ");
        }

        public void bufferToSend(string text)
        {
            writeBuffer.Append(text + "\r\n");
        }

        public void sendBuffer()
        {
            client.send(writeBuffer.ToString());
            writeBuffer.Clear();
        }
    }
}
