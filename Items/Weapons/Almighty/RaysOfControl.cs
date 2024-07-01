using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using SGAmod.Items.Materials.Misc;
using ReLogic.Content;

namespace SGAmod.Items.Weapons.Almighty
{
    public class RaysOfControlOrb
    {
        public class OrbParticles
        {
            public Vector2 position;
            public Vector2 speed;
            public float angle;
            public Vector2 scale = Vector2.One;
            public Color color;

            public int timeAdd = 0;
            public int timeLeft = 20;
            public int timeLeftMax = 20;
            public OrbParticles(Vector2 position, Vector2 speed, float angle, Color color)
            {
                this.position = position;
                this.speed = speed;
                this.angle = angle;
                this.color = color;
            }
            public void Update()
            {
                timeLeft--;
                position += speed;
                timeAdd++;
            }
        }

        public static RenderTarget2D orbSurface;

        public static bool hasLoaded = false;
        public static bool oneUpdate = false;
        public static float progress = 0f;

        public static List<OrbParticles> particles = new List<OrbParticles>();
        public static int timeLeft = 0;

        public static void UpdateAll()
        {
            if (particles.Count > 0 || timeLeft > 0) 
            {
                particles = particles.Where(testby => testby.timeLeft > 0).ToList();
                foreach(OrbParticles particle in particles)
                {
                    particle.Update();
                }
            }
        }
        public static void Load()
        {
            if (hasLoaded)
                return;

            orbSurface = new RenderTarget2D(Main.graphics.GraphicsDevice, TextureAssets.BlackTile.Value.Width * 32, TextureAssets.BlackTile.Value.Height * 32, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
            hasLoaded = true;
        }

        public static void Unload()
        {
            if (!hasLoaded) return;

            if (!orbSurface.IsDisposed)
                orbSurface.Dispose();
        }

        public static void DrawToRenderTarget()
        {
            if (Main.dedServ || !hasLoaded || !oneUpdate) 
                return;

            oneUpdate = false;
            if (orbSurface == null || orbSurface.IsDisposed)
                return;

            Texture2D glowOrb = TextureAssets.Item[ModContent.ItemType<StygianCore>()].Value;

            RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

            Main.graphics.GraphicsDevice.SetRenderTarget(orbSurface);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            Effect effect = SGAmod.TextureBlendEffect;

<<<<<<< Updated upstream
            effect.Parameters["Texture"].SetValue(ModContent.Request<Texture2D>("SGAmod/Texture/TiledPerlin").Value);
=======
            effect.Parameters["Texture"].SetValue(ModContent.Request<Texture2D>("SGAmod/Assets/Textures/Effects/TiledPerlin").Value);
>>>>>>> Stashed changes
            effect.Parameters["noiseTexture"].SetValue(glowOrb);
            effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f,1f));
            effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
            effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
            effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));
            effect.Parameters["noiseProgress"].SetValue(Main.GlobalTimeWrappedHourly);
            effect.Parameters["textureProgress"].SetValue(Main.GlobalTimeWrappedHourly * 2f);
            effect.Parameters["noiseBlendPercent"].SetValue(1f);
            effect.Parameters["strength"].SetValue(0.25f);

            foreach (OrbParticles particle in particles)
            {
                float timeLeft = particle.timeLeft / (float)particle.timeLeftMax;
                float strength = MathHelper.Clamp(timeLeft * 3f, 0f, Math.Min(particle.timeAdd * 3f, 1f));

                effect.Parameters["colorTo"].SetValue(particle.color.ToVector4());
                effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());
                effect.Parameters["strength"].SetValue(strength);

                effect.CurrentTechnique.Passes["TextureBlend"].Apply();
                Main.spriteBatch.Draw(glowOrb, orbSurface.Size() / 2f + particle.position, null, Color.White, particle.angle, glowOrb.Size() / 2f, particle.scale, default, 0);
            }
            
            Main.spriteBatch.End();
            Main.graphics.GraphicsDevice.SetRenderTargets(binds);

            progress = 0f;
        }
    }
}
