using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Buffs.Debuffs;
using SGAmod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Ranged
{
    public class AcidBullet : ModProjectile
    {
        private Vector2[] oldPos = new Vector2[6];
        private float[] oldRot = new float[6];

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.ignoreWater = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 5;
            AIType = ProjectileID.Bullet;
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = ProjectileID.CursedBullet;
            return true;
        }

        public override string Texture => "Terraria/Images/Projectile_" + 14;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Ammo/AcidBullet").Value;
            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2;

            for (int k = oldPos.Length - 1; k >= 0; --k)
            {
                Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
                Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
                float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 1f;
                Main.spriteBatch.Draw(tex, drawPos, null, color * alphaz, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void AI()
        {
            if (Main.rand.Next(0, 4) == 1)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AcidDust>());
                Main.dust[dust].scale = 0.5f;
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
            }
            Projectile.position -= Projectile.velocity * 0.8f;

            for (int k = oldPos.Length - 1; k > 0; k--)
            {
                oldPos[k] = oldPos[k - 1];
            }
            oldPos[0] = Projectile.Center;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.Next(0, 3) < 2) target.AddBuff(ModContent.BuffType<AcidBurn>(), 30 * (Main.rand.Next(0, 3) == 1 ? 2 : 1));
        }
    }
}
