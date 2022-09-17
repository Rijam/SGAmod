//#define WebmilioCommonsPresent
#define DEBUG
#define DefineHellionUpdate
#define Dimensions


using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Shaders;
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
using System.IO;
using System.Diagnostics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
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
		public SGAmod Instance;

		public SGAmod()
		{

		}

		public override void Load()
		{
			Instance = this;
		}

		public override uint ExtraPlayerBuffSlots => 50;

		public override void Unload()
		{

		}
	}
}