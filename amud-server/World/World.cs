﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace amud_server
{
    [Serializable]
    class World
    {
        public static List<Room> rooms { get; set; }
        public static ConcurrentBag<NPC> mobs { get; set; }
        public static DateTime worldTime { get; set; }

        public static Random randomNumber = new Random();
        
        public World()
        {
            worldTime = new DateTime();
            rooms = new List<Room>();
            rooms.Add(new Room("The Void", "You are standing in the middle of nothing."));
            mobs = new ConcurrentBag<NPC>();

            for (int x = 0; x < 10; x++)
            {
                NPC test = new NPC("mob", "A slimy sticky stinky mob", new CharacterStats(100, 100));
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
       
    }
}
