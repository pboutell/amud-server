using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace amud_server
{
    [Serializable]
    class Merchant : NPC
    {
        public bool isMobile { get; private set; }

        public Merchant()
        {
            isMobile = false;
            gold = 1000;
        }

        public override void update(DateTime time)
        {
            base.update(time);
        }

        public override void updateMovement(DateTime time)
        {
            if (isMobile)
            {
                base.updateMovement(time);
            }
        }

        public override bool shop(string[] args, Player player)
        {
            StringBuilder buffer = new StringBuilder();
            if (args.ElementAtOrDefault(1) == "buy")
            {
                if (args.ElementAtOrDefault(2) != null)
                {
                    Item item = items.getItemByName(args[2].TrimEnd('\r','\n'));
                    if (item != null)
                    {
                        player.items.addToInventory(item);
                        gold += item.value;
                        player.gold -= item.value;
                        buffer.AppendFormat("\r\n%BYou purchase %W{0} %Bfor %Y{1} %Bgold.", item.description, item.value);
                        say("Thank you for your business.");
                        player.client.send(buffer.ToString());
                    }
                    else
                    {
                        say("I'm sorry, im fresh out of " + args[2].TrimEnd('\r', '\n'));
                    }
                }
            }
            else if (args.ElementAtOrDefault(1) == "sell")
            {
                player.client.send("Selling not allowed yet.");
            }
            else
            {
                player.client.send(listBuy());
            }

            return true;
        }

        private string listBuy()
        {
            StringBuilder buffer = new StringBuilder();
            int x = 1;

            say("Hello! please have a look at my merchandise.");

            buffer.AppendLine("\r\n%YItems Available:");
            foreach (Item i in items.inventory)
            {
                buffer.AppendFormat("\r\n\t%B{0}%W) %w{1,3}%Yg %W[ %w{2,8}%W ]  %B{3}%x\r\n",
                                    x,
                                    i.value,
                                    i.name,
                                    i.description);
            }

            return buffer.ToString();
        }
    }
}
