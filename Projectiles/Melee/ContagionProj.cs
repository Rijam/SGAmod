using Microsoft.Xna.Framework;
using SGAmod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Melee
{
    public class ContagionProj : ModProjectile
    {
        protected virtual float HoldoutRangeMin => 48f;
        protected virtual float HoldoutRangeMax => 112f;
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = 19;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.hide = true;
            Projectile.ai[0] = 1;
        }
        
        
        public override bool PreAI()
        {
            Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
            Player player = Main.player[Projectile.owner];
            int duration = Main.player[Projectile.owner].itemAnimationMax;
           
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            float progress;
            Projectile.velocity = Vector2.Normalize(Projectile.velocity);
            if (Projectile.timeLeft < duration / 2)
            {
                progress = Projectile.timeLeft / (duration * 0.5f) ;
            }
            else if (Projectile.timeLeft == duration * 0.5f)
            {
                if(Main.myPlayer == Projectile.owner) Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity, Projectile.velocity * 5.5f, ModContent.ProjectileType<AcidSpear>(), (int)((double)Projectile.damage * 0.5), Projectile.knockBack * 0.85f, Projectile.owner, 0f, 0f);
                progress = 0;
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / (duration * 0.5f);
            }

            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
            Projectile.rotation = (float)Math.Atan2(Projectile.Center.Y, Projectile.Center.X) + 2.355f;
            if (Projectile.spriteDirection == -1) Projectile.rotation -= 1.57f;
            if (Main.rand.Next(4) == 0)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<AcidDust>(), 0f, 0f, 254, default(Color), 0.3f);
                Main.dust[dustIndex].velocity += Projectile.velocity * 0.5f;
                Main.dust[dustIndex].velocity *= 0.5f;
            }
            Lighting.AddLight(Projectile.position, 0.3f, 0.5f, 0f);
            return false;
        }
    }
}
