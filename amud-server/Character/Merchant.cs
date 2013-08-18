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

        public Merchant() :base()
        {
            isMobile = false;
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
            player.client.send(listBuy());
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
                buffer.AppendFormat("\r\n\t%B{0}%W) %w{1,3}%yg %W[ %w{2,8}%W ]  %B{3}%x\r\n",
                                    x,
                                    i.value,
                                    i.name,
                                    i.description);
            }

            return buffer.ToString();
        }
    }
}
