#define DEBUG
#define DefineHellionUpdate
#define Dimensions

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Shaders;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Idglibrary;
using System.IO;
using System.Diagnostics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
<<<<<<< Updated upstream
=======
using Terraria.Graphics.CameraModifiers;
using SGAmod.Items.Weapons.Shields;
using static SGAmod.EffectsSystem;
using SGAmod.Items.Weapons.Almighty;

>>>>>>> Stashed changes
/*
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.World;
*/
/*
using SGAmod.NPCs;
using SGAmod.NPCs.Wraiths;
using SGAmod.NPCs.Hellion;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.NPCs.Murk;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.Cratrosity;
using SGAmod.HavocGear.Items;
using SGAmod.HavocGear.Items.Weapons;
using SGAmod.HavocGear.Items.Accessories;
using SGAmod.Items;
using SGAmod.Items.Weapons;
using SGAmod.Items.Armors;
using SGAmod.Items.Accessories;
using SGAmod.Items.Consumables;
using SGAmod.Items.Weapons.Caliburn;
using SGAmod.Tiles;
using SGAmod.UI;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
using Terraria.GameInput;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Items.Placeable;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Dimensions.NPCs;
using SGAmod.Items.Placeable.Paintings;
using Terraria.ModLoader.Audio;
using System.Net;

#if Dimensions
using SGAmod.Dimensions;
#endif
*/

//using SubworldLibrary;

namespace SGAmod
{

    /*public class Blank : Subworld
    {
        public override int width => 800;
        public override int height => 400;
        public override ModWorld modWorld => SGAWorld.Instance;

        public override SubworldGenPass[] tasks => new SubworldGenPass[]
        {
        new SubworldGenPass("Loading", 1f, progress =>
        {
            progress.Message = "Loading"; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY - 42; //Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
        })
        };

        public override void Load()
        {
            Main.dayTime = true;
            Main.time = 27000;
            Main.worldRate = 0;
        }
    }*/

    public partial class SGAmod : Mod
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
		public static SGAmod Instance;

        public SGAmod()
        {

        }

        public static int ExpertiseCustomCurrencyID;
        public static CustomCurrencySystem ExpertiseCustomCurrencySystem;

		public static readonly BindingFlags UniversalBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

#pragma warning restore CA2211 // Non-constant fields should not be visible

		public override void Load()
        {
            Instance = this;

            ExpertiseCustomCurrencySystem = new ExpertiseCurrency(ModContent.ItemType<Items.Misc.ExpertiseItem>(), 999L);
            ExpertiseCustomCurrencyID = CustomCurrencyManager.RegisterCurrency(ExpertiseCustomCurrencySystem);

			if (!Main.dedServ)
			{
<<<<<<< Updated upstream
				Ref<Effect> screenRef = new(Assets.Request<Effect>("Effects/Shockwave", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
				Filters.Scene["SGAmod:Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["SGAmod:ShockwaveBanshee"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
=======


				//Ref<Effect> screenRef = new(Assets.Request<Effect>("Effects/Shockwave", /*, ReLogic.Content.AssetRequestMode.ImmediateLoad*/).Value);
				Filters.Scene["SGAmod:Shockwave"] = new Filter(new ScreenShaderData(Assets.Request<Effect>("Effects/Shockwave" , ReLogic.Content.AssetRequestMode.ImmediateLoad), "Shockwave"), EffectPriority.VeryHigh);
				Filters.Scene["SGAmod:ShockwaveBanshee"] = new Filter(new ScreenShaderData(Assets.Request<Effect>("Effects/Shockwave" , ReLogic.Content.AssetRequestMode.ImmediateLoad), "Shockwave"), EffectPriority.VeryHigh);
>>>>>>> Stashed changes

				//Overlays.Scene["SGAmod:ScreenExplosions"] = new SGAScreenExplosionsOverlay(); TODO
			}

<<<<<<< Updated upstream
			SGAILHacks.Patch();
=======

				TrailEffect = Assets.Request<Effect>("Effects/trailShaders", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                RadialEffect = Assets.Request<Effect>("Effects/Radial", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                CataEffect = Assets.Request<Effect>("Effects/CataLogo" , ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                TextureBlendEffect = Assets.Request<Effect>("Effects/TextureBlend", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                
            }
            Overlays.Scene["SGAmod:ScreenExplosions"] = new SGAScreenExplosionsOverlay();

            SGAmod.GemColors = new Dictionary<int, Color>();

            GemColors.Add(ItemID.Sapphire, Color.Blue);
            GemColors.Add(ItemID.Ruby, Color.Red);
            GemColors.Add(ItemID.Emerald, Color.Lime);
            GemColors.Add(ItemID.Topaz, Color.Yellow);
            GemColors.Add(ItemID.Amethyst, Color.Purple);
            GemColors.Add(ItemID.Diamond, Color.Aquamarine);
            GemColors.Add(ItemID.Amber, Color.Orange);

            SGAPlayer.ShieldTypes.Clear();
            SGAPlayer.ShieldTypes.Add(ModContent.ItemType<CorrodedShield>(), ModContent.ProjectileType<CorrodedShieldProj>());
            SGAPlayer.ShieldTypes.Add(ModContent.ItemType<CapShield>(), ModContent.ProjectileType<CapShieldProj>());

            SGAILHacks.Patch();

            
>>>>>>> Stashed changes
		}

        public override void Unload()
        {
            SGAmod.ExpertiseCustomCurrencySystem = null;
			SGAILHacks.Unpatch();
<<<<<<< Updated upstream
=======
            if (!Main.dedServ)
            {
                //Items.Weapons.Almighty.CataLogo.Unload();
            }
>>>>>>> Stashed changes
		}
    }
<<<<<<< Updated upstream
=======
    public partial class SGAmodSystem : ModSystem
    {

		public override void PreSaveAndQuit()
		{
			
			Overlays.Scene.Deactivate("SGAmod:ScreenExplosions");
		}
		public delegate void PostUpdateEverythingDelegate();
        public static event PostUpdateEverythingDelegate PostUpdateEverythingEvent;

        public override void PostUpdateEverything()
        {
			Terraria.Cinematics.CinematicManager.Instance.Update(new GameTime());
			RaysOfControlOrb.UpdateAll();
			if (SGAmod._screenShake > 0)
			{
				
				SGAmod._screenShake --;
			}
				
               
            PostUpdateEverythingEvent?.Invoke();

            if (SGAmod.screenExplosions.Count > 0)
            {
                foreach (ScreenExplosion explosion in SGAmod.screenExplosions)
                {
                    explosion.Update();
                }
                SGAmod.screenExplosions = SGAmod.screenExplosions.Where(testby => testby.timeLeft > 0).ToList();

                RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

                Main.graphics.GraphicsDevice.SetRenderTargets(SGAmod.screenExplosionCopy);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

                Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, Vector2.One, default, 0);

                Main.spriteBatch.End();
            }
            SGAWorld.modtimer += 1;
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            SGAInterface.ModifyInterfaceLayers(layers);
        }

		public delegate void ModifyTransformMatrixDelegate(ref SpriteViewMatrix Transform);
		public static event ModifyTransformMatrixDelegate ModifyTransformMatrixEvent;
		public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
		{
			ModifyTransformMatrixEvent?.Invoke(ref Transform);

			if (SGAmod.ScreenShake > 0)
			{
				Main.screenPosition += Main.rand.NextVector2Circular(SGAmod.ScreenShake, SGAmod.ScreenShake);
			}
		}

	}

    
>>>>>>> Stashed changes
}