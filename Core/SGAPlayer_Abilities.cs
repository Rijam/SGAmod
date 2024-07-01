using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;

namespace SGAmod
{
    public partial class SGAPlayer : ModPlayer
    {
        public bool DashBlink()
        {
            if (noModTeleport || maxBlink < 1 || (Player.mount != null && Player.mount.Active)) return false;

            int previousDash = Player.dashType;
<<<<<<< HEAD
			Player.dashType = 1;
=======
<<<<<<< Updated upstream
            
=======
			Player.dashType = 1;
>>>>>>> Stashed changes
>>>>>>> a400078764b98522fee96ded515f61837496b4c4
            

            if (Math.Abs(Player.dashTime) > 0 && Player.dashDelay < 1 && Player.dashType > 0 && (Player.controlLeft || Player.controlRight))
            {
                int bufftime = 0;
                if(Player.HasBuff(BuffID.ChaosState))
                    bufftime = Player.buffTime[Player.FindBuffIndex(BuffID.ChaosState)];

                if (bufftime < maxBlink && (Player.controlUp))
                {
                    Player.Teleport(Player.Center + new Vector2(Player.dashTime > 0 ? -8 : 0, -20), 1);
                    for (int i = 0; i < 30; i++)
                    {
                        if (Collision.CanHitLine(Player.Center,16,16,Player.Center + new Vector2(Math.Sign(Player.dashTime) * 8, 0), 16, 16))
                        {
                            Player.Center += new Vector2(Math.Sign(Player.dashTime) * 8, 0);
                        }
                        else
                        {
                            Player.Center -= new Vector2(Math.Sign(Player.dashTime) * 16, 0);
                            break;
                        }
                    }
                    Player.Teleport(Player.Center + new Vector2(Player.dashTime > 0 ? -8 : 0, -20), 1);
                    Player.dashTime = 0;
                    Player.dashDelay = 3;
                    Player.AddBuff(BuffID.ChaosState, bufftime + 120);


                    return true;
                }

                Player.dashType = previousDash;
            }
            
            return false;
        }
        
        public void AddElectricCharge(int amount)
        {
            electricCharge += amount;
            
        }
    }
}
