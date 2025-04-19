using Microsoft.Xna.Framework;

using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using static SGAmod.EffectsSystem;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Effects;
using Terraria.Utilities;
using Terraria.DataStructures;
using SGAmod.Items.Materials.Environment;
using Microsoft.Build.Evaluation;

namespace SGAmod.Items.Weapons.Almighty
{
    public class MorningStar : Megido
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 100;
            Item.width = 46;
            Item.height = 46;
            Item.useTurn = true;
            Item.rare = ItemRarityID.Red;
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
            Item.maxStack = 9999;
            Item.shoot = ModContent.ProjectileType<MorningStarProj>();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<SGAPlayer>().ActionCooldownStack_AddCooldownStack(100, 1, true))
                return true;
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (UseStacks(player.GetModPlayer<SGAPlayer>(), 60 * 80, 1))
            {
                int pushYUp = -1;
                player.FindSentryRestingSpot(Item.shoot, out var worldx, out var worldy, out pushYUp);
                Projectile.NewProjectile(Item.GetSource_FromThis(), new Vector2(worldx, worldy), Vector2.Zero, ModContent.ProjectileType<MorningStarProj>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
		public override void AddRecipes()
		{
			CreateRecipe()
				//.AddIngredient(ModContent.ItemType<Megadoloan>())
				.AddIngredient(ModContent.ItemType<IlluminantEssence>())
				.AddRecipeGroup("SGAmod:CelestialFragments")
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
    public class MorningStarProj : MegidoProj
    {
        public class CloudBoom
        {
            public Vector2 position;
            public Vector2 speed;
            public float angle;
            public int cloudType;
            public Vector2 scale = new Vector2(1f, 1f);

            public int timeLeft = 20;
            public int timeLeftMax = 20;
            public CloudBoom(Vector2 position, Vector2 speed, float angle, int cloudtype)
            {
                this.position = position;
                this.speed = speed;
                this.angle = angle;
                this.cloudType = cloudtype;
            }
            public void Update()
            {
                timeLeft -= 1;
                position += speed;
            }
        }

        public List<CloudBoom> boomOfClouds = new List<CloudBoom>();

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 350;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0] += 1;
            Projectile.localAI[0] += 1;
            if (Projectile.localAI[0] == 1)
            {

                SoundEngine.PlaySound(new SoundStyle("SGAmod/Sounds/Custom/MorningStar").WithVolumeScale(1f).WithPitchOffset(.15f), Projectile.Center);
            }

            if (Projectile.ai[0] == 180)
            {
                ScreenExplosion explode = SGAmod.AddScreenExplosion(Projectile.Center, Projectile.timeLeft, 2f, 1600);
                if (explode != null)
                {
                    explode.warmupTime = 16;
                    explode.decayTime = 64;
                    explode.strengthBasedOnPercent = delegate (float percent)
                    {
                        return 2f + MathHelper.Clamp((percent - 0.5f) * 2f, 0f, 1f) * 1f;
                    };
                }
            }

            /*if (projectile.timeLeft == 300)
			{
				ScreenExplosion explode = SGAmod.AddScreenExplosion(projectile.Center, 300, 1.25f, 2400);
				if (explode != null)
				{
					explode.warmupTime = 200;
					explode.decayTime = 64;
				}
			}*/

            if (Projectile.ai[0] > 180)
            {
                bool endhit = Projectile.timeLeft == 30;
                if (SGAmod.ScreenShake < 16)
                    SGAmod.AddScreenShake(6f, 3200, Projectile.Center);
                if ((Projectile.ai[0] % 10 == 0 && Projectile.timeLeft > 30) || endhit)
                {
                    foreach (NPC enemy in Main.npc.Where(testby => testby.IsValidEnemy()))
                    {
                        Rectangle rect = new Rectangle((int)Projectile.Center.X - 240, (int)Projectile.Center.Y - 1000, 480, 1200);
                        if (endhit)
                            rect = new Rectangle((int)Projectile.Center.X - 1600, (int)Projectile.Center.Y - 1600, 3200, 3200);


                        if (enemy.Hitbox.Intersects(rect))
                        {
                            int damage = (int)((Main.DamageVar((Projectile.damage))) * (endhit ? 5f : 1f));
                            CheckApoco(ref damage, enemy, Projectile, endhit);
                            enemy.SimpleStrikeNPC(damage, 0, false, 1);
                            
                            
                            Main.player[Projectile.owner].addDPS(damage);
                        }
                    }
                }


                float scaleUpeffect = 0.75f + ((float)Math.Pow((Projectile.localAI[0] - 180f) / 160f, 4f));

                for (int i = 0; i < 8; i += 1)
                {
                    CloudBoom boomer = new CloudBoom(Projectile.Center + Main.rand.NextVector2Circular(260f, 120f), Vector2.UnitX.RotatedBy(-Main.rand.NextFloat(MathHelper.Pi)) * Main.rand.NextFloat(20f, 26f) * (0.45f + (scaleUpeffect / 3f)), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
                    boomer.scale = Vector2.One * (0.60f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f));

                    boomOfClouds.Add(boomer);
                }

                boomOfClouds = boomOfClouds.Where(testby => testby.timeLeft > 0).ToList();

                foreach (CloudBoom cb in boomOfClouds)
                {
                    cb.Update();
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = 1f;
            Texture2D statTex = ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/Extra_57b").Value;
            Texture2D beamTex = ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/LightBeam").Value;
            Texture2D glowOrb = ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/GlowOrb").Value;
            Vector2 offsetbeam = new Vector2(beamTex.Width / 2f, beamTex.Height / 4f);

            Vector2 starHalf = statTex.Size() / 2f;
            float timeLeft = MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, 1f);
            float beamAlpha = MathHelper.Clamp((Projectile.localAI[0] - 180) / 20f, 0f, 1f);
            float scaleUpeffect = 0.75f + ((float)Math.Pow((Projectile.localAI[0] - 180f) / 160f, 4f));
            float endalpha = (1f - MathHelper.Clamp((scaleUpeffect - 3f) / 6f, 0f, 1f));


            List<(float, Vector2, float, float, Color)> listofstuff = new List<(float, Vector2, float, float, Color)>();

            UnifiedRandom random = new UnifiedRandom(Projectile.whoAmI);

            //Stars

            for (int i = 10; i < 160; i += 1)
            {
                float progress = (random.NextFloat(1f) +
                    Main.GlobalTimeWrappedHourly * (random.NextFloat(0.04f, 0.075f) * (1f + beamAlpha * 25f))
                ) % 1f;

                Vector2 pos = new Vector2(random.Next(-256, 256), -1200 + (progress * 1500f));
                float alphaentry = (1f - MathHelper.Clamp(((i * 2) - Projectile.localAI[0]) / 60f, 0f, 1f)) * ((float)Math.Sin(progress * MathHelper.Pi));
                float rot = (random.NextFloat(MathHelper.TwoPi) + (random.NextFloat(-0.01f, 0.01f) * Main.GlobalTimeWrappedHourly)) * (1f - beamAlpha);
                Color color = Main.hslToRgb(random.NextFloat(1f), 0.85f, 0.95f) * 0.5f;
                listofstuff.Add((progress, pos, alphaentry, rot, color));
            }

            foreach ((float, Vector2, float, float, Color) entry in listofstuff.OrderBy(testby => testby.Item1))
            {
                if (entry.Item3 > 0)
                    Main.spriteBatch.Draw(statTex, Projectile.Center + entry.Item2 - Main.screenPosition, null, entry.Item5 * entry.Item3 * endalpha * timeLeft, entry.Item4, starHalf / 2f, new Vector2(1f, 1f + beamAlpha * 2f) * 0.50f, default, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            //Sky Beams

            for (int i = 0; i < 10; i += 1)
            {
                float alpha3 = (MathHelper.Clamp((Projectile.localAI[0] - (i * 1.25f)) / 160f, 0f, 1f));
                Vector2 beamscale = new Vector2(1f + Projectile.localAI[0] / 240f, 8f + Projectile.localAI[0] / 320f);
                Vector2 offset = new Vector2(random.NextFloat(-64f, 64f), -640);
                float rot = 0f;

                Main.spriteBatch.Draw(beamTex, Projectile.Center + offset - Main.screenPosition, null, Color.Lerp(Color.White, Color.Aqua, 0.50f) * endalpha * alpha3 * timeLeft * (0.20f + (beamAlpha * 0.05f)), rot, offsetbeam, beamscale, default, 0);

            }

            //Big ass laser comes down!
            if (Projectile.localAI[0] > 180)
            {
                UnifiedRandom randomz = new UnifiedRandom(Projectile.whoAmI);

                float max = 3;
                //3 trails as the lasers
                for (int ii = 0; ii < max; ii += 1)
                {
                    List<Vector2> poses = new List<Vector2>();
                    for (float f = 0; f < 2200; f += 25)
					{
						poses.Add(new Vector2(Projectile.Center.X + (float)Math.Sin((ii * (MathHelper.TwoPi / max)) + (Main.GlobalTimeWrappedHourly * 12f) + (f / 400f)) * 90f, (Projectile.Center.Y - f)));
					}

                    TrailHelper trail = new TrailHelper("BasicEffectAlphaPass", ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/TrailEffect").Value);
                    //UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
                    Color colorz = Color.Aqua;
                    trail.projsize = Projectile.Hitbox.Size() / 2f;
                    trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * randomz.NextFloat(6.2f, 9f));
                    trail.coordMultiplier = new Vector2(1f, randomz.NextFloat(1.5f, 4f));

                    trail.strength = beamAlpha * endalpha * timeLeft * 8f;
                    trail.strengthPow = 2f;
                    trail.doFade = true;

                    trail.color = delegate (float percent)
                    {
                        float alphacol = beamAlpha;
                        return Color.Lerp(Color.Turquoise, colorz, MathHelper.Clamp(Projectile.ai[0] / 7f, 0f, 1f));
                    };


                    float extra = randomz.NextFloat(MathHelper.TwoPi);
                    float randc = randomz.NextFloat(4f, 6f);
                    float randd = randomz.NextFloat(2f, 4f);
                    float rande = 1f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * randomz.NextFloat(1f, 1.25f)) * 0.15f;

                    trail.trailThicknessFunction = delegate (float percent)
                    {
                        float math = (float)Math.Sin((Main.GlobalTimeWrappedHourly * -randc) + (percent * MathHelper.TwoPi * randd) + extra);
                        float beamzz = MathHelper.Clamp((beamAlpha * 2f) - percent, 0f, 1f);

                        return (90f + math * 45f) * (beamzz * rande);
                    };

                    trail.DrawTrail(poses, Projectile.Center);

                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                //Expanding orb at epicenter

                Color orbcolor = Color.Lerp(Color.PaleTurquoise, Color.White, MathHelper.Clamp((scaleUpeffect - 2f) / 6f, 0f, 1f)) * beamAlpha * timeLeft;

                Vector2 halfGlow = glowOrb.Size() / 2f;
                float scaleUpeffect2 = 0.75f + ((float)Math.Pow((Projectile.localAI[0] - 180f) / 240f, 15f));

                Main.spriteBatch.Draw(glowOrb, Projectile.Center - Main.screenPosition, null, orbcolor, 0, halfGlow, (new Vector2(1.45f, 1.15f) * scaleUpeffect) * beamAlpha, default, 0);

                //Smoke and clouds

                foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
                {
                    Texture2D cloudTex = ModContent.Request<Texture2D>("SGAmod/Assets/Textures/HellionClouds/Clouds" + cb.cloudType).Value;
                    float cbalpha = MathHelper.Clamp(cb.timeLeft / (float)cb.timeLeftMax, 0f, 1f);
                    float cloudfadeAlpha = Math.Min((cb.timeLeftMax - cb.timeLeft) / 12f, 1f) * 0.75f;

                    Main.spriteBatch.Draw(cloudTex, cb.position - Main.screenPosition, null, Color.Lerp(Color.Lerp(Color.Aqua, Color.DarkCyan, cbalpha), Color.White, MathHelper.Clamp((scaleUpeffect - 2f) / 3f, 0f, 1f)) * beamAlpha * timeLeft * cbalpha * cloudfadeAlpha * endalpha, cb.angle, cloudTex.Size() / 2f, cb.scale, default, 0);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                //Expanding That covers all in the end

                Main.spriteBatch.Draw(glowOrb, Projectile.Center - Main.screenPosition, null, orbcolor * MathHelper.Clamp((scaleUpeffect - 1f) / 12f, 0f, 1f) * (MathHelper.Clamp((Projectile.timeLeft - 20f) / 20f, 0f, 1f)), 0, halfGlow, (new Vector2(0.8f, 0.6f) * scaleUpeffect2) * beamAlpha, default, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
