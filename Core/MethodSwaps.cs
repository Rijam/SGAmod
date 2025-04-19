using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Idglibrary;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Audio;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using System.IO;
using SGAmod.Credits;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Terraria.Server;
using tModPorter;
using Humanizer;
using SGAmod.Buffs.Debuffs;

namespace SGAmod
{
    public class SGAMethodSwaps
    {
        internal static void Apply()
        {
            On_Main.SetDisplayMode += RecreateRenderTargetsOnScreenChange;
            if (/*SGAConfig.Instance.QuestionableDetours*/ true)
            {
                //SGAmod.Instance.Logger.Debug("Loading Monogame detours, these can be disabled in configs");
                On_Main.DoUpdate += OverrideCreditsUpdate;
                On_Main.Draw += Main_Draw;
                On_NPC.AddBuff += SmartBuffs;
                
            }
        }
        private static void Main_Draw(On_Main.orig_Draw orig, Main self, GameTime gameTime)
        {
            // 'orig' is a delegate that lets you call back into the original method.
            // 'self' is the 'this' parameter that would have been passed to the original method.
            if (CreditsManager.CreditsActive && !SGAmod.ForceDrawOverride) 
            { 
                CreditsManager.DrawCredits(gameTime);
                return;
            }
            orig(self, gameTime);
        }
        private static void OverrideCreditsUpdate(On_Main.orig_DoUpdate orig, Main self, ref GameTime gameTime)
        {
            SGAmod.lastTime = gameTime;
            if (CreditsManager.queuedCredits)
            {
                CreditsManager.RollCredits();
                CreditsManager.queuedCredits = false;
            }
            if (CreditsManager.CreditsActive)
            {
                CreditsManager.UpdateCredits(gameTime);
                return;
            }
            orig(self, ref gameTime);
        }
        public static bool RenderTargetCreated = true;

        public static void RecreateRenderTargetsOnScreenChange(On_Main.orig_SetDisplayMode orig, int width, int height, bool fullscreen)
        {
            if (RenderTargetCreated)
            {
                CreateRenderTarget2Ds(width, height, fullscreen,true);
                RenderTargetCreated = false;
            }
            else
            {
                CreateRenderTarget2Ds(width,height, fullscreen);
            }
            
            orig(width, height, fullscreen);
        }
        public delegate void RenderTargetsDelegate();
        public static event RenderTargetsDelegate RenderTargetsEvent;
        public static void CreateRenderTarget2Ds(int width, int height, bool fullscreen, bool initialize = false)
        {
            if (Main.dedServ)
                return;

            if ((!Main.gameInactive && (width != Main.screenWidth || height != Main.screenHeight)) || initialize)
            {
                SGAmod.drawnScreen = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
                SGAmod.drawnScreenAdditiveTextures = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);

                SGAmod.screenExplosionCopy = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);

                RenderTargetsEvent?.Invoke();

            }
        }
        private static void SmartBuffs(On_NPC.orig_AddBuff orig, NPC self, int type, int time, bool quiet)
        {
            if (type == ModContent.BuffType<DankSlow>() && self.buffImmune[BuffID.Poisoned])
                return;

            orig(self, type, time, quiet);
        }
    }
}
