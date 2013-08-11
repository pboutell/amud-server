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

        public Player (Client client, string name)
        {
            this.client = client;
            this.name = name;
            room = World.rooms.First();
            parser = new CommandParser(this);
        }
    }
}
