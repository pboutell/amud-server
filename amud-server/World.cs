using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace amud_server
{
    class World
    {
        public static List<Room> rooms { get; private set; }
        

        public World()
        {
            rooms = new List<Room>();
            rooms.Add(new Room("The Void", "You are standing in the middle of nothing,"));

            NPC test = new NPC("mob", "A slimy sticky stinky mob", new CharacterStats(5, 5));
            rooms.First().addNPC(test);

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
    }
}
