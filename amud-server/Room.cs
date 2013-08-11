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
            exits.Add(null);
            exits.Add(null);
            exits.Add(null);
            exits.Add(null);
        }

        public Room(string name, string description, List<Room> exits)
        {
            this.name = name;
            this.description = description;
            addExits(exits);
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
        
    }
}
