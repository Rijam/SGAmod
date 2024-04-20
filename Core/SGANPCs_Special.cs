using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Buffs.Debuffs;
using SGAmod.Projectiles.ExThrown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod
{
    public partial class SGAnpcs : GlobalNPC
    {
        public void IrradiatedExplosion(NPC npc, int baseDamage)
        {
            if (IrradiatedAmount > 0)
            {
                int proj = Projectile.NewProjectile(NPC.GetSource_None(), npc.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), (npc.boss ? 0 : baseDamage) + IrradiatedAmount, 0, Main.player.OrderBy(playerxy => playerxy.Distance(npc.Center)).ToArray()[0].whoAmI);
                Main.projectile[proj].ai[1] = 1;
                Main.projectile[proj].timeLeft = 2;
                Main.projectile[proj].netUpdate = true;

                SoundEngine.PlaySound(SoundID.DD2_DarkMageSummonSkeleton with { Pitch = -0.525f}, npc.Center);

                if (npc.HasBuff<RadioDebuff>())
                    npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<RadioDebuff>()));
                IrradiatedAmount = 0;
                IrradiatedAmount_ = 0;
            }
        }
        public void AddDamageStack(int damage, int time)
        {
            damageStacks.Add(new DamageStack(damage, time));
        }
    }
}
