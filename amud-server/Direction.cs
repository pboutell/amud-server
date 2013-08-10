using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    static class Direction
    {
        public const int North = 0;
        public const int East = 1;
        public const int South = 2;
        public const int West = 3;

        public static string directionName(int direction)
        {
            if (direction == 0)
                return "North";
            if (direction == 1)
                return "East";
            if (direction == 2)
                return "South";
            if (direction == 3)
                return "West";

            return "towards the sun";
        }

        public static int directionNumber(string direction)
        {
            string clean = direction.TrimEnd('\r', '\n');
            int num = -1;

            switch (clean.Trim())
            {
                case "north": num = 0; break;
                case "east": num = 1; break;
                case "south": num = 2; break;
                case "west": num = 3; break;
            }

            return num;
        }

        public static int oppositeExit(int exit)
        {
            switch (exit)
            {
                case 0: return 2;
                case 1: return 3;
                case 2: return 0;
                case 3: return 1;
            }

            return 0;
        }
    }
}
