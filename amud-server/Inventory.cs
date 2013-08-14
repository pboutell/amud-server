using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    class Inventory
    {
        private Dictionary<string, Item> equipped = new Dictionary<string, Item>();
        private List<Item> inventory = new List<Item>();

        public Inventory()
        {
            this.equipped.Add("none", new Item("dummmy", "", 0, ""));
        }

        public void addToInventory(Item item)
        {
            inventory.Add(item);
        }

        public void removeFromInventory(Item item)
        {
            inventory.Remove(item);
        }

        public Item removeFromEquipped(string search)
        {
            Item item = null;

            foreach(KeyValuePair<string, Item> e in equipped)  
            {
                if (e.Value.name.StartsWith(search))
                {
                    equipped.TryGetValue(e.Key, out item);
                    item = e.Value;
                }
            }
            if (item != null)
            {
                equipped.Remove(item.wearLocation);
                inventory.Add(item);
            }

            return item;
        }

        public bool addToEquipped(string location, Item item)
        {
            if (!equipped.Keys.Contains(location))
            {
                equipped.Add(location, item);
                inventory.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string inventoryToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("  %w:%y[ %WInventory %y]%W:\r\n");

            foreach (Item i in inventory)
            {
                buffer.AppendFormat("  %W( %y{0,11}%W )    %W{1}\r\n", i.name, i.description);
            }

            return buffer.ToString();
        }

        public string equippedToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("  %w:%b[ %WEquipment %b]%W:");
            buffer.AppendLine();
            foreach (KeyValuePair<string, Item> e in equipped)
            {
                buffer.AppendFormat("  %W( %b{0, 11} %W)    %W{1}\r\n", e.Key, e.Value.description);
            }

            return buffer.ToString();
        }

        public Item getItemByName(string search)
        {
            foreach (Item i in inventory)
            {
                if (i.name.StartsWith(search))
                {
                    return i;
                }
            }

            return null;
        }
    }
}
