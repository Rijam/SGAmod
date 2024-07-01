using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SGAmod.Projectiles.NPCS.SPinky;
using SGAmod.Effects;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.CodeAnalysis;
using Terraria.Utilities;

namespace SGAmod.Items.Weapons.Almighty
{
    [Autoload(true)]
    public class Megido : AlmightyWeapon
    {
        public override void SetDefaults()
        {
            
            Item.damage = 150;
            Item.width = 48;
            Item.height = 48;
            Item.useTurn = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = 500;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.knockBack = 8;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.noMelee = true;
            Item.shootSpeed = 2f;
<<<<<<< Updated upstream
            Item.maxStack = 30;
=======
            Item.maxStack = 9999;
>>>>>>> Stashed changes
            Item.shoot = ModContent.ProjectileType<MegidoProj>();
        }
        public bool UseStacks(SGAPlayer sgaply, int time,  int count = 1)
        {
            Player player = sgaply.Player;
            if (Main.rand.Next(100)<20 && sgaply.personaDeck)
            {
                sgaply.Player.QuickSpawnItem(Item.GetSource_Loot(), ModContent.ItemType<TheJoker>(), count);
                SoundEngine.PlaySound(SoundID.Item16.WithPitchOffset(0.20f), sgaply.Player.Center);
                int HPlost = count * 20;

                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Red, HPlost, false, false);
                player.statLife -= HPlost;
                if (player.statLife < 1)
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " had a fatal change of heart"), 1337, 0);
                }

                sgaply.ActionCooldownStack_AddCooldownStack((int)(time * (sgaply.personaDeck ? 0.5f : 1f)), count);

                return false;
            }

            for (int i = 0; i < count; i++)
            {
                if (player.HasItem(ModContent.ItemType<TheJoker>()))
                {
                    player.ConsumeItem(ModContent.ItemType<TheJoker>(), true);
                    player.HealEffect(20);
                    player.netLife = true;
                    player.Heal(20);
                    SoundEngine.PlaySound(new SoundStyle("SGAmod/Sounds/Custom/P5Loot").WithVolumeScale(1f).WithPitchOffset(0.1f), player.Center);
                }
            }
            return sgaply.ActionCooldownStack_AddCooldownStack((int)(time * (sgaply.personaDeck ? 0.5f : 1f)), count);
        }

		public override bool CanUseItem(Player player)
		{
			if (player.GetModPlayer<SGAPlayer>().ActionCooldownStack_AddCooldownStack(100, testOnly: true))
			{
				NPC[] findnpc = SGAUtils.ClosestEnemies(player.Center, 1500, checkWalls: false, checkCanChase: true)?.ToArray();
				if (findnpc != null && findnpc.Length > 0)
					return true;
			}
			return false;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(new SoundStyle("SGAmod/Sounds/Custom/MegidoSnd").WithVolumeScale(.7f).WithPitchOffset(.15f), player.Center);
            UseStacks(player.GetModPlayer<SGAPlayer>(), 60 * 20);

            for (int i = 0; i < 4; i++)
            {
                NPC[] findnpc = SGAUtils.ClosestEnemies(player.Center, 1500, Main.MouseWorld, checkWalls: false, checkCanChase: true)?.ToArray();
                NPC target = findnpc[i % findnpc.Count()];

                Projectile proj = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), player.Center, Vector2.UnitX.RotatedBy(MathHelper.PiOver4 + (i * (MathHelper.TwoPi / 4f))) * 8f, ModContent.ProjectileType<MegidoProj>(), damage, knockback, player.whoAmI, 0, target.whoAmI);
                proj.ai[1] = target.whoAmI;
                proj.netUpdate = true;
            }
            return false;
        }
        
    


        
    }
    public class MegidoProj : PinkyMinionKilledProj
    {
        protected override float ScalePercent => MathHelper.Clamp(Projectile.timeLeft / 10f, 0f, Math.Min(Projectile.localAI[0] / 3f, 0.75f));
        protected override float SpinRate => 0.2f;

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

        public void CheckApoco(ref int damage, NPC npc, Projectile proj, bool always = false)
        {
            float kb = 0f;
            bool crit = false;
            double[] highestApoco = Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().apocalypticalChance.OrderBy(testby => 10000 - testby).ToArray();
            damage += npc.defense / 2;

            NPC.HitInfo hitinfo = new NPC.HitInfo();
            hitinfo.Crit = crit;
            hitinfo.Knockback = kb;

            if (npc.realLife >= 0)
                damage = (int)(damage * 0.1f);

            if (always || Main.rand.NextFloat(100f) < highestApoco[0])
                npc.GetGlobalNPC<ApocalypticalNPCs>().DoApoco(npc, proj, Main.player[Projectile.owner], null, ref damage, ref hitinfo, 2, true);
        }

        public override void ReachedTarget(NPC target)
        {
            Player player = Main.player[Projectile.owner];
            int damage = Main.DamageVar(Projectile.damage) + target.defense / 2;
            CheckApoco(ref damage, target, Projectile);
            target.SimpleStrikeNPC(damage, 0, false, 1);
            SGAmod.AddScreenShake(6f, 2400, target.Center);
            Main.player[Projectile.owner].addDPS(damage);

            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, target.whoAmI, Projectile.damage, 0f, (float)1, 0, 0, 0);

            Projectile.velocity = Vector2.Zero;

            for (int i = 0; i < 24; i++)
            {
                Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
                int dust = Dust.NewDust(Projectile.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, ScalePercent);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].alpha = 180;
                Main.dust[dust].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
                Main.dust[dust].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(2f, 5f));
            }

            SoundEngine.PlaySound(SoundID.Item86 with { Pitch = -0.5f }, Projectile.Center);

            Projectile.ai[0]++;
            Projectile.timeLeft = (int)MathHelper.Clamp(Projectile.timeLeft, 0, 10);
            Projectile.netUpdate = true;
        }

        public override void AI()
        {
            if (startingloc == default)
                startingloc = Projectile.Center;
            Projectile.localAI[0] += 0.25f;

            List<Point> WeightedPoints2 = new List<Point>();

            int index = 0;
            int us = 0;

            NPC findnpc = Main.npc[(int)Projectile.ai[1]];

            if (findnpc != null && findnpc.active)
            {
                Projectile.velocity *= 0.94f;
                if (Projectile.localAI[0] > 8f && Projectile.ai[0] < 1)
                {
                    NPC target = findnpc;
                    int dist = 60 * 60;
                    Vector2 distto = target.Center - Projectile.Center;
                    Projectile.velocity += Vector2.Normalize(distto).RotatedBy((MathHelper.Clamp(1f - (Projectile.localAI[0] - 8f) / 5f, 0f, 1f) * 0.85f) * SpinRate) * 3.2f;
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MathHelper.Clamp(Projectile.velocity.Length(), 8f, 32f + Projectile.localAI[0]);

                    if (Projectile.timeLeft > 10 && Projectile.ai[0] < 1 && distto.LengthSquared() < dist)
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
            if (Projectile.ai[0] > 0)
                Projectile.ai[0]++;
            int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(8f, 8f), 0, 0, DustID.t_Marble, 0, 0, 240, Color.Aqua, ScalePercent);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity * Main.rand.NextFloat(0.1f, 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
<<<<<<< Updated upstream
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
=======
            for (int i = 0; i < Projectile.oldPos.Length; i++)//dumb hack to get the trails to not appear at 0,0
			{
>>>>>>> Stashed changes
                if (Projectile.oldPos[i] == default)
                    Projectile.oldPos[i] = Projectile.position;
            }

<<<<<<< Updated upstream
            TrailHelper trail = new TrailHelper("DefaultPass", ModContent.Request<Texture2D>("SGAmod/Textures/Noise").Value);
=======
            TrailHelper trail = new TrailHelper("DefaultPass", ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/NoiseFix").Value);
>>>>>>> Stashed changes

            Color colorz = Color.Turquoise;
            trail.color = delegate (float percent)
            {
                return Color.Lerp(colorz, Color.DarkCyan, MathHelper.Clamp(Projectile.ai[0] / 7f, 0f, 1f));
            };
            trail.projsize = Projectile.Hitbox.Size() / 2f;
            trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
            trail.trailThickness = 4 + MathHelper.Clamp(Projectile.ai[0], 0f, 30f) / 20f;
            trail.trailThicknessIncrease = 6;
            trail.strength = ScalePercent;
            trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);

<<<<<<< Updated upstream
            trail = new TrailHelper("BasicEffectDarkPass", ModContent.Request<Texture2D>("SGAmod/Textures/TrailEffect").Value);
=======
            trail = new TrailHelper("BasicEffectDarkPass", ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/TrailEffect").Value);
>>>>>>> Stashed changes
            trail.projsize = Projectile.Hitbox.Size() / 2f;
            trail.coordMultiplier = new Vector2(1f, 2f);
            trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -2f);
            trail.trailThickness = 3 + MathHelper.Clamp(Projectile.ai[0], 0f, 30f) / 20f;
            trail.trailThicknessIncrease = 6;
            trail.strength = ScalePercent;
            trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);

            Texture2D mainTex = TextureAssets.Projectile[540].Value;
            float blobSize = (MathHelper.Clamp(Projectile.localAI[0], 0f, 4f) * 0.1f) + (MathHelper.Clamp(Projectile.ai[0], 0f, 30f) * 0.150f);

<<<<<<< Updated upstream
            Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(colorz, Color.Black, 0.4f) * trail.strength, 0, mainTex.Size() / 2f, blobSize, default, 0);
            Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(colorz, Color.White, 0.25f) * 0.75f * trail.strength, 0, mainTex.Size() / 2f, blobSize * 0.75f, default, 0);
=======
            Main.EntitySpriteDraw(mainTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(colorz, Color.Black, 0.4f) * trail.strength, 0, mainTex.Size() / 2f, blobSize, default, 0);
            Main.EntitySpriteDraw(mainTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(colorz, Color.White, 0.25f) * 0.75f * trail.strength, 0, mainTex.Size() / 2f, blobSize * 0.75f, default, 0);
>>>>>>> Stashed changes

            UnifiedRandom random = new UnifiedRandom(Projectile.whoAmI);
            for (float f = 0; f < MathHelper.Pi; f += MathHelper.TwoPi / 22f)
            {
                float angle = random.NextFloat(MathHelper.TwoPi);
                Vector2 loc = Vector2.UnitX.RotatedBy(angle) * (random.NextFloat(6f, 26f) * blobSize);

<<<<<<< Updated upstream
                Main.spriteBatch.Draw(mainTex, Projectile.Center + loc - Main.screenPosition, null, Color.Lerp(Color.Turquoise, Color.Black, 0.5f) * 0.5f * trail.strength, angle, mainTex.Size() / 2f, new Vector2(blobSize / 12f, blobSize / 6f), default, 0);
=======
                Main.EntitySpriteDraw(mainTex, Projectile.Center + loc - Main.screenPosition, null, Color.Lerp(Color.Turquoise, Color.Black, 0.5f) * 0.5f * trail.strength, angle, mainTex.Size() / 2f, new Vector2(blobSize / 12f, blobSize / 6f), default, 0);
>>>>>>> Stashed changes
            }

            return false;
        }
    }
}
