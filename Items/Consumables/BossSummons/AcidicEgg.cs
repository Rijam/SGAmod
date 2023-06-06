#define DefineHellionUpdate

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.GameContent.Events;
using Terraria.Localization;
using Microsoft.Xna.Framework.Audio;
using SGAmod.NPCs.Bosses.SpiderQueen;

namespace SGAmod.Items.Consumables.BossSummons
{
	public class AcidicEgg : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.
			// DisplayName.SetDefault("Acidic Egg");
			// Tooltip.SetDefault("'No words for this...' \nSummons the Spider Queen\nRotten Eggs drop from spiders");
		}
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = Item.CommonMaxStack;
			Item.rare = ItemRarityID.Green;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item44;
			Item.consumable = true;
		}

		public static bool Underground(Entity player) => (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
		public static bool Underground(int here) => (int)((double)((here / 16f) * 2.0) - Main.worldSurface * 2.0) > 0;

		public override bool CanUseItem(Player player)
		{
			//bool underground = (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;

			if (Underground(player) && !NPC.AnyNPCs(ModContent.NPCType<SpiderQueen>()))
			{
				return true;
			}
			else
			{
				if (player == Main.LocalPlayer && !NPC.AnyNPCs(ModContent.NPCType<SpiderQueen>()))
				{
					Main.NewText("There are no spiders here, try using it underground", 30, 200, 30);
				}
				return false;
			}
		}
		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				SoundEngine.PlaySound(SoundID.Roar, player.position);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<SpiderQueen>());
				}
				else
				{
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: ModContent.NPCType<SpiderQueen>());
				}
			}
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.RottenEgg, 1)
				.AddIngredient(ItemID.Cobweb, 25)
				.AddRecipeGroup(SGARecipes.EvilBossMaterials, 5)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}