using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.GameContent.UI;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.Graphics;
using ReLogic.Graphics;
using SGAmod.Items;
using System.Reflection;
using static SGAmod.EffectsSystem;


namespace SGAmod
{
    internal class SGAUI : UIState
    {
        public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int foundindex = layers.FindIndex(layer => layer.Name == "Vanilla: Resource Bars");
            if (foundindex != -1)
                layers.Insert(foundindex, new LegacyGameInterfaceLayer("SGAmod: HUD", DrawHUD, InterfaceScaleType.UI ));
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            Player locply = Main.LocalPlayer;
            SGAPlayer modply = locply.GetModPlayer<SGAPlayer>();
            float perc = 0; //(float)modply.boosterPowerLeft / (float)modply.boosterPowerLeftMax;
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/Core/ActionCooldown").Value;// ModContent.Request<Texture2D>("BoostBar")
            int offsetY = -texture.Height + SGAConfigClient.Instance.HUDDisplacement;


            if (modply.CooldownStacks != null && modply.CooldownStacks.Count > 0)
            {
                texture = ModContent.Request<Texture2D>("SGAmod/Core/ActionCooldown").Value;
                int drawx = (int)(-texture.Width / 4f);
                int drawy = (int)(48 + offsetY);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(locply.Center.X - Main.screenPosition.X, locply.Center.Y - Main.screenPosition.Y, 0));

                int maxx = Math.Max(modply.MaxCooldownStacks, modply.CooldownStacks.Count);

                for (int q = 0; q < maxx; q++)
                {
                    if (q < modply.CooldownStacks.Count)
                    {
                        float xoffset = (float)q * (float)texture.Width * 0.5f;

                        perc = Math.Min(1f, (float)modply.CooldownStacks[q].timeleft / 30f);
                        float percprev = 0f;
                        Color colormode = Color.White;
                        if (modply.MaxCooldownStacks <= q)
                            colormode = Color.Lerp(Color.White, Color.Red, 0.5f);
                        if (q - 1 >= 0)
                            percprev = Math.Min(1f, (float)modply.CooldownStacks[q - 1].timeleft / 30f);
                        float percent = (float)modply.CooldownStacks[q].timeleft / (float)modply.CooldownStacks[q].maxtime;
                        spriteBatch.Draw(texture, new Vector2(drawx - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawy), null, Color.Lerp(Color.Black, Color.DarkGray, 0.25f) * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);
                        spriteBatch.Draw(texture, new Vector2(drawx - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawy), new Rectangle(0, 0, texture.Width, (int)((float)texture.Height * percent)), colormode * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);

                    }
                }
            }
        }
        public static bool DrawHUD()
        {
            if (Main.gameMenu || SGAmod.Instance == null && !Main.dedServ)
                return false;
            Player locply = Main.LocalPlayer;
            if (locply == null) 
                return false;
            if (locply != null && locply.whoAmI == Main.myPlayer)
            {
                SpriteBatch spriteBatch = Main.spriteBatch;

                if (!locply.dead)
                {
                     SGAmod mod = SGAmod.Instance;
                    SGAPlayer modply = locply.GetModPlayer<SGAPlayer>();
                    float perc = 0; //(float)modply.boosterPowerLeft / (float)modply.boosterPowerLeftMax;
                    Texture2D texture = ModContent.Request<Texture2D>("SGAmod/Core/ActionCooldown").Value;// ModContent.Request<Texture2D>("BoostBar").Value;
                    int offsetY = -texture.Height + SGAConfigClient.Instance.HUDDisplacement;

                    
                    if (modply.CooldownStacks != null && modply.CooldownStacks.Count > 0)
                    {
                        texture = ModContent.Request<Texture2D>("SGAmod/Core/ActionCooldown").Value;
                        int drawx = (int)(-texture.Width / 4f);
                        int drawy = (int)(48 + offsetY);

                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(locply.Center.X - Main.screenPosition.X, locply.Center.Y - Main.screenPosition.Y, 0));

                        int maxx = Math.Max(modply.MaxCooldownStacks, modply.CooldownStacks.Count);

                        for (int q = 0; q < maxx; q++)
                        {
                            if (q < modply.CooldownStacks.Count)
                            {
                                float xoffset = (float)q * (float)texture.Width * 0.5f;

                                perc = Math.Min(1f, (float)modply.CooldownStacks[q].timeleft / 30f);
                                float percprev = 0f;
                                Color colormode = Color.White;
                                if (modply.MaxCooldownStacks <= q)
                                    colormode = Color.Lerp(Color.White, Color.Red, 0.5f);
                                if (q - 1 >= 0)
                                    percprev = Math.Min(1f, (float)modply.CooldownStacks[q - 1].timeleft / 30f);
                                float percent = (float)modply.CooldownStacks[q].timeleft / (float)modply.CooldownStacks[q].maxtime;
                                spriteBatch.Draw(texture, new Vector2(drawx - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawy), null, Color.Lerp(Color.Black, Color.DarkGray, 0.25f) * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);
                                spriteBatch.Draw(texture, new Vector2(drawx - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawy), new Rectangle(0, 0, texture.Width, (int)((float)texture.Height * percent)), colormode * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);

                            }
                        }
                    }
                }

            }

            return true;
        }
    }
    public abstract class SGAInterface
    {
        public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int foundIndex = 0;
            int foundLastIndex = layers.Count - 1;

            for (int k = 0; k < layers.Count; k++)
            {
                if (layers[k].Name == "Vanilla: Resource Bars") 
                {
                foundIndex = k + 1;
                    foundLastIndex = foundIndex;
                    break;
                }
            }

            layers.Insert(foundIndex, new LegacyGameInterfaceLayer("SGAmod:HUD", DrawHUD, InterfaceScaleType.UI));
        }
        public static bool DrawHUD()
        {
            if (Main.gameMenu || SGAmod.Instance == null && !Main.dedServ)
                return false;
            Player locply = Main.LocalPlayer;
            if (locply == null)
                return false;
            if (locply != null && locply.whoAmI == Main.myPlayer)
            {
                SpriteBatch spriteBatch = Main.spriteBatch;

                if (!locply.dead)
                {
                    SGAmod mod = SGAmod.Instance;
                    SGAPlayer modply = locply.GetModPlayer<SGAPlayer>();
                    float perc = 100; //(float)modply.boosterPowerLeft / (float)modply.boosterPowerLeftMax;
                    Texture2D texture = ModContent.Request<Texture2D>("SGAmod/Core/ActionCooldown").Value;// ModContent.Request<Texture2D>("BoostBar").Value;
                    int offsetY = -texture.Height + SGAConfigClient.Instance.HUDDisplacement;


                    if (modply.CooldownStacks != null && modply.CooldownStacks.Count > 0)
                    {
                        texture = ModContent.Request<Texture2D>("SGAmod/Core/ActionCooldown").Value;
                        int drawx = (int)(-texture.Width / 4f);
                        int drawy = (int)(48 + offsetY);

                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(locply.Center.X - Main.screenPosition.X, locply.Center.Y - Main.screenPosition.Y, 0));

                        int maxx = Math.Max(modply.MaxCooldownStacks, modply.CooldownStacks.Count);

                        for (int q = 0; q < maxx; q++)
                        {
                            if (q < modply.CooldownStacks.Count)
                            {
                                float xoffset = (float)q * (float)texture.Width * 0.5f;

                                perc = Math.Min(1f, (float)modply.CooldownStacks[q].timeleft / 30f);
                                float percprev = 0f;
                                Color colormode = Color.White;
                                if (modply.MaxCooldownStacks <= q)
                                    colormode = Color.Lerp(Color.White, Color.Red, 0.5f);
                                if (q - 1 >= 0)
                                    percprev = Math.Min(1f, (float)modply.CooldownStacks[q - 1].timeleft / 30f);
                                float percent = (float)modply.CooldownStacks[q].timeleft / (float)modply.CooldownStacks[q].maxtime;
                                spriteBatch.Draw(texture, new Vector2(drawx - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawy), null, Color.Lerp(Color.Black, Color.DarkGray, 0.25f) * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);
                                spriteBatch.Draw(texture, new Vector2(drawx - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawy), new Rectangle(0, 0, texture.Width, (int)((float)texture.Height * percent)), colormode * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);

                            }
                        }
                    }
                }

            }

            return true;
        }
    }


    [Autoload(Side = ModSide.Client)]
    public class OverlaySystem : ModSystem
    {
        private UserInterface SGAInterface;
        internal SGAUI sgaUI;
        private GameTime UIgametime;

        public override void Load()
        {
            sgaUI= new();
            SGAInterface = new();
            SGAInterface.SetState(sgaUI);

            if (!Main.dedServ)
            {
                SGAInterface = new UserInterface();

                sgaUI = new SGAUI();
                sgaUI.Activate();
                SGAInterface.SetState(sgaUI);
                
            }
        }

        public override void Unload()
        {
            sgaUI = null;
        }
        public override void UpdateUI(GameTime gameTime)
        {
            UIgametime = gameTime;
            if(SGAInterface?.CurrentState != null)
                SGAInterface.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int foundindex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (foundindex != -1)
                layers.Insert(foundindex, new LegacyGameInterfaceLayer("SGAmod: HUD", delegate
                {
                    SGAUI.DrawHUD();
                    return SGAUI.DrawHUD();
                }, InterfaceScaleType.UI));
        }
    }

    public class SGAScreenExplosionsOverlay : Overlay
    {
        public SGAScreenExplosionsOverlay() : base(EffectPriority.VeryHigh, RenderLayers.All) { }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible())
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

                foreach (ScreenExplosion explosion in SGAmod.screenExplosions)
                {
                    float warmupTime = Math.Min(explosion.time / explosion.warmupTime, 1f);
                    float timeLeft = MathHelper.Clamp(explosion.timeLeft / explosion.decayTime, 0f, warmupTime );

                    float timeLeftDirect = explosion.time / (float)explosion.timeLeftMax;
                    if (explosion.timeLeft < 2)
                        continue;

                    float maxstr = explosion.strength;
                    if (explosion.strengthBasedOnPercent != default)
                        maxstr = explosion.strengthBasedOnPercent(timeLeftDirect);

                    Vector2 halfOfScreen = SGAmod.screenExplosionCopy.Size() / 2f;

                    Vector2 center = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
                    Vector2 screenCenter = Main.screenPosition + center;
                    Vector2 dist = (screenCenter - explosion.where);

                    float distFade = MathHelper.Clamp((explosion.distance - dist.Length()) / 480f, 0f, 1f);

                    for (float str = 1f; str < 1f + (maxstr * warmupTime); str += 0.05f)
                    {
                        Vector2 offset = center + dist * (str - 1f);

                        float fadeLater = (1f - ((str - 1f) / (maxstr - 1f)));

                        spriteBatch.Draw(SGAmod.screenExplosionCopy, offset, null, Color.White * distFade *fadeLater * explosion.alpha * timeLeft, 0, halfOfScreen, 1f + ((str - 1f)*explosion.perscreenscale * timeLeft), SpriteEffects.None, 0f);
                    }
                   
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.Identity);
            }
            
        }
        public override void Activate(Vector2 position, params object[] args)
        {
        }
        public override void Deactivate(params object[] args)
        {
        }

        public override bool IsVisible()
        {
            bool draw = false;
            if (!Main.gameMenu && Main.LocalPlayer != null && SGAmod.Instance != null && SGAmod.screenExplosions.Count > 0) 
            {
                draw = true;
            }
            else
            {
                Overlays.Scene.Deactivate("SGAmod:ScreenExplosions");
            }
            return draw;
        }
    }
}