using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using SGAmod.Dusts;
using SGAmod.Buffs.Debuffs;

namespace SGAmod.Projectiles.Melee
{
    public class AcidSpear : ModProjectile
    {
        public override void SetDefaults()
        {
            

            Projectile.width = 7;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 8;
            Projectile.alpha = 200;
            Projectile.timeLeft = 2000;
            Projectile.light = 0.75f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;

        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;

            if (Main.rand.Next(3) == 0)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AcidDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 200, default, 0.7f);
                Main.dust[dustIndex].velocity += Projectile.velocity * 0.3f;
                Main.dust[dustIndex].velocity *= 0.2f;
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 2;
            target.AddBuff(ModContent.BuffType<AcidBurn>(), 100);
        }
    }
    
}
