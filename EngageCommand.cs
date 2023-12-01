using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheHouse
{
    public class EngageCommand : Command
    {
        public EngageCommand() : base()
        {
            this.Name = "engage";
        }



        public override bool Execute(Player player)
        {
            {
                if (this.HasSecondWord())
                {
                    if (this.HasThirdWord())
                    {
                        player.engage(this.SecondWord + " " + this.ThirdWord);
                    }
                    else
                    {
                        player.engage(this.SecondWord);
                    }

                }
                else
                {
                    player.WarningMessage("\n What do I engage?");

                }
            }
            return false;
        }
    }
}

