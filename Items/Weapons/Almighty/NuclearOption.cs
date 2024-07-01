using Idglibrary;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Items.Consumables.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.Almighty
{
    public class NOPlayer : ModPlayer
    {
        private int _charge = 0;
        public int Charge
        {
            get
            {
                int charger = _charge;
                if (!IdgNPC.bossAlive && !UnlimitedPower)
                    charger = Math.Min(charger, ChargeMax / 2);
                return charger;
            }
            set
            {
                _charge = value;
            }
        }
        public bool UnlimitedPower => Player.HasItem(ModContent.ItemType<Debug10>());

        public int ChargeMax => 100000;
        public float ChargePercent => (float)Charge / (float)ChargeMax;
        public int ChargeSpeed => (int)(((10 + Math.Min(Player.lifeRegen / 3, 10)) * MathHelper.Clamp(Player.lifeRegenTime / 400f, 0f, 5f) * (heldOption ? 1f : 0.25f)) * (UnlimitedPower ? 100f : 1f));

        public bool hasOption => Player.HasItem(ModContent.ItemType<NuclearOption>());
        public bool heldOption => Player.HeldItem.type == ModContent.ItemType<NuclearOption>();
        public override void PostUpdate()
        {
            if (hasOption)
            {
                if(heldOption)
                {
                    float square = 96f * 96f;
                    foreach (Projectile proj in Main.projectile.Where(testby => testby.active && !testby.friendly && testby.hostile && (testby.Center - Player.Center).LengthSquared() < square && testby.GetGlobalProjectile<SGAProjectile>().grazed == false))
                    {
                        proj.GetGlobalProjectile<SGAProjectile>().grazed = true;
                        var snd = SoundEngine.PlaySound(SoundID.Item35 with { Pitch = 0.25f});
                        Charge += 500 + (proj.damage * 15);
                    }
                }
                Charge = (int)MathHelper.Clamp(Charge + ChargeSpeed, 0f, ChargeMax);
            }
            else
            {
                Charge = 0;
            }
        }

    }
    [Autoload(true)]
    public class NuclearOption : Megido,  IRadioactiveDebuffText
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 200;
            Item.width = 48;
            Item.height = 48;
            Item.useTurn = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = 500;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.knockBack = 8;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.consumable = false;
            Item.noMelee = true;
            Item.shootSpeed = 1f;
            Item.maxStack = 1;
            Item.shoot = ModContent.ProjectileType<NuclearOptionProj>();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<NOPlayer>().ChargePercent > 0.5f)
            {
                return true;
            }
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.lifeRegen = 0;
            player.lifeRegenTime = 0;
            float perc = (player.GetModPlayer<NOPlayer>().ChargePercent * player.GetModPlayer<NOPlayer>().ChargePercent);
            Projectile proj = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<NuclearOptionProj>(), (int)(damage * perc), knockback, player.whoAmI);
            proj.ai[1] = player.GetModPlayer<NOPlayer>().ChargePercent;
            player.GetModPlayer<NOPlayer>().Charge = 0;
            proj.netUpdate = true;
            return false;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D inner = ModContent.Request<Texture2D>("SGAmod/Assets/Textures/UI/BoostBar").Value;

            Vector2 slotsize = new Vector2(52f, 52f) * scale;
            //position -= slotsize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position; //+ slotsize * Main.inventoryScale / 2f;
            Vector2 textureOrigin = Vector2.Zero;

            slotsize.X /= 1.0f;
            slotsize.Y = -slotsize.Y / 4f;

            Vector2 HPHeight = new Vector2(1f,1f);

            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, drawPos, null, drawColor, Main.GlobalTimeWrappedHourly, TextureAssets.Item[Item.type].Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);

            Vector2 scalerr = Vector2.One * new Vector2(slotsize.X / 2f, 1f);
            if (Main.rand.Next(100) < 999)
            {
                spriteBatch.Draw(inner, drawPos - new Vector2(slotsize.X / 2, slotsize.Y), new Rectangle(2,0,2,inner.Height), Color.White, 0, textureOrigin, scalerr * HPHeight, SpriteEffects.None, 0f);
                NOPlayer cataply = Main.LocalPlayer.GetModPlayer<NOPlayer>();
                spriteBatch.Draw(inner, drawPos - new Vector2(slotsize.X / 2, slotsize.Y), new Rectangle(2, 0, 2, inner.Height), Color.Turquoise, 0, textureOrigin, scalerr * new Vector2(cataply.ChargePercent, 1f) * HPHeight, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(inner, drawPos - new Vector2(0, slotsize.Y), new Rectangle(0, 2, 2, inner.Height), Color.White, 0, textureOrigin, Main.inventoryScale * HPHeight, SpriteEffects.None, 0f);
            spriteBatch.Draw(inner, drawPos - new Vector2(0, slotsize.Y), new Rectangle(0, 0, 2, inner.Height), Color.White, 0, textureOrigin, Main.inventoryScale * HPHeight, SpriteEffects.FlipHorizontally, 0f);

            return false;

        }
    }

    public class NuclearOptionProj : MorningStarProj
    {
        public List<CloudBoom> raysOfLight = new List<CloudBoom>();

        Vector2 OverallScale => (Vector2.One * 3f * Projectile.ai[1]) * ((float)Math.Pow(Projectile.localAI[0] / 30f, 0.32f));

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
        }
        public override string Texture => "Terraria/Images/Misc/MoonExplosion/Explosion";
        public override void Load()
        {
            SGAmodSystem.PostUpdateEverythingEvent += SGAmod_PostUpdateEverythingEvent;
            
        }
        private void SGAmod_PostUpdateEverythingEvent()
        {
            CataLogo.DrawToRenderTarget();
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!Main.dedServ)
            {
                CataLogo.Load();
                CataLogo.oneUpdate = true;
            }

            Projectile.ai[0]++;
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("SGAmod/Sounds/Custom/MegidoloanSnd").WithVolumeScale(1f).WithPitchOffset(.15f), Projectile.Center);
                SGAmod.AddScreenShake(64f, 2400, Projectile.Center);
            }

            float lenn = 512 * (Projectile.ai[1] * Projectile.ai[1]) + (Projectile.ai[0] < 60 ? Projectile.ai[0] * 25 : 0) + (OverallScale.X * 16f);

            if (Projectile.ai[0] % 10 == 0 && Projectile.timeLeft > 30)
            {
                foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.hostile && !testby.friendly && (testby.Center - Projectile.Center).Length() < lenn))
                {
                    bool canDelete = (proj.ModProjectile != null && ((proj.ModProjectile is INonDestructableProjectile)/* || (proj.ModProjectile is Dimensions.IMineableAsteroid)*/));

                    if (proj.timeLeft > 3 && !proj.GetGlobalProjectile<SGAProjectile>().raindown && proj.whoAmI != Projectile.whoAmI && proj.damage > 0 && proj.hostile && !proj.friendly && (proj.ModProjectile == null && !canDelete))
                    {
                        proj.GetGlobalProjectile<SGAProjectile>().raindown = true;
                        proj.timeLeft = 3;

                        for (int i = 0; i < 24; i++)
                        {
                            Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
                            int dust128 = Dust.NewDust(proj.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, 3.25f - (i / 12f));
                            Main.dust[dust128].noGravity = true;
                            Main.dust[dust128].alpha = 160;
                            Main.dust[dust128].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
                            Main.dust[dust128].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(2f, 5f)) + (i * Vector2.Normalize(proj.Center - Projectile.Center) * 0.075f);
                        }
                    }
                }

                foreach (NPC npc in Main.npc.Where(testby => testby.IsValidEnemy() && (testby.Center - Projectile.Center).Length() < lenn))
                {
                    int damage = Main.DamageVar(Projectile.damage);
                    CheckApoco(ref damage, npc, Projectile);
                    npc.SimpleStrikeNPC(damage, 0, false, 1);
                    player.addDPS(damage);
                    npc.GetGlobalNPC<SGAnpcs>().IrradiatedAmount = Math.Min(npc.GetGlobalNPC<SGAnpcs>().IrradiatedAmount + 30, Projectile.damage * 3);
                    npc.AddBuff(ModContent.BuffType<Buffs.Debuffs.RadioDebuff>(), 60 * 20);

                    if (Projectile.ai[1] >= 1f)
                    {
                        if (ModLoader.TryGetMod("CalamityMod", out Mod calamid))
                        {
                            if (npc.ModNPC != null && npc.ModNPC.Mod.Name == "CalamityMod")
                            {
                                npc.StrikeInstantKill();
                                if (npc.active)
                                {
                                    npc.active = false;
                                   
                                }
                            }
                        }
                    }
                    for (int i = 0; i < 16; i += 1)
                    {
                        Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
                        int num128 = Dust.NewDust(npc.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, 1.50f - (i / 24f));
                        Main.dust[num128].noGravity = true;
                        Main.dust[num128].alpha = 130;
                        Main.dust[num128].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
                        Main.dust[num128].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(6f, 12f)) + Vector2.Normalize(npc.Center - Projectile.Center) * 20f;
                    }
                }
            }
            if (Projectile.timeLeft > 30)
            {
                if (SGAmod.ScreenShake < 10)
                    SGAmod.AddScreenShake(5f, 720 + (Projectile.timeLeft * 4), Projectile.Center);
            }

            float scaleUpeffect = 1f;

            float explodScale = 16f / MathHelper.Clamp(1f + (Projectile.timeLeft / 4f), 0.001f, 100f);
            float cataScale = 8f / MathHelper.Clamp(1f + (Projectile.timeLeft / 4f), 0.001f, 100f);

            for (int i = 0; i < 2; i += 1)
            {
                Vector2 velo = new Vector2(Main.rand.NextFloat(-1f, 1f)) * 0.05f;
                CloudBoom boomer = new CloudBoom(new Vector2(Main.rand.NextFloat(MathHelper.TwoPi), 0), velo, Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
                boomer.scale = (Vector2.One * (1f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f))) * 0.50f;

                boomer.angle = Main.rand.NextFloat(MathHelper.TwoPi);

                raysOfLight.Add(boomer);
            }
            foreach (CloudBoom cb in raysOfLight.Where(testby => testby.timeLeft > 0))
            {
                cb.angle += cb.speed.X;
                cb.position -= cb.speed;
                cb.Update();
            }
            for (int i = 0; i < 32; i += 1)
            {
                Vector2 velo = Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(12f, 18f) * (1f + explodScale + cataScale) * (OverallScale * 0.20f);
                CloudBoom boomer = new CloudBoom(Projectile.Center + (Vector2.Normalize(velo) * explodScale), velo * (0.45f + (scaleUpeffect / 3f)), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
                boomer.scale = (Vector2.One * (1f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f))) * 0.50f;

                boomOfClouds.Add(boomer);
            }
            foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
            {
                cb.Update();
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            float alpha = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);
            float alpha2 = MathHelper.Clamp(Projectile.timeLeft / 20f, 0f, 1f);
            float alpha3 = 1f - MathHelper.Clamp((Projectile.timeLeft - 20f) / 90f, 0f, 1f);
            float alpha4 = 1f - MathHelper.Clamp(Projectile.localAI[0] / 60f, 0f, 1f);
            float alpha5 = MathHelper.Clamp((Projectile.timeLeft - 20f) / 20f, 0f, 1f);

            Texture2D explosionTex = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D lightBeamTex = ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/LightBeam").Value;
            Texture2D glowOrbTex = ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/GlowOrb").Value;

            Vector2 exploorig = new Vector2(explosionTex.Width, explosionTex.Height / 7) / 2f;
            Vector2 lightorig = new Vector2(lightBeamTex.Width, lightBeamTex.Height / 4) / 2f;
            Vector2 orgCenter = glowOrbTex.Size() / 2f;

            float explodScale = 16f / MathHelper.Clamp(1f + (Projectile.timeLeft / 4f), 0.001f, 100f);
            float explodScale2 = 4f / MathHelper.Clamp(1f + (Projectile.timeLeft / 16f), 0.001f, 100f);
            float cataScale = 64f / MathHelper.Clamp(1f + (Projectile.timeLeft / 4f), 0.001f, 100f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            if (alpha4 > 0f)
                Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, Color.Aqua * alpha4 * 1f, 0, orgCenter, OverallScale * (20f * (1f - alpha4)), default, 0);


            Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Turquoise, Color.White, MathHelper.Clamp(explodScale * 2f, 0f, 1f)) * alpha * 0.50f, 0, orgCenter, OverallScale * 3f, default, 0);

            Color boomColor = Color.Aqua;

            Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, boomColor * alpha2, 0, orgCenter, OverallScale * explodScale, default, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            foreach (CloudBoom cb in raysOfLight)
            {
                float timePercent = (cb.timeLeft / (float)cb.timeLeftMax);
                float timePercentBack = 1f - (cb.timeLeft / (float)cb.timeLeftMax);
                float cbAlpha = MathHelper.Clamp(timePercent * 4f, 0f, 1f);
                Color color = Color.DarkTurquoise;
                Vector2 explosionSize = (Vector2.One * 0.20f) + ((Vector2.One * 0.80f) * timePercent) * cb.scale;

                Main.spriteBatch.Draw(lightBeamTex, Projectile.Center - Main.screenPosition, null, color * cbAlpha * alpha * 0.50f, -MathHelper.PiOver2 + cb.angle + (cb.position.X), lightorig, explosionSize * new Vector2(0.5f, 1.5f) * OverallScale, default, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (CloudBoom cb in boomOfClouds)
            {
                float timePercent = (cb.timeLeft / (float)cb.timeLeftMax);
                float timePercentBack = 1f - (cb.timeLeft / (float)cb.timeLeftMax);
                float cbAlpha = MathHelper.Clamp(timePercent * 4f, 0f, 1f);
                Color color = Color.White;
                Vector2 explosionSize = (Vector2.One * 0.20f) + ((Vector2.One * 0.80f) * timePercent) * cb.scale;

                Rectangle rect = new Rectangle(0, (explosionTex.Height / 7) * (int)(timePercentBack * 7), explosionTex.Width, explosionTex.Height / 7);
                Main.spriteBatch.Draw(explosionTex, cb.position - Main.screenPosition, rect, color * cbAlpha * alpha * 0.25f, cb.angle, exploorig / 2f, explosionSize * OverallScale, default, 0);
            }

            CataLogo.Draw(Projectile.Center - Main.screenPosition, alpha * alpha5, new Vector2(3f, 3f) * OverallScale * cataScale);



            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, Color.Turquoise * alpha5 * 1f, 0, orgCenter, OverallScale * cataScale * 0.025f, default, 0);

            Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, Color.White * alpha2 * alpha3, 0, orgCenter, OverallScale * explodScale, default, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            return false;
        }
    }
    public class CataLogo
    {
        public static RenderTarget2D CataSurface;

        public static Effect cataEffect;
        public static Effect radialEffect;
        public static bool hasLoaded = false;
        public static bool oneUpdate = false;

        public static void Load()
        {
            RaysOfControlOrb.Load();

            if (hasLoaded)
                return;
            cataEffect = SGAmod.CataEffect;
            radialEffect = SGAmod.RadialEffect;
            CataSurface = new RenderTarget2D(Main.graphics.GraphicsDevice, TextureAssets.BlackTile.Width() * 32, TextureAssets.BlackTile.Height() * 32, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
            hasLoaded = true;
        }
        public static void Unload()
        {
            RaysOfControlOrb.Unload();

            if (!hasLoaded) return;

            if (!CataSurface.IsDisposed)
            {
                CataSurface.Dispose();
            }
        }
        public static void DrawToRenderTarget()
        {
            RaysOfControlOrb.DrawToRenderTarget();
            if (Main.dedServ || !hasLoaded || !oneUpdate)
                return;
            oneUpdate = false;

            if (CataSurface == null || CataSurface.IsDisposed)
                return;
            BlendState Blending =new BlendState
            {
                ColorSourceBlend = Blend.Zero,
                ColorDestinationBlend = Blend.InverseSourceColor,
                AlphaSourceBlend = Blend.Zero,
                AlphaDestinationBlend =  Blend.InverseSourceColor
            };

            RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

            Main.graphics.GraphicsDevice.SetRenderTarget(CataSurface);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            float edgesize = 0.40f;// + (float)(Math.Sin(Main.GlobalTime * 2f) * 0.10f);
            float ballsize = 0.05f;// + (float)(Math.Sin(Main.GlobalTime) * 0.05f);
            float ballgapsize = 0.05f;// + (float)(Math.Sin(Main.GlobalTime * 1.2f) * 0.02f);

            Effect RadialEffect = radialEffect;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            for (int i = 0; i < 3; i += 1)
            {
                for (float f = -1f; f < 2; f += 2)
                {
                    RadialEffect.Parameters["overlayTexture"].SetValue(ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/Fire").Value);
                    RadialEffect.Parameters["alpha"].SetValue(0.50f);
                    RadialEffect.Parameters["texOffset"].SetValue(new Vector2(f * Main.GlobalTimeWrappedHourly * 0.25f, -Main.GlobalTimeWrappedHourly * 0.575f));
                    RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(3f, 1f + i));
                    RadialEffect.Parameters["ringScale"].SetValue(0.36f);
                    RadialEffect.Parameters["ringOffset"].SetValue(0.16f);
                    RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
                    RadialEffect.Parameters["tunnel"].SetValue(false);

                    RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

                    Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, TextureAssets.BlackTile.Size() * 16f, null, Color.White, 0, TextureAssets.BlackTile.Size() * 0.5f, 96f, SpriteEffects.None, 0f);
                }
            }
            RadialEffect.Parameters["overlayTexture"].SetValue(ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/Fire").Value);
            RadialEffect.Parameters["alpha"].SetValue(5f);
            RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0, -Main.GlobalTimeWrappedHourly * 0.2575f));
            RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(0.5f, 1f));
            RadialEffect.Parameters["ringScale"].SetValue(0.1f);
            RadialEffect.Parameters["ringOffset"].SetValue((ballsize + ballgapsize) * (32f / 96f) * 2.5f);
            RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
            RadialEffect.Parameters["tunnel"].SetValue(false);

            RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, TextureAssets.BlackTile.Size() * 16f, null, Color.White, 0, TextureAssets.BlackTile.Size() * 0.5f, 96f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, Blending, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            cataEffect.Parameters["angleAdd"].SetValue(Main.GlobalTimeWrappedHourly * 1f);
            cataEffect.Parameters["edges"].SetValue(3);
            cataEffect.Parameters["ballSize"].SetValue(ballsize);
            cataEffect.Parameters["edgeSize"].SetValue(edgesize);
            cataEffect.Parameters["ballEdgeGap"].SetValue(ballgapsize);

            cataEffect.CurrentTechnique.Passes["CataLogoInverse"].Apply();

            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 32f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();

            Main.graphics.GraphicsDevice.SetRenderTargets(binds);
        }
        public static void Draw(Vector2 where, float alpha, Vector2 scale)
        {

            Effect RadialEffect = radialEffect;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            RadialEffect.Parameters["overlayTexture"].SetValue(ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/Fire").Value);
            RadialEffect.Parameters["alpha"].SetValue(4f * alpha);
            RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0, Main.GlobalTimeWrappedHourly * 0.575f));
            RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(3f, 0.75f));
            RadialEffect.Parameters["ringScale"].SetValue(0.20f);
            RadialEffect.Parameters["ringOffset"].SetValue(0.14f);
            RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
            RadialEffect.Parameters["tunnel"].SetValue(true);

            RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, where, null, Color.White, 0, TextureAssets.BlackTile.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


            Main.spriteBatch.Draw(CataSurface, where, null, Color.White * alpha, 0, (Vector2.One * CataSurface.Size()) / 2f, scale / 18f, SpriteEffects.None, 0f);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


        }
    }
}
