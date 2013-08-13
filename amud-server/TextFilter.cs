﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class TextFilter
    {
        private Dictionary<string, string> colors = new Dictionary<string, string>();

        public TextFilter()
        {
            colors.Add("r", "\x1B[0m\x1B[31m");  //red
            colors.Add("g", "\x1B[0m\x1B[32m");  //green
            colors.Add("y", "\x1B[0m\x1B[33m");  //yellow
            colors.Add("b", "\x1B[0m\x1B[34m");  //blue
            colors.Add("n", "\x1B[0m\x1B[35m");  //magenta
            colors.Add("c", "\x1B[0m\x1B[36m");  //cyan
            colors.Add("w", "\x1B[0m\x1B[37m");  //white
            colors.Add("R", "\x1B[1m\x1B[31m");  //red
            colors.Add("G", "\x1B[1m\x1B[32m");  //green
            colors.Add("Y", "\x1B[1m\x1B[33m");  //yellow
            colors.Add("B", "\x1B[1m\x1B[34m");  //blue
            colors.Add("M", "\x1B[1m\x1B[35m");  //magenta
            colors.Add("C", "\x1B[1m\x1B[36m");  //cyan
            colors.Add("W", "\x1B[1m\x1B[37m");  //white
            colors.Add("x", "\x1B[0m");   //reset
            
        }

        public string filterColor(string input)
        {
            string output = "";
            bool colorCode = false;

            foreach (char c in input)
            {
                if (c == ' ')
                {
                    colorCode = false;
                    output += c;
                }
                else if (colorCode)
                {
                    string buffer = "";
                    colors.TryGetValue(c.ToString(), out buffer);

                    output += buffer;
                    colorCode = false;
                }
                else if (c == '&')
                {
                    colorCode = true;
                }
                else
                {
                    output += c;
                }

            }
            return output + "\x1B[0m";
        }
    }
}
