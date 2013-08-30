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
            string clean = args.ElementAtOrDefault(1).TrimEnd('\r', '\n');   
            if (clean == "buy")
            {
                if (args.ElementAtOrDefault(2) != null)
                {
                    buyItem(args[2].TrimEnd('\r', '\n'), player);
                }
                else
                {
                    say("What would you like to purchase?");
                }
            }
            else if (clean == "sell")
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
                                    i.value*2,
                                    i.name,
                                    i.description);
            }

            return buffer.ToString();
        }

        private void buyItem(string search, Player player)
        {
            StringBuilder buffer = new StringBuilder();
            
            Item item = items.getItemByName(search);
            
            if (item != null)
            {
                int sellPrice = item.value * 2;
                if (player.gold > sellPrice)
                {
                    player.items.addToInventory(item);
                    gold += sellPrice;
                    player.gold -= sellPrice;
                    buffer.AppendFormat("\r\n%BYou purchase %W{0} %Bfor %Y{1} %Bgold.", item.description, sellPrice);
                    say("Thank you for your business.");
                    player.client.send(buffer.ToString());
                }
                else
                {
                    say("Please, don't waste my time. Come back when you have some coin.");
                }
            }
            else
            {
                say("I apologize, I am fresh out of " + search);
            }
        }
    }
}
