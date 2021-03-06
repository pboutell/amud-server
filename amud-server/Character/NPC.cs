﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace amud_server
{
    [Serializable]
    public class NPC : Character
    {
        public bool isMobile { get; set; }
        [NonSerialized]
        private Logger logger = new Logger();

        public NPC()
        {
        }

        public NPC(string name, string description, Stats stats, World world)
        {
            this.name = name;
            this.description = description;
            this.stats = stats;
            this.world = world;
            this.isMobile = true;
        }

        public virtual void update(DateTime time)
        {
            updateCombat();
            updateMovement(time);

            if (World.randomNumber.Next(150) == 0 && !combat.isFighting)
                say("Hello!");

            if (stats.health <= 0)
            {
                die();
                stats.health = 1;
            }
        }
       
        public virtual void updateCombat()
        {
            StringBuilder buffer = new StringBuilder();

            if (combat.target != null && combat.isFighting == true 
            && combat.target.room == room)
            {
                int damageDone = 0;

                damageDone = combat.attack(combat.target);
                buffer.AppendFormat("{0} attacks you dealing %R{1}%x damage!", 
                                    description, damageDone);
                combat.target.messagePipe.Enqueue(buffer.ToString());
            }
        }

        public virtual void updateMovement(DateTime time)
        {
            Movement move = new Movement();

            if (isMobile)
            {
                if (World.randomNumber.Next(75) == 0)
                {
                    move.walk(World.randomNumber.Next(4), this);
                }
            }
        }

        public virtual bool shop(string[] args, Player player)
        {
            return false;
        }

        public void die()
        {
            characterDie();
            NPC dead = this;

            room.removeNPC(this);
            while (!world.mobs.TryTake(out dead)) ;
        }

        public void say(string message)
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendFormat("\r\n%B{0} says \"%W{1}%B\"", name, message);
            room.sendToRoom(buffer.ToString());
        }
    }
}
