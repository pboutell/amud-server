using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Item
    {
        public string name { get; private set; }
        public string description { get; private set; }
        public int weight { get; private set; }
        public string wearLocation { get; private set; }

        public Item(string name, string description, int weight, string wearLocation)
        {
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.wearLocation = wearLocation;
        }
    }
}
