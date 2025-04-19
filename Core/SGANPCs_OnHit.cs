using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using SGAmod.Buffs.Debuffs;
using Terraria.ID;
using System.Drawing;
using Terraria.WorldBuilding;
using static Terraria.NPC;

// If it was in OnHit in the SGAnpcs_Methods file, put it in here.
namespace SGAmod
{
    public partial class SGAnpcs : GlobalNPC
    {
        private void OnHit(NPC npc, Player player, int damage, float knockback, bool crit, Item item, Projectile projectile, bool isproj = false)
        {
            SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();

            if (moddedplayer.alkalescentHeart)
            {
                //throwing crit chance not added.
                int[] maxcrit = { (int)player.GetCritChance(DamageClass.Melee), (int)player.GetCritChance(DamageClass.Ranged), (int)player.GetCritChance(DamageClass.Magic) };
                Array.Sort(maxcrit);
                Array.Reverse(maxcrit);
                if (crit || (projectile != null && projectile.minion && Main.rand.Next(0, 100) < maxcrit[0]))
                {
                    Point point = new(0, 0);
                    point.X = !npc.HasBuff(BuffID.Poisoned) ? BuffID.Poisoned : (!npc.HasBuff(BuffID.Venom) ? BuffID.Venom : (!npc.HasBuff(ModContent.BuffType<AcidBurn>()) ? ModContent.BuffType<AcidBurn>() : -1));
                    if (point.X > -1)
                    {
                        point.Y = point.X == ModContent.BuffType<AcidBurn>() ? 45 : point.X == BuffID.Venom ? 200 : point.X == BuffID.Poisoned ? 300 : 0;
                        npc.AddBuff(point.X, point.Y);
                    }
                }

            }
        }
        private StatModifier ModifyDamage(NPC npc, Player player, ref StatModifier sourcedamage, Item item, Projectile projectile) 
        {
            StatModifier damage = sourcedamage;
            SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();
            if (moddedplayer.alkalescentHeart)
            {
                int venomTier;
                if (npc.HasBuff(ModContent.BuffType<AcidBurn>())) venomTier = 3;
                else if (npc.HasBuff(BuffID.Venom)) venomTier = 2;
                else if (npc.HasBuff(BuffID.Poisoned)) venomTier = 1;
                else venomTier = 0;

                damage *= (1f + (0.05f * venomTier));


            }
            sourcedamage = damage;
            return sourcedamage;
        }
    }
}
