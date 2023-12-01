using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class OpenCommand : Command
    {
        public OpenCommand() : base()
        {
            this.Name = "open";
        }
        public override bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Open(this.SecondWord);
            }
            else
            {
                player.WarningMessage("open what?");
            }
            return false;


        }
    }
}