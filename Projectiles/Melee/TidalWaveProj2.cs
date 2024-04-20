using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace SGAmod.Projectiles.Melee
{
    public class TidalWaveProj2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 60;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.wet) modifiers.SetCrit();
        }
        public override void AI()
        {
            Point16 loc = new Point16((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);
            if (WorldGen.InWorld(loc.X, loc.Y))
            {
                Tile tile = Main.tile[loc.X, loc.Y];
                if (tile != null) if (tile.LiquidAmount > 64) Projectile.timeLeft += 1;
            }
            if (Main.rand.Next(0,30) < 20)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
                Main.dust[dust].scale = 0.8f;
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity = Projectile.velocity * 0.4f;
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X ) + 1.57f;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
                Main.dust[dust].scale = 1.25f;
                Main.dust[dust].noGravity=false;
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
}
