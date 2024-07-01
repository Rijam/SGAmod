using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.NPCs;
using Terraria.Audio;
using System.Linq;
using SGAmod.Effects;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using Terraria.GameContent;

namespace SGAmod.Projectiles.NPCS.SPinky
{
    //No, im not doing pinky (yet), this just so happens to be needed somewhere else
    public class PinkyMinionKilledProj : ModProjectile
    {
        protected virtual float ScalePercent => MathHelper.Clamp(Projectile.timeLeft / 10f, 0f, Math.Min(Projectile.localAI[0] / 3f, 0.75f));
        protected virtual int EnemyType => ModContent.NPCType<SPinkyTrue>();
        protected virtual float SpinRate => 1f;

        Vector2 startingloc = default;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }
        public override string Texture => "SGAmod/Projectiles/BoulderBlast";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override bool? CanDamage()
        {
            return false;
        }
        public virtual void ReachedTarget(NPC target)
        {
            target.SimpleStrikeNPC((int)Projectile.damage, 0 , knockBack: 1);
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, target.whoAmI, Projectile.damage, 0f, (float)1, 0, 0, 0);
            }

            for (int i = 0; i < 32; i++)
            {
                Vector2 position = new Vector2(6,6) + new Vector2(Main.rand.Next(12), Main.rand.Next(12));
                int dust = Dust.NewDust(Projectile.Center + position, 0, 0, DustID.t_Marble, 0, 0, 240, Color.Pink, ScalePercent);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].color = Color.Pink;
                Main.dust[dust].velocity = (Vector2.Normalize(position) * 6f) + (Projectile.velocity * 2.5f);
            }

            SoundEngine.PlaySound(SoundID.Item86 with { Pitch = 1.5f}, Projectile.Center);

            Projectile.ai[0]++;
            Projectile.timeLeft = (int)MathHelper.Clamp(Projectile.timeLeft, 0, 10);
            Projectile.netUpdate = true;
        }
        public override void AI()
        {
            if (startingloc == default)
                startingloc = Projectile.Center;
            Projectile.localAI[0] += 0.25f;

            List<Point> weightedPoints2 = new List<Point>();

            NPC[] findNPC = SGAUtils.ClosestEnemies(Projectile.Center, 1500, checkWalls: false, checkCanChase: false)?.ToArray();
            findNPC = findNPC != null ? findNPC.Where(testby => testby.type == EnemyType).ToArray() : null;

            if (findNPC != null && findNPC.Count() > 0 && findNPC[0].type == EnemyType)
            {
                Projectile.velocity *= 0.94f;
                if (Projectile.localAI[0] > 8f)
                {
                    NPC target = findNPC[0];
                    int dist = 60 * 60;
                    Vector2 distto = target.Center - Projectile.Center;
                    Projectile.velocity += Vector2.Normalize(distto).RotatedBy((MathHelper.Clamp(1f - (Projectile.localAI[0] - 8f) / 5f, 0f, 1f) * 0.85f) * SpinRate) * 3.2f;
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MathHelper.Clamp(Projectile.velocity.Length(), 0f, 32f + Projectile.localAI[0]);

                    if (Projectile.timeLeft < 10 && Projectile.ai[0] < 1 && distto.LengthSquared() < dist)
                    {
                        ReachedTarget(target);
                    }
                }
                

            }
            else
            {
                Projectile.timeLeft = (int)MathHelper.Clamp(Projectile.timeLeft, 0, 10);
            }
            Projectile.velocity *= 0.97f;
            if (Projectile.localAI[0] > 0)
                Projectile.ai[0]++;

            int dust = Dust.NewDust(Projectile.position - new Vector2(2, 2), Main.rand.Next(4), Main.rand.Next(4), DustID.t_Marble, 0, 0, 240, Color.Pink, ScalePercent);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity * 0.5f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)//dumb hack to get the trails to not appear at 0,0
            {
                if (Projectile.oldPos == default)
                    Projectile.oldPos[i] = Projectile.position;
            }

            TrailHelper trail = new TrailHelper("DefaultPass", ModContent.Request<Texture2D>("SGAmod/Textures/Noise").Value);
            UnifiedRandom rando = new UnifiedRandom(Projectile.whoAmI);
            float colorz = rando.NextFloat();
            trail.color = delegate (float percent)
            {
                return Color.Lerp(Main.hslToRgb((colorz + (percent / 4f)) % 1f, 1f, 0.75f), Color.Pink, MathHelper.Clamp(Projectile.ai[0] / 7f, 0f, 1f));
            };
            trail.projsize = Projectile.Hitbox.Size() / 2f;
            trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * - 1f);
            trail.trailThickness = 4 + MathHelper.Clamp(Projectile.ai[0], 0f, 30f);
            trail.trailThicknessIncrease = 6;
            trail.strength = ScalePercent;
            trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);

            Texture2D mainTex = TextureAssets.Item[ItemID.Gel].Value;

            Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Main.hslToRgb(colorz % 1f, 1f, 0.75f) * trail.strength, 0, mainTex.Size() / 2f, 2f + MathHelper.Clamp(Projectile.ai[0], 0f, 30) * 0.1f, default, 0);

            return false;
        }
    }
}
