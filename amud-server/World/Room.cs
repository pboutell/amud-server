using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amud_server
{
    [Serializable]
    public class Room
    {
        //public event EventHandler<EventArgs> OnPlayerEnter; ??

        public List<Room> exits { get; private set; }
        public List<Character> characters { get; private set; }
        public List<Player> players { get; private  set; }
        public List<NPC> npcs { get; private set; }
        public List<Item> items { get; private set; }
        public string name { get; private set; }
        public string description { get; private set; }

        public Room(string name, string description)
        {
            this.name = name;
            this.description = description;
            
            exits = new List<Room>();
            characters = new List<Character>();
            players = new List<Player>();
            npcs = new List<NPC>();
            items = new List<Item>();

            initExits(this.exits);
        }

        public bool hasExit(int direction)
        {
            return exits.ElementAtOrDefault(direction) != null;
        }

        public string playersToString(Player player)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (Player p in players)
            {
                if (p != player)
                {
                    if (!p.client.isPlaying)
                    {
                        buffer.Append("%w( %rdisconnected%w )%x ");
                    }
                    buffer.AppendFormat("{0} is standing here.\r\n", p.name);
                }
            }

            return buffer.ToString().TrimEnd('\r', '\n');
        }

        public string NPCsToString()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (NPC n in npcs)
            {
                buffer.AppendFormat("A {0} is standing here.\r\n", n.name);
            }

            return buffer.ToString().TrimEnd('\r', '\n');
        }

        public string itemsToString()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (Item i in items)
            {
                if (i.name.EndsWith("s"))
                    buffer.AppendFormat("Some {0} are laying on the ground.\r\n", i.name);
                else
                    buffer.AppendFormat("A {0} is laying on the ground.\r\n", i.name);
            }

            return buffer.ToString().TrimEnd('\r', '\n');
        }

        public Character getCharacterByName(string search)
        {
            foreach (Character c in characters)
            {
                if (c.name.StartsWith(search.TrimEnd('\r', '\n')))
                {
                    return c;
                }
            }

            return null;
        }

        public Player getPlayerByName(string search)
        {
            foreach (Player p in players)
            {
                if (p.name.StartsWith(search.TrimEnd('\r', '\n')))
                {
                    return p;
                }
            }

            return null;
        }

        public NPC getNPCByName(string search)
        {
            foreach (NPC n in npcs)
            {
                if (n.name.StartsWith(search.TrimEnd('\r', '\n')))
                {
                    return n;
                }
            }

            return null;
        }

        public Item getItemByName(string search)
        {
            foreach (Item i in items)
            {
                if (i.name.StartsWith(search.TrimEnd('\r', '\n')))
                {
                    return i;
                }
            }

            return null;
        }

        public void addPlayer(Player player)
        {
            players.Add(player);
            characters.Add(player);
            player.room = this;
        }

        public void removePlayer(Player player)
        {
            characters.Remove(player);
            players.Remove(player);
        }

        public void addNPC(NPC npc)
        {
            npc.room = this;
            characters.Add(npc);
            npcs.Add(npc);
        }

        public void removeNPC(NPC npc)
        {
            characters.Remove(npc);
            npcs.Remove(npc);
        }

        public void addItem(Item item)
        {
            items.Add(item);
        }

        public void removeItem(Item item)
        {
            items.Remove(item);
        }

        private void initExits(List<Room> exits) 
        {
            for (int x = 0; x < 4; x++)
            {
                if (exits.ElementAtOrDefault(x) == null)
                {
                    this.exits.Add(null);
                }
                else
                {
                    this.exits.Add(exits.ElementAtOrDefault(x));
                }
            }
        }

        public string exitsToString()
        {
            StringBuilder exits = new StringBuilder();

            exits.Append("%W[%Mexits:%y");
            int appended = 0;
            for (int x = 0; x < 4; x++)
            {
                if (this.hasExit(x))
                {
                    exits.Append(" ");
                    exits.Append(Direction.directionToName(x));
                    appended++;
                }
            }

            if (appended < 1)
                exits.Append(" none");

            exits.Append("%W]");

            return exits.ToString();
        }

        public int exitsCount()
        {
            int count = 0;

            for (int x = 0; x < 4; x++)
            {
                if (hasExit(x))
                    count++;
            }
            return count;
        }

        public void sendToRoom(string message)
        {
            foreach (Player p in players)
            {
                p.client.send(message);
            }
        }

        public void sendToRestRoom(string message, Player player)
        {
            foreach (Player p in players)
            {
                if (player != p)
                {
                    p.client.send(message);
                }
            }
        }
    }
}
