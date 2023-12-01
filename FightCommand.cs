using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class FightCommand : Command
    {


        public FightCommand() : base()
        {
            this.Name = "fight";
        }



        public override bool Execute(Player player)
        {
            {
                if (this.HasSecondWord())
                {
                    if (this.HasThirdWord())
                    {
                        player.fight(this.SecondWord + " " + this.ThirdWord);
                    }
                    else
                    {
                        player.fight(this.SecondWord);
                    }

                }
                else
                {
                    player.WarningMessage("\n What do I fight?");

                }
            }
            return false;
        }
    }
}
