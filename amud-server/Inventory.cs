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
                if (e.Value.name == search)
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

            buffer.AppendLine("\r\nInventory:");
            foreach (Item i in inventory)
            {
                buffer.AppendFormat("  {0}\t{1}\r\n", i.name, i.description);
            }

            return buffer.ToString();
        }

        public string equippedToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("\r\nEquipment:");
            foreach (KeyValuePair<string, Item> e in equipped)
            {
                buffer.AppendFormat("  [ {0} ]\t{1}\r\n", e.Key, e.Value.description);
            }

            return buffer.ToString();
        }

        public Item getItemByName(string search)
        {
            foreach (Item i in inventory)
            {
                if (i.name == search)
                {
                    return i;
                }
            }

            return null;
        }

    }
}
