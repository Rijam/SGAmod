using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using SGAmod.Dusts;

namespace SGAmod.Projectiles.Magic
{
    public class SwampWave : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1000;
            Projectile.timeLeft = 60;
            Projectile.light = 0.75f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.penetrate < 996) return false;
            return base.CanHitNPC(target);
        }
        public override void AI()
        {
            Projectile.velocity *= 0.98f;
            Projectile.alpha = 255 - (int)(100*(Projectile.timeLeft / 60f));
            Projectile.scale += 0.02f;
            Projectile.width = (int)(40*Projectile.scale);
            Projectile.height = (int)(40 * Projectile.scale);
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (Main.rand.Next(5) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<MangroveDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 200, default(Color), 0.7f);
                Main.dust[dustIndex].velocity += Projectile.velocity * 0.3f;
                Main.dust[dustIndex].velocity *= 0.2f;
            }
        }
    }
}
