using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class BackCommand : Command
    {
        public BackCommand() : base() //initialization of back command
        {
            this.Name = "back";
        }

        override
        public bool Execute(Player player) //execute command of back command
        {
            if (player._rooms.Count != 0)
            {
                player.Back();
            }
            else
            {
                player.WarningMessage("There is nowhere else to go back to...");
            }
            return false;
        }
    }
}
