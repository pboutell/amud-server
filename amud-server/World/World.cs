using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace amud_server
{
    [Serializable]
    public class World
    {
        public List<Room> rooms { get; set; }
        public ConcurrentBag<NPC> mobs { get; set; }
        public DateTime worldTime { get; set; }

        [NonSerialized]
        public static Random randomNumber = new Random();

        public World()
        {
            worldTime = new DateTime();
            rooms = new List<Room>();
            rooms.Add(new Room("The Void", "You are standing in the middle of nothing."));
            mobs = new ConcurrentBag<NPC>();

            for (int x = 0; x < 3; x++)
            {
                NPC test = new NPC("mob", "A slimy sticky stinky mob", new CharacterStats(10, 100), this);
                rooms.First().addNPC(test);
                mobs.Add(test);
                test = new NPC("bob", "A slimy sticky stinky bob", new CharacterStats(10, 100), this);
                rooms.First().addNPC(test);
                mobs.Add(test);
                test = new NPC("sob", "A slimy sticky stinky sob", new CharacterStats(10, 100), this);
                rooms.First().addNPC(test);
                mobs.Add(test);
                test = new NPC("cob", "A slimy sticky stinky cob", new CharacterStats(10, 100), this);
                rooms.First().addNPC(test);
                mobs.Add(test);
            }

            rooms.First().addItem(new Item("leggings", "a worn pair of leather leggings", 2, "legs"));
        }

        public string getWeather(DateTime worldTime)
        {
            StringBuilder buffer = new StringBuilder();
            Weather weather = new Weather(worldTime);

            buffer.Append(weather.dayNightToString());

            if (Weather.weatherDuration > 0)
            {
                Weather.weatherDuration--;
                if (Weather.weatherDuration == 0)
                {
                    buffer.Append("The weather has improved.");
                }
            }
            else
            {
                buffer.Append(weather.weatherToString());
            }

            return buffer.ToString();
        }

        public bool newExit(int direction, Room room)
        {
            if (room.hasExit(direction))
            {
                return false;
            }
            else
            {
                Room newRoom = new Room("New Room", "This room has not been finished yet.");
                room.exits[direction] = newRoom;
                newRoom.exits[Direction.oppositeExit(direction)] = room;
                rooms.Add(newRoom);
                return true;
            }
        }
       
    }
}
