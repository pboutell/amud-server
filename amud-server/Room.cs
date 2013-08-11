using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Room
    {
        public List<Room> exits = new List<Room>();

        public string name;
        public string description;
        
        public Room(string name, string description)
        {
            this.name = name;
            this.description = description;
            initExits(this.exits);
        }

        public Room(string name, string description, List<Room> exits)
        {
            this.name = name;
            this.description = description;
            initExits(exits);
        }

        public bool hasExit(int direction)
        {
            return exits.ElementAtOrDefault(direction) != null;
        }

        public bool newExit(int direction)
        {
            if (hasExit(direction))
            {
                return false;
            }
            else
            {
                Room newRoom = new Room("New Room", "This room has not been finished yet.");
                exits[direction] = newRoom;
                newRoom.exits[Direction.oppositeExit(direction)] = this;
                World.rooms.Add(newRoom);
                return true;
            }
        }

        private void initExits(List<Room> exits) 
        {
            for (int x = 0; x < 4; x++)
            {
                if (exits.ElementAtOrDefault(x) == null)
                {
                    this.exits.Add(null);
                }
                else
                {
                    this.exits.Add(exits.ElementAtOrDefault(x));
                }
            }
        }

        public string exitsToString()
        {
            StringBuilder exits = new StringBuilder();

            exits.Append("[ ");
            int appended = 0;
            for (int x = 0; x < 4; x++)
            {
                if (this.exits.ElementAtOrDefault(x) != null)
                {
                    exits.Append(Direction.directionToName(x));
                    appended++;
                    exits.Append(" ");
                }
            }

            if (appended < 1)
                exits.Append("none ");

            exits.Append("]\r\n\n");

            return exits.ToString();
        }
    }
}
