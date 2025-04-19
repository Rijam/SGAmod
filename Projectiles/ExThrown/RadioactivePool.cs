using Microsoft.Xna.Framework;
using SGAmod.Buffs.Debuffs;
using SGAmod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace SGAmod.Projectiles.ExThrown
{
    public class RadioactivePool : SludgeProj
    {
        float scalePercent => MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, Math.Min(Projectile.localAI[0] / 10f, 1f));
        public override void SetDefaults()
        {
            Projectile.height = 128;
            Projectile.width = 128;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 160;
            Projectile.DamageType = DamageClass.Default;
            Projectile.light = 0.1f;
            Projectile.extraUpdates = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            AIType = -1;
            Main.projFrames[Projectile.type] = 1;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[1] > 0;
        }
        public override void AI()
        {
            if (Projectile.localAI[1] < 10)
            {
                Projectile.localAI[1] = Main.rand.Next(3) + 100;
            }
            Projectile.localAI[0]++;

            Point16 point = new Point16((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);

            float boomspeed = Projectile.ai[1] > 0 ? 8 : 0;

            if (Projectile.timeLeft > 30 && boomspeed < 1)
            {
                foreach (NPC enemy in Main.npc.Where(npctest => npctest.active && !npctest.friendly && !npctest.immortal && npctest.Distance(Projectile.Center) < Projectile.width * Projectile.scale))
                {
                    SGAnpcs sganpcs = enemy.GetGlobalNPC<SGAnpcs>();
                    sganpcs.nonStackingImpaled = Projectile.damage * 3;
                    sganpcs.impaled += Projectile.damage / 2;
                    sganpcs.IrradiatedAmount = Math.Min(sganpcs.IrradiatedAmount + 1, Projectile.damage * 2);
                    enemy.AddBuff(ModContent.BuffType<RadioDebuff>(), 60 * 18);
                }
            }
            for (int num654 = 0; num654 < 4 + boomspeed * 4; num654++)
            {
                if (boomspeed > 0 || Main.rand.Next(160) < Projectile.timeLeft)
                {
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
                    int num655 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.width), 0, 0, DustID.ScourgeOfTheCorruptor, Projectile.velocity.X + randomcircle.X * (4f), Projectile.velocity.Y + randomcircle.Y * (4f), 150, Color.Lime, 0.5f);
                    Main.dust[num655].noGravity = true;
                }
            }

            for (int num654 = 1; num654 < 3 + boomspeed * 4; num654++)
            {
                if (boomspeed > 0 || Main.rand.Next(160) < Projectile.timeLeft)
                {
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
                    int num655 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.width / 2), 0, 0, ModContent.DustType<RadioDust>(), Projectile.velocity.X + randomcircle.X * (2f), Projectile.velocity.Y + randomcircle.Y * (2f), boomspeed > 0 ? 140 : 220, boomspeed > 0 ? Color.Orange : Color.Lime, 0.5f + (boomspeed / 20f));
                    Main.dust[num655].noGravity = true;
                }
            }

            if (Main.rand.Next(24) < Projectile.timeLeft)
            {
                int num126 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.width / 2), 0, 0, DustID.ScourgeOfTheCorruptor, 0, 0, 140, new Color(30, 30, 30, 20), 1f);
                Main.dust[num126].noGravity = true;
                Main.dust[num126].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-6f, 1f));
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}
