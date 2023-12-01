using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class InspectCommand : Command
    {

        public override bool Execute(Player player)
        {
            {
                if (this.HasSecondWord())
                {
                    if (this.HasThirdWord())
                    {
                        player.Inspect(this.SecondWord + " " + this.ThirdWord);
                    }
                    else
                    {
                        player.Inspect(this.SecondWord);
                    }
                }
                else
                {
                    player.WarningMessage("\n Item not found for inspection?");

                }
            }
            return false;
        }

        public InspectCommand() : base()
        {
            this.Name = "inspect";
        }

    }
}