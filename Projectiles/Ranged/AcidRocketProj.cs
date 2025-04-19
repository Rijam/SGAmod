using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SGAmod.Dusts;
using Idglibrary;
using SGAmod.Buffs.Debuffs;
using Terraria.ID;

namespace SGAmod.Projectiles.Ranged
{
    public class AcidRocketProj : ModProjectile
    {
        double keepspeed = 0.0;
        public Player P;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.ignoreWater = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 200;
            AIType = -1;
            Projectile.aiStyle = -1;
        }
        public override string Texture => "SGAmod/Items/Weapons/Ammo/AcidRocket";

        bool hitonce = false;

        public override bool PreKill(int timeLeft)
        {
            if (!hitonce)
            {
                Projectile.width = 200;
                Projectile.height = 200;
                Projectile.position -= new Vector2(100, 100);
            }
            for (int i = 0; i < 125; i++)
            {
                float randomfloat = Main.rand.NextFloat(1f, 6f);
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000,8000), Main.rand.Next(-8000,8000)); randomcircle.Normalize();

                int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 64, Projectile.Center.Y - 64), 128, 128, ModContent.DustType<AcidDust>());
                Main.dust[dust].scale = 3.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = (Projectile.velocity * (float)(Main.rand.Next(10, 50) * 0.01f)) + (randomcircle * randomfloat);
            }

            int theproj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Explosion>(), (int)((double)Projectile.damage * 1f), Projectile.knockBack, Projectile.owner, 0f, 0f);
            Main.projectile[theproj].DamageType = DamageClass.Ranged;
            IdgProjectile.AddOnHitBuff(theproj, ModContent.BuffType<AcidBurn>(), 120);

            Projectile.velocity = default(Vector2);
            Projectile.type = ProjectileID.GrenadeIII;
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hitonce)
            {
                hitonce = true;
                Projectile.position -= new Vector2(100, 100);
                Projectile.width = 200;
                Projectile.height = 200;
                Projectile.timeLeft = 1;
            }
            target.AddBuff(ModContent.BuffType<AcidBurn>(), 200);
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;

            if (Projectile.ai[0]>20 && Projectile.ai[0]<70) 
            {
                Vector2 speedz = Projectile.velocity;
                Vector2 speedzc = speedz; speedzc.Normalize();
                Projectile.velocity = speedzc * (speedz.Length() + 0.4f);
            }
            for (float i = 0; i < 2.5f; i += 0.75f)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.Next(0, 100) < 15 ? DustID.Torch : ModContent.DustType<AcidDust>());
                Main.dust[dust].scale = 1.15f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = -Projectile.velocity * (float)(Main.rand.Next(20, 50 + (int)(i * 40f)) * 0.01f) / 2f;
            }
            Projectile.timeLeft--;
        }

    }
}
