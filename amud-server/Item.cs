using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    [Serializable]
    public class Item
    {
        public string name { get; set; }
        public string description { get; set; }
        public int weight { get; set; }
        public string wearLocation { get; set; }
        public int value { get; set; }

        public Item(string name, string description, int weight, string wearLocation, int value)
        {
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.wearLocation = wearLocation;
            this.value = value;
        }
    }
}
