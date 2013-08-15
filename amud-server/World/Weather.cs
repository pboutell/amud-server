using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Weather
    {
        public static int weatherDuration = 0;
        public bool isNight { get; private set; }

        private DateTime worldTime;

        public Weather(DateTime worldTime)
        {
            this.worldTime = worldTime;
        }

        public string dayNightToString()
        {
            StringBuilder buffer = new StringBuilder();

            string[] time = worldTime.ToShortTimeString().Split(':');
            string hour = time[0] + time[1];

            buffer.AppendLine();

            switch (hour)
            {
                case "1200 AM":
                    buffer.Append("The clock strikes midnight.");
                    break;
                case "1200 PM":
                    buffer.Append("The time is 12:00pm");
                    break;
                case "600 AM":
                    buffer.Append("The sun rises from the east, marking a new day.");
                    break;
                case "800 PM":
                    buffer.Append("The sun sets to the west, night has arrived.");
                    break;
            }

            return buffer.ToString();
        }

        public string weatherToString()
        {
            StringBuilder buffer = new StringBuilder();
            bool isWeather = false;

            switch (World.randomNumber.Next(100))
            {
                case 0:
                    buffer.Append("It has started raining.");
                    isWeather = true;
                    break;
                case 1:
                    buffer.Append("It is downpouring.");
                    isWeather = true;
                    break;
                default:
                    isWeather = false;
                    break;
            }

            if (isWeather)
            {
                weatherDuration = World.randomNumber.Next(100, 1000);
            }

            return buffer.ToString();
        }
    }
}
