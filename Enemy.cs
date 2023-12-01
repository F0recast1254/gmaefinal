using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TheHouse
{
    public class Enemy : ICombatant
    {
        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ColoredMessage(string message, ConsoleColor newColor)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = newColor;
            OutputMessage(message);
            Console.ForegroundColor = oldColor;
        }

        public void InfoMessage(string message)
        {
            ColoredMessage(message, ConsoleColor.Blue);
        }
        public String name { get; }
        public int health { get; set; }

        public int attack { get; }
        public int defense { get; }
        public int maxHealth { get; }

        public Enemy(String Name, int Health, int Attack, int Defense)
        {
            this.name = Name;
            this.health = Health;
            this.maxHealth = Health;
            this.attack = Attack;
            this.defense = Defense;
        }

    }
}

