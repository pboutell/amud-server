using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace amud_server
{
    class World
    {
        public static List<Room> rooms = new List<Room>();

        public World()
        {
            rooms.Add(new Room("The Void", "You are standing in the middle of nothing,"));
            NPC test = new NPC("mob", "A slimy sticky stinky mob", new CharacterStats(5, 5));
            rooms.First().addNPC(test);
        }
    }
}
