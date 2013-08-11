using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Room
    {
        public string name;
        public string description;

        public List<Room> exits = new List<Room>();

        public Room(string name, string description)
        {
            this.name = name;
            this.description = description;
            addExits(this.exits);
        }

        public Room(string name, string description, List<Room> exits)
        {
            this.name = name;
            this.description = description;
            addExits(exits);
        }

        public bool hasExit(int direction)
        {
            return exits.ElementAtOrDefault(direction) != null;
        }

        private void addExits(List<Room> exits) 
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
