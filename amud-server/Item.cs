using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Item
    {
        public string name { get; set; }
        public string description { get; set; }
        public int weight;
        public string wearLocation;

        public Item(string name, string description, int weight, string wearLocation)
        {
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.wearLocation = wearLocation;
        }
    }
}
