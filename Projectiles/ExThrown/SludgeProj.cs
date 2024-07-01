using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Buffs.Debuffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.ExThrown
{
    public class SludgeProj : ModProjectile
    {
        float scalePercent => MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, Math.Min(Projectile.localAI[0] / 10f, 1f));

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 400;
            Projectile.light = 0.1f;
            Projectile.extraUpdates = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            AIType = -1;
            Main.projFrames[Projectile.type] = 1;
        }
        public override string Texture => "SGAmod/Projectiles/Melee/MudBlob";
        public override bool? CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            if (Projectile.localAI[1] < 10)
            {
                Projectile.localAI[1] = Main.rand.Next(3) + 100;
            }
            Projectile.ai[1]++;
            Projectile.localAI[1]++;

            Point16 point = new Point16((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);

            if (Projectile.localAI[0] > Projectile.ai[0] && WorldGen.InWorld(point.X, point.Y))
            {
                if (Main.tile[point.X, point.Y].WallType > 0)
                {
                    if (Projectile.localAI[0] < 10000)
                    {
                        Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                        Projectile.localAI[0] += 10000;
                    }

                    Projectile.velocity /= 1.25f;
                    Projectile.rotation += (Projectile.velocity.X + Projectile.velocity.Y) / 8f;
                    Projectile.scale = Math.Min(Projectile.scale + (4 - Projectile.scale)/ 3f, 3f);

                    foreach (NPC enemy in Main.npc.Where(npctest => npctest.active && !npctest.friendly && !npctest.immortal && npctest.Distance(Projectile.Center) < (32 * Projectile.scale * (32 * Projectile.scale))))
                    {
                        if (!enemy.boss)
                            enemy.AddBuff(ModContent.BuffType<DankSlow>(), 20);
                        enemy.AddBuff(BuffID.Oiled, 60 * 1);
                        enemy.AddBuff(BuffID.Confused, 3);
                        enemy.GetGlobalNPC<SGAnpcs>().nonStackingImpaled = Projectile.damage;

                        if (Projectile.ai[1] % 20 == 0)
                            enemy.GetGlobalNPC<SGAnpcs>().AddDamageStack(Projectile.damage / 3, 120);
                    }

                    for (int dust654 = 0; dust654 < 1 + (Projectile.localAI[0] < 10003 ? 10 : 0); dust654++)
                    {
                        Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
                        randomcircle.Normalize();
                        Vector2 ogcircle = randomcircle;
                        randomcircle *= (float)(dust654 / 10.00);
                        int dust655 = Dust.NewDust(Projectile.position + Vector2.UnitX * -20f, Projectile.width + 40, Projectile.height + 40, DustID.ScourgeOfTheCorruptor, Projectile.velocity.X + randomcircle.X * 4f, Projectile.velocity.Y + randomcircle.Y * 4f, 200, new Color(30, 30, 30, 20), 1f);
                        Main.dust[dust655].noGravity = true;
                    }

                    return;

                }
                else
                {
                    if (Projectile.localAI[0] > 8000)
                        Projectile.Kill();
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.velocity.Y += 0.25f;

            int dust126 = Dust.NewDust(Projectile.position - (Vector2.One * 2), Main.rand.Next(Projectile.width + 6), Main.rand.Next(Projectile.height + 6), DustID.ScourgeOfTheCorruptor, 0, 0, 140, new Color(30, 30, 30, 20), 1f);
            Main.dust[dust126].noGravity = true;
            Main.dust[dust126].velocity = Projectile.velocity * 0.5f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawpos = Projectile.Center - Main.screenPosition;
            Texture2D tex = TextureAssets.Projectile[569 + ((int)Projectile.localAI[1] % 3)].Value;
            if (Projectile.localAI[0] < 9000)
            {
                tex = TextureAssets.Projectile[Projectile.type].Value;
                Main.spriteBatch.Draw(tex, drawpos, new Rectangle(0, (Projectile.whoAmI * (tex.Height / 3)) % tex.Height, tex.Width, tex.Height / 3), (lightColor.MultiplyRGB(Color.Brown) * 0.75f) * scalePercent, Projectile.rotation, new Vector2(tex.Width, tex.Height / 3f) / 2f, Projectile.scale, SpriteEffects.None, 0f);
                return false;
            }
            Main.spriteBatch.Draw(tex, drawpos, null, lightColor.MultiplyRGB(Color.Brown) * 0.5f * scalePercent, Projectile.rotation, tex.Size()/2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}
