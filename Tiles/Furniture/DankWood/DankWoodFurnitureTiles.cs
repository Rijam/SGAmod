using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Items.Placeable.Furniture.DankWood;
using Terraria.GameContent;
using ReLogic.Content;
using SGAmod.Generation.Overworld;
using Terraria.GameContent.Drawing;

namespace SGAmod.Tiles.Furniture.DankWood
{
	//Walls are located in a different cs file

	#region DankWood Door
	public class DankDoorClosed : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.OpenDoorID[Type] = ModContent.TileType<DankDoorOpen>();
			
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);

			DustType = 184;
			AdjTiles = new int[] { TileID.ClosedDoor };
			
			AddMapEntry(new Color(20, 120, 20), CreateMapEntryName());
			
			TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.ClosedDoor, 0));
			TileObjectData.addTile(Type);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}
		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<DankDoorItem>();
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail || effectOnly)
			{
				foreach (NPC enemy in Main.npc.Where(npc => npc.active && !npc.friendly && npc.DistanceSQ(new Vector2(i * 16, j * 16)) < 200 * 200))
				{
					enemy.AddBuff(BuffID.Confused, 60 * 5);
				}
			}
		}
	}
	// TODO Figure out why the door breaks when openning left.
	public class DankDoorOpen : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileLavaDeath[Type] = true;
			Main.tileNoSunLight[Type] = true;
			TileID.Sets.HousingWalls[Type] = true; // needed for non-solid blocks to count as walls
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.CloseDoorID[Type] = ModContent.TileType<DankDoorClosed>();
			
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			
			DustType = 184;
			AdjTiles = new int[] { TileID.OpenDoor };
			RegisterItemDrop(ModContent.ItemType<DankDoorItem>(), 0);
			TileID.Sets.CloseDoorID[Type] = ModContent.TileType<DankDoorClosed>(); // EM has this twice?

			AddMapEntry(new Color(20, 120, 20), Language.GetText("MapObject.Door"));

			TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.OpenDoor, 0));
			TileObjectData.addTile(Type);

		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<DankDoorItem>();
		}
	}
	#endregion
	#region DankWood Block
	public class DankWoodBlock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			AddMapEntry(new Color(20, 120, 20));
			DustType = 184;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	#endregion
	#region DankWood Platform
	public class DankWoodPlatformTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.Platforms[Type] = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 27;
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.newTile.UsesCustomCanPlace = false;
			TileObjectData.newTile.LavaDeath = true;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(new Color(20, 120, 20));
			DustType = 184;
			AdjTiles = new int[] { TileID.Platforms };
		}

		public override void PostSetDefaults()
		{
			Main.tileNoSunLight[Type] = false;
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	#endregion
	#region Dank Wood Workbench
	public class DankWoodWorkbench : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.WorkBenches };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	#endregion
	#region Dank Wood Chair
	public class DankWoodChair : ModTile
	{
		public const int NextStyleHeight = 40; // Calculated by adding all CoordinateHeights + CoordinatePaddingFix.Y applied to all of them + 2
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.CanBeSatOnForNPCs[Type] = true; // Facilitates calling ModifySittingTargetInfo for NPCs
			TileID.Sets.CanBeSatOnForPlayers[Type] = true; // Facilitates calling ModifySittingTargetInfo for Players
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1); // Facing right will use the second texture style
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.Chairs };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range
		}

		public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
		{
			// It is very important to know that this is called on both players and NPCs, so do not use Main.LocalPlayer for example, use info.restingEntity
			Tile tile = Framing.GetTileSafely(i, j);

			//info.directionOffset = info.restingEntity is Player ? 6 : 2; // Default to 6 for players, 2 for NPCs
			//info.visualOffset = Vector2.Zero; // Defaults to (0,0)

			info.TargetDirection = -1;
			if (tile.TileFrameX != 0)
			{
				info.TargetDirection = 1; // Facing right if sat down on the right alternate (added through addAlternate in SetStaticDefaults earlier)
			}

			// The anchor represents the bottom-most tile of the chair. This is used to align the entity hitbox
			// Since i and j may be from any coordinate of the chair, we need to adjust the anchor based on that
			info.AnchorTilePosition.X = i; // Our chair is only 1 wide, so nothing special required
			info.AnchorTilePosition.Y = j;

			if (tile.TileFrameY % NextStyleHeight == 0)
			{
				info.AnchorTilePosition.Y++; // Here, since our chair is only 2 tiles high, we can just check if the tile is the top-most one, then move it 1 down
			}
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			{ // Avoid being able to trigger it from long range
				player.GamepadEnableGrappleCooldown();
				player.sitting.SitDown(player, i, j);
			}

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			{ // Match condition in RightClick. Interaction should only show if clicking it does something
				return;
			}

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.DankWood.DankWoodChair>();

			if (Main.tile[i, j].TileFrameX / 18 < 1)
			{
				player.cursorItemIconReversed = true;
			}
		}
	}
	#endregion
	#region Dank Wood Toilet
	public class DankWoodToilet : ModTile
	{
		public const int NextStyleHeight = 40; // Calculated by adding all CoordinateHeights + CoordinatePaddingFix.Y applied to all of them + 2
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.CanBeSatOnForNPCs[Type] = true; // Facilitates calling ModifySittingTargetInfo for NPCs
			TileID.Sets.CanBeSatOnForPlayers[Type] = true; // Facilitates calling ModifySittingTargetInfo for Players
			TileID.Sets.DisableSmartCursor[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, 2);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;

			// The following 3 lines are needed if you decide to add more styles and stack them vertically
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleHorizontal = true;

			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1); // Facing right will use the second texture style
			TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.Chairs, TileID.Toilets };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range
		}
		public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
		{
			// It is very important to know that this is called on both players and NPCs, so do not use Main.LocalPlayer for example, use info.restingEntity
			Tile tile = Framing.GetTileSafely(i, j);

			//info.directionOffset = info.restingEntity is Player ? 6 : 2; // Default to 6 for players, 2 for NPCs
			//info.visualOffset = Vector2.Zero; // Defaults to (0,0)

			info.TargetDirection = -1;

			if (tile.TileFrameX != 0)
			{
				info.TargetDirection = 1; // Facing right if sat down on the right alternate (added through addAlternate in SetStaticDefaults earlier)
			}

			// The anchor represents the bottom-most tile of the chair. This is used to align the entity hitbox
			// Since i and j may be from any coordinate of the chair, we need to adjust the anchor based on that
			info.AnchorTilePosition.X = i; // Our chair is only 1 wide, so nothing special required
			info.AnchorTilePosition.Y = j;

			if (tile.TileFrameY % NextStyleHeight == 0)
			{
				info.AnchorTilePosition.Y++; // Here, since our chair is only 2 tiles high, we can just check if the tile is the top-most one, then move it 1 down
			}

			// Here we add a custom fun effect to this tile that vanilla toilets do not have. This shows how you can type cast the restingEntity to Player and use visualOffset as well.
			if (info.RestingEntity is Player player && player.HasBuff(BuffID.Stinky))
			{
				info.VisualOffset = Main.rand.NextVector2Circular(2, 2);
			}
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			{ // Avoid being able to trigger it from long range
				player.GamepadEnableGrappleCooldown();
				player.sitting.SitDown(player, i, j);
			}

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			{ // Match condition in RightClick. Interaction should only show if clicking it does something
				return;
			}

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.DankWood.DankWoodToilet>();

			if (Main.tile[i, j].TileFrameX / 18 < 1)
			{
				player.cursorItemIconReversed = true;
			}
		}

		public override void HitWire(int i, int j)
		{
			// Spawn the toilet effect here when triggered by a signal
			Tile tile = Main.tile[i, j];

			int spawnX = i;
			int spawnY = j - (tile.TileFrameY % NextStyleHeight) / 18;

			Wiring.SkipWire(spawnX, spawnY);
			Wiring.SkipWire(spawnX, spawnY + 1);

			if (Wiring.CheckMech(spawnX, spawnY, 60))
			{
				Projectile.NewProjectile(Wiring.GetProjectileSource(spawnX, spawnY), spawnX * 16 + 8, spawnY * 16 + 12, 0f, 0f, ProjectileID.ToiletEffect, 0, 0f, Main.myPlayer);
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.rand.NextBool(360))
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.PooFly, new ParticleOrchestraSettings
				{
					PositionInWorld = new Vector2(i * 16 + 8, j * 16 + 8),
					MovementVector = Vector2.Zero
				});
			}
		}
	}
	#endregion
	#region Dank Wood Table
	public class DankWoodTable : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.Tables };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	#endregion
	#region Dank Wood Dresser
	public class DankWoodDresser : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileContainer[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.BasicDresser[Type] = true;
			TileID.Sets.AvoidedByNPCs[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileID.Sets.IsAContainer[Type] = true;
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

			AdjTiles = new int[] { TileID.Dressers };
			//DustType = 

			// Names
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName(), MapChestName);

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] {
				TileID.MagicalIceBlock,
				TileID.Boulder,
				TileID.BouncyBoulder,
				TileID.LifeCrystalBoulder,
				TileID.RollingCactus
			};
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
		}

		public override LocalizedText DefaultContainerName(int frameX, int frameY)
		{
			return CreateMapEntryName();
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
		{
			width = 3;
			height = 1;
			extraY = 0;
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			int left = Main.tile[i, j].TileFrameX / 18;
			left %= 3;
			left = i - left;
			int top = j - Main.tile[i, j].TileFrameY / 18;
			if (Main.tile[i, j].TileFrameY == 0)
			{
				Main.CancelClothesWindow(true);
				Main.mouseRightRelease = false;
				player.CloseSign();
				player.SetTalkNPC(-1);
				Main.npcChatCornerItem = 0;
				Main.npcChatText = "";
				if (Main.editChest)
				{
					SoundEngine.PlaySound(SoundID.MenuTick);
					Main.editChest = false;
					Main.npcChatText = string.Empty;
				}
				if (player.editedChestName)
				{
					NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f);
					player.editedChestName = false;
				}
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					if (left == player.chestX && top == player.chestY && player.chest != -1)
					{
						player.chest = -1;
						Recipe.FindRecipes();
						SoundEngine.PlaySound(SoundID.MenuClose);
					}
					else
					{
						NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, top);
						Main.stackSplit = 600;
					}
				}
				else
				{
					player.piggyBankProjTracker.Clear();
					player.voidLensChest.Clear();
					int chestIndex = Chest.FindChest(left, top);
					if (chestIndex != -1)
					{
						Main.stackSplit = 600;
						if (chestIndex == player.chest)
						{
							player.chest = -1;
							Recipe.FindRecipes();
							SoundEngine.PlaySound(SoundID.MenuClose);
						}
						else if (chestIndex != player.chest && player.chest == -1)
						{
							player.OpenChest(left, top, chestIndex);
							SoundEngine.PlaySound(SoundID.MenuOpen);
						}
						else
						{
							player.OpenChest(left, top, chestIndex);
							SoundEngine.PlaySound(SoundID.MenuTick);
						}
						Recipe.FindRecipes();
					}
				}
			}
			else
			{
				Main.playerInventory = false;
				player.chest = -1;
				Recipe.FindRecipes();
				player.SetTalkNPC(-1);
				Main.npcChatCornerItem = 0;
				Main.npcChatText = "";
				Main.interactedDresserTopLeftX = left;
				Main.interactedDresserTopLeftY = top;
				Main.OpenClothesWindow();
			}
			return true;
		}

		// This is not a hook, this is just a normal method used by the MouseOver and MouseOverFar hooks to avoid repeating code.
		public static void MouseOverNearAndFarSharedLogic(Player player, int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			left -= tile.TileFrameX % 54 / 18;
			if (tile.TileFrameY % 36 != 0)
			{
				top--;
			}
			int chestIndex = Chest.FindChest(left, top);
			player.cursorItemIconID = -1;
			if (chestIndex < 0)
			{
				player.cursorItemIconText = Language.GetTextValue("LegacyDresserType.0");
			}
			else
			{
				string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY); // This gets the ContainerName text for the currently selected language

				if (Main.chest[chestIndex].name != "")
				{
					player.cursorItemIconText = Main.chest[chestIndex].name;
				}
				else
				{
					player.cursorItemIconText = defaultName;
				}
				if (player.cursorItemIconText == defaultName)
				{
					player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.DankWood.DankWoodDresser>();
					player.cursorItemIconText = "";
				}
			}
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}

		public override void MouseOverFar(int i, int j)
		{
			Player player = Main.LocalPlayer;
			MouseOverNearAndFarSharedLogic(player, i, j);
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			MouseOverNearAndFarSharedLogic(player, i, j);
			if (Main.tile[i, j].TileFrameY > 0)
			{
				player.cursorItemIconID = ItemID.FamiliarShirt;
				player.cursorItemIconText = "";
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Chest.DestroyChest(i, j);
		}

		public static string MapChestName(string name, int i, int j)
		{
			int left = i;
			int top = j;
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}

			if (tile.TileFrameY != 0)
			{
				top--;
			}

			int chest = Chest.FindChest(left, top);
			if (chest < 0)
			{
				return Language.GetTextValue("LegacyDresserType.0");
			}

			if (Main.chest[chest].name == "")
			{
				return name;
			}

			return name + ": " + Main.chest[chest].name;
		}
	}
	#endregion
	#region Dank Wood Bed
	public class DankWoodBed : ModTile
	{
		public const int NextStyleHeight = 38; //Calculated by adding all CoordinateHeights + CoordinatePaddingFix.Y applied to all of them + 2

		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
			TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
			TileID.Sets.IsValidSpawnPoint[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair); // Beds count as chairs for the purpose of suitable room creation

			//DustType = 
			AdjTiles = new int[] { TileID.Beds };

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); // this style already takes care of direction for us
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
			TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}

		public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
		{
			// Because beds have special smart interaction, this splits up the left and right side into the necessary 2x2 sections
			width = 2; // Default to the Width defined for TileObjectData.newTile
			height = 2; // Default to the Height defined for TileObjectData.newTile
			//extraY = 0; // Depends on how you set up frameHeight and CoordinateHeights and CoordinatePaddingFix.Y
		}

		public override void ModifySleepingTargetInfo(int i, int j, ref TileRestingInfo info)
		{
			// Default values match the regular vanilla bed
			// You might need to mess with the info here if your bed is not a typical 4x2 tile
			info.VisualOffset.Y += 4f; // Move player down a notch because the bed is not as high as a regular bed
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int spawnX = (i - (tile.TileFrameX / 18)) + (tile.TileFrameX >= 72 ? 5 : 2);
			int spawnY = j + 2;

			if (tile.TileFrameY % NextStyleHeight != 0)
			{
				spawnY--;
			}

			if (!Player.IsHoveringOverABottomSideOfABed(i, j))
			{ // This assumes your bed is 4x2 with 2x2 sections. You have to write your own code here otherwise
				if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
				{
					player.GamepadEnableGrappleCooldown();
					player.sleeping.StartSleeping(player, i, j);
				}
			}
			else
			{
				player.FindSpawn();

				if (player.SpawnX == spawnX && player.SpawnY == spawnY)
				{
					player.RemoveSpawn();
					Main.NewText(Language.GetTextValue("Game.SpawnPointRemoved"), byte.MaxValue, 240, 20);
				}
				else if (Player.CheckSpawn(spawnX, spawnY))
				{
					player.ChangeSpawn(spawnX, spawnY);
					Main.NewText(Language.GetTextValue("Game.SpawnPointSet"), byte.MaxValue, 240, 20);
				}
			}

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (!Player.IsHoveringOverABottomSideOfABed(i, j))
			{
				if (player.IsWithinSnappngRangeToTile(i, j, PlayerSleepingHelper.BedSleepingMaxDistance))
				{ // Match condition in RightClick. Interaction should only show if clicking it does something
					player.noThrow = 2;
					player.cursorItemIconEnabled = true;
					player.cursorItemIconID = ItemID.SleepingIcon;
				}
			}
			else
			{
				player.noThrow = 2;
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.DankWood.DankWoodBed>();
			}
		}
	}
	#endregion
	#region Dank Wood Sofa
	public class DankWoodSofa : ModTile
	{
		public const int NextStyleHeight = 40; // Calculated by adding all CoordinateHeights + CoordinatePaddingFix.Y applied to all of them + 2
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.CanBeSatOnForNPCs[Type] = true; // Facilitates calling ModifySittingTargetInfo for NPCs
			TileID.Sets.CanBeSatOnForPlayers[Type] = true; // Facilitates calling ModifySittingTargetInfo for Players
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.Chairs };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance); // Avoid being able to trigger it from long range
		}

		public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info)
		{
			// It is very important to know that this is called on both players and NPCs, so do not use Main.LocalPlayer for example, use info.restingEntity
			Tile tile = Framing.GetTileSafely(i, j);

			//info.directionOffset = info.restingEntity is Player ? 6 : 2; // Default to 6 for players, 2 for NPCs
			//info.visualOffset = Vector2.Zero; // Defaults to (0,0)

			info.TargetDirection = 1;
			if (tile.TileFrameX != 0)
			{
				info.TargetDirection = -1; // Facing right if sat down on the right alternate (added through addAlternate in SetStaticDefaults earlier)
			}

			// The anchor represents the bottom-most tile of the chair. This is used to align the entity hitbox
			// Since i and j may be from any coordinate of the chair, we need to adjust the anchor based on that
			info.AnchorTilePosition.X = i; // Our chair is only 1 wide, so nothing special required
			info.AnchorTilePosition.Y = j;

			if (tile.TileFrameY % NextStyleHeight == 0)
			{
				info.AnchorTilePosition.Y++; // Here, since our chair is only 2 tiles high, we can just check if the tile is the top-most one, then move it 1 down
			}
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			{ // Avoid being able to trigger it from long range
				player.GamepadEnableGrappleCooldown();
				player.sitting.SitDown(player, i, j);
			}

			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;

			if (!player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance))
			{ // Match condition in RightClick. Interaction should only show if clicking it does something
				return;
			}

			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.DankWood.DankWoodSofa>();

			if (Main.tile[i, j].TileFrameX / 18 < 1)
			{
				player.cursorItemIconReversed = true;
			}
		}
	}
	#endregion
	#region Dank Wood Lantern
	internal class DankWoodLantern : ModTile
	{
		public override void SetStaticDefaults()
		{
			// Main.tileFlame[Type] = true; This breaks it.
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AddMapEntry(new Color(253, 221, 3), CreateMapEntryName());
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topY = j - tile.TileFrameY / 18 % 3;
			short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
			Main.tile[i, topY].TileFrameX += frameAdjustment;
			Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
			Wiring.SkipWire(i, topY);
			Wiring.SkipWire(i, topY + 1);
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 0)
			{
				// We can support different light colors for different styles here: switch (tile.frameY / 54)
				r = 1f;	 //1f more moody
				g = 0.75f;  //0.47f
				b = 0.30f;  //0.0f
			}
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
			{
				Tile tile = Main.tile[i, j];
				short frameX = tile.TileFrameX;
				short frameY = tile.TileFrameY;
				if (Main.rand.NextBool(40) && frameX == 0)
				{
					int style = frameY / 54;
					if (frameY / 18 % 3 == 0)
					{
						int dustChoice = -1;
						if (style == 0)
						{
							dustChoice = 6; // A flame dust.
						}
						// We can support different dust for different styles here
						if (dustChoice != -1)
						{
							int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustChoice, 0f, 0f, 100, default, 1f);
							if (!Main.rand.NextBool(3))
							{
								Main.dust[dust].noGravity = true;
							}
							Main.dust[dust].velocity *= 0.3f;
							Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1.5f;
						}
					}
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (i % 2 == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			Tile tile = Main.tile[i, j];
			int width = 16;
			int offsetY = 0;
			int height = 16;
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;
			TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);
			Texture2D flameTexture = ModContent.Request<Texture2D>("SGAmod/Tiles/Furniture/DankWood/DankWoodLantern_Flame").Value; // We could also reuse Main.FlameTexture[] textures, but using our own texture is nice.

			ulong num190 = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
			// We can support different flames for different styles here: int style = Main.tile[j, i].frameY / 54;
			for (int c = 0; c < 7; c++)
			{
				float shakeX = Utils.RandomInt(ref num190, -10, 11) * 0.15f;
				float shakeY = Utils.RandomInt(ref num190, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, width, height), new Color(100, 100, 100, 0), 0f, default, 1f, effects, 0f);
			}
		}
	}
	#endregion
	#region Dank Wood Lamp
	internal class DankWoodLamp : ModTile
	{
		public override void SetStaticDefaults()
		{
			// Main.tileFlame[Type] = true; This breaks it.
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
			TileObjectData.newTile.WaterDeath = true;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(253, 221, 3), CreateMapEntryName());
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topY = j - tile.TileFrameY / 18 % 3;
			short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
			Main.tile[i, topY].TileFrameX += frameAdjustment;
			Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
			Main.tile[i, topY + 2].TileFrameX += frameAdjustment;
			Wiring.SkipWire(i, topY);
			Wiring.SkipWire(i, topY + 1);
			Wiring.SkipWire(i, topY + 2);
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 0)
			{
				// We can support different light colors for different styles here: switch (tile.frameY / 54)
				r = 1f;	 //1f more moody
				g = 0.75f;  //0.47f
				b = 0.30f;  //0.0f
			}
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
			{
				Tile tile = Main.tile[i, j];
				short frameX = tile.TileFrameX;
				short frameY = tile.TileFrameY;
				if (Main.rand.NextBool(40) && frameX == 0)
				{
					int style = frameY / 54;
					if (frameY / 18 % 3 == 0)
					{
						int dustChoice = -1;
						if (style == 0)
						{
							dustChoice = 6; //flame dust
						}
						// We can support different dust for different styles here
						if (dustChoice != -1)
						{
							int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustChoice, 0f, 0f, 100, default, 1f);
							if (!Main.rand.NextBool(3))
							{
								Main.dust[dust].noGravity = true;
							}
							Main.dust[dust].velocity *= 0.3f;
							Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1.5f;
						}
					}
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (i % 2 == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			Tile tile = Main.tile[i, j];
			int width = 16;
			int offsetY = 0;
			int height = 16;
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;
			TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);
			Texture2D flameTexture = ModContent.Request<Texture2D>("SGAmod/Tiles/Furniture/DankWood/DankWoodLamp_Flame").Value; // We could also reuse Main.FlameTexture[] textures, but using our own texture is nice.

			ulong num190 = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
			// We can support different flames for different styles here: int style = Main.tile[j, i].frameY / 54;
			for (int c = 0; c < 7; c++)
			{
				float shakeX = Utils.RandomInt(ref num190, -10, 11) * 0.15f;
				float shakeY = Utils.RandomInt(ref num190, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, width, height), new Color(100, 100, 100, 0), 0f, default, 1f, effects, 0f);
			}
		}
	}
	#endregion
	#region Dank Wood Bookshelf
	public class DankWoodBookcase : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.Bookcases };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	#endregion
	#region Dank Wood Torch
	public class DankWoodTorch : ModTile
	{
		private Asset<Texture2D> flameTexture;
		public override void SetStaticDefaults()
		{
			// Properties
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			Main.tileNoAttach[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileWaterDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.DisableSmartInteract[Type] = true;
			TileID.Sets.Torch[Type] = true;

			DustType = DustID.Torch;
			AdjTiles = new int[] { TileID.Torches };

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Torches, 0));
			TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(253, 221, 3), CreateMapEntryName());

			// Assets
			if (!Main.dedServ)
			{
				flameTexture = ModContent.Request<Texture2D>("SGAmod/Tiles/Furniture/DankWood/DankWoodTorch_Flame");
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = Main.rand.Next(1, 3);

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;

			// We can determine the item to show on the cursor by getting the tile style and looking up the corresponding item drop.
			int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
			player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type, style);
		}

		public override float GetTorchLuck(Player player)
		{
			// GetTorchLuck is called when there is an ExampleTorch nearby the client player
			// In most use-cases you should return 1f for a good luck torch, or -1f for a bad luck torch.
			// You can also add a smaller amount (eg 0.5) for a smaller postive/negative luck impact.
			// Remember that the overall torch luck is decided by every torch around the player, so it may be wise to have a smaller amount of luck impact.
			// Multiple example torches on screen will have no additional effect.

			// Positive and negative luck are accumulated separately and then compared to some fixed limits in vanilla to determine overall torch luck.
			// Postive luck is capped at 1, any value higher won't make any difference and negative luck is capped at 2.
			// A negative luck of 2 will cancel out all torch luck bonuses.

			// The influence positive torch luck can have overall is 0.1 (if positive luck is any number less than 1) or 0.2 (if positive luck is greater than or equal to 1)

			bool inDankShrineBiome = player.InModBiome<DankShrine>();
			return inDankShrineBiome ? 0.1f : -0.1f;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX < 66)
			{
				r = 1f;	 //1f more moody
				g = 0.75f;  //0.47f
				b = 0.30f;  //0.0f
			}
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			// This code slightly lowers the draw position if there is a solid tile above, so the flame doesn't overlap that tile. Terraria torches do this same logic.
			offsetY = 0;

			if (WorldGen.SolidTile(i, j - 1))
			{
				offsetY = 4;
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			// The following code draws multiple flames on top our placed torch.

			int offsetY = 0;

			if (WorldGen.SolidTile(i, j - 1))
			{
				offsetY = 4;
			}

			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);

			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i); // Don't remove any casts.
			Color color = new(255, 119, 0, 0);
			int width = 20;
			int height = 20;
			var tile = Main.tile[i, j];
			int frameX = tile.TileFrameX;
			int frameY = tile.TileFrameY;

			for (int k = 0; k < 7; k++)
			{
				float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
				float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

				spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + xx, j * 16 - (int)Main.screenPosition.Y + offsetY + yy) + zero, new Rectangle(frameX, frameY, width, height), color, 0f, default, 1f, SpriteEffects.None, 0f);
			}
		}
	}
	#endregion
	#region Dank Wood Clock
	public class DankWoodClock : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.CoordinateHeights = new[]
			{
				16,
				16,
				16,
				16,
				16
			};
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.GrandfatherClocks };
		}
		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}
		public override bool RightClick(int x, int y)
		{
			string text = "AM";
			//Get current weird time
			double time = Main.time;
			if (!Main.dayTime)
			{
				//if it's night add this number
				time += 54000.0;
			}
			//Divide by seconds in a day * 24
			time = time / 86400.0 * 24.0;
			//Dunno why we're taking 19.5. Something about hour formatting
			time = time - 7.5 - 12.0;
			//Format in readable time
			if (time < 0.0)
			{
				time += 24.0;
			}
			if (time >= 12.0)
			{
				text = "PM";
			}
			int intTime = (int)time;
			//Get the decimal points of time.
			double deltaTime = time - intTime;
			//multiply them by 60. Minutes, probably
			deltaTime = (int)(deltaTime * 60.0);
			//This could easily be replaced by deltaTime.ToString()
			string text2 = string.Concat(deltaTime);
			if (deltaTime < 10.0)
			{
				//if deltaTime is eg "1" (which would cause time to display as HH:M instead of HH:MM)
				text2 = "0" + text2;
			}
			if (intTime > 12)
			{
				//This is for AM/PM time rather than 24hour time
				intTime -= 12;
			}
			if (intTime == 0)
			{
				//0AM = 12AM
				intTime = 12;
			}
			//Whack it all together to get a HH:MM format
			var newText = string.Concat("Time: ", intTime, ":", text2, " ", text);
			Main.NewText(newText, 255, 240, 20);
			return true;
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Main.SceneMetrics.HasClock = true;
			}
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	#endregion
	#region Dank Wood Piano
	public class DankWoodPiano : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.Pianos };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	#endregion
	#region Dank Wood Sink
	public class DankWoodSink : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = false;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16};
			TileObjectData.addTile(Type);
			//AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.Sinks };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	#endregion
	#region Dank Wood Bathtub
	public class DankWoodBathtub : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2); //this style already takes care of direction for us
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(99, 82, 54), CreateMapEntryName());
			AdjTiles = new int[] { TileID.Bathtubs };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}
	}
	#endregion
	#region Novite Candle
	internal class NoviteCandle : ModTile
	{
		public override void SetStaticDefaults()
		{
			// Main.tileFlame[Type] = true; This breaks it.
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
			TileObjectData.newTile.CoordinateHeights = new int[1] { 20 };
			TileObjectData.newTile.DrawYOffset = -4;
			TileObjectData.newTile.WaterDeath = false ;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AdjTiles = new int[] { TileID.Candles };

			AddMapEntry(new Color(253, 221, 3), CreateMapEntryName());
		}

		public override bool RightClick(int i, int j) //Candle gets destroyed when right clicked.
		{
			//Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Placeable.SGAmod.NoviteCandle>());
			WorldGen.KillTile(i, j);
			return true;
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int topY = j - tile.TileFrameY / 18 % 3;
			short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);
			Main.tile[i, topY].TileFrameX += frameAdjustment;
			Wiring.SkipWire(i, topY);
			NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Placeable.Furniture.DankWood.NoviteCandle>();
		}
		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 0)
			{
				// We can support different light colors for different styles here: switch (tile.frameY / 54)
				r = 1f;	 //1f more moody
				g = 0.75f;  //0.47f
				b = 0.30f;  //0.0f
			}
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
			{
				Tile tile = Main.tile[i, j];
				short frameX = tile.TileFrameX;
				short frameY = tile.TileFrameY;
				if (Main.rand.NextBool(40) && frameX == 0)
				{
					int style = frameY / 54;
					if (frameY / 18 % 3 == 0)
					{
						int dustChoice = -1;
						if (style == 0)
						{
							dustChoice = 6; //flame dust
						}
						// We can support different dust for different styles here
						if (dustChoice != -1)
						{
							int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustChoice, 0f, 0f, 100, default, 1f);
							if (!Main.rand.NextBool(3))
							{
								Main.dust[dust].noGravity = true;
							}
							Main.dust[dust].velocity *= 0.3f;
							Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1.5f;
						}
					}
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (i % 2 == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}

			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			Tile tile = Main.tile[i, j];
			int width = 16;
			int offsetY = -4;
			int height = 20;
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;
			TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);
			Texture2D flameTexture = ModContent.Request<Texture2D>("SGAmod/Tiles/Furniture/DankWood/NoviteCandle_Flame").Value; // We could also reuse Main.FlameTexture[] textures, but using our own texture is nice.

			ulong num190 = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
			// We can support different flames for different styles here: int style = Main.tile[j, i].frameY / 54;
			for (int c = 0; c < 7; c++)
			{
				float shakeX = Utils.RandomInt(ref num190, -10, 11) * 0.15f;
				float shakeY = Utils.RandomInt(ref num190, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, width, height), new Color(100, 100, 100, 0), 0f, default, 1f, effects, 0f);
			}
		}
	}
	#endregion
	#region Novite Chandelier
	internal class NoviteChandelier : ModTile
	{
		public override void SetStaticDefaults()
		{
			// Main.tileFlame[Type] = true; This breaks it.
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileWaterDeath[Type] = false;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);

			// We need to change the 3x3 default to allow only placement anchored to top rather than on bottom. Also, the 1,1 means that only the middle tile needs to attach
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			// This is so we can place from above.
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AddMapEntry(new Color(253, 221, 3), CreateMapEntryName());
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int x = i - Main.tile[i, j].TileFrameX / 18 % 3;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 3;
			short frameAdjustment = (short)(tile.TileFrameX >= 54 ? -54 : 54);
			Main.tile[x, y].TileFrameX += frameAdjustment;
			Main.tile[x, y + 1].TileFrameX += frameAdjustment;
			Main.tile[x, y + 2].TileFrameX += frameAdjustment;
			Main.tile[x + 1, y].TileFrameX += frameAdjustment;
			Main.tile[x + 1, y + 1].TileFrameX += frameAdjustment;
			Main.tile[x + 1, y + 2].TileFrameX += frameAdjustment;
			Main.tile[x + 2, y].TileFrameX += frameAdjustment;
			Main.tile[x + 2, y + 1].TileFrameX += frameAdjustment;
			Main.tile[x + 2, y + 2].TileFrameX += frameAdjustment;
			Wiring.SkipWire(x, y);
			Wiring.SkipWire(x, y + 1);
			Wiring.SkipWire(x, y + 2);
			Wiring.SkipWire(x + 1, y);
			Wiring.SkipWire(x + 1, y + 1);
			Wiring.SkipWire(x + 1, y + 2);
			Wiring.SkipWire(x + 2, y);
			Wiring.SkipWire(x + 2, y + 1);
			Wiring.SkipWire(x + 2, y + 2);
			NetMessage.SendTileSquare(-1, x, y + 1, 3, TileChangeType.None);
		}

		/*public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}*/

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 0)
			{
				// We can support different light colors for different styles here: switch (tile.frameY / 54)
				r = 1f;	 //1f more moody
				g = 0.75f;  //0.47f
				b = 0.30f;  //0.0f
			}
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
			{
				Tile tile = Main.tile[i, j];
				short frameX = tile.TileFrameX;
				short frameY = tile.TileFrameY;
				if (Main.rand.NextBool(40) && frameX == 0)
				{
					int style = frameY / 54;
					if (frameY / 18 % 3 == 0)
					{
						int dustChoice = -1;
						if (style == 0)
						{
							dustChoice = 6; // A flame dust.
						}
						// We can support different dust for different styles here
						if (dustChoice != -1)
						{
							int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustChoice, 0f, 0f, 100, default, 1f);
							if (!Main.rand.NextBool(3))
							{
								Main.dust[dust].noGravity = true;
							}
							Main.dust[dust].velocity *= 0.3f;
							Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1.5f;
						}
					}
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SpriteEffects effects = SpriteEffects.None;

			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			Tile tile = Main.tile[i, j];
			int width = 48;
			int offsetY = 0;
			int height = 48;
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;
			TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);
			Texture2D flameTexture = ModContent.Request<Texture2D>("SGAmod/Tiles/Furniture/DankWood/NoviteChandelier_Flame").Value; // We could also reuse Main.FlameTexture[] textures, but using our own texture is nice.

			ulong num190 = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
			// We can support different flames for different styles here: int style = Main.tile[j, i].frameY / 54;
			for (int c = 0; c < 7; c++)
			{
				float shakeX = Utils.RandomInt(ref num190, -10, 11) * 0.15f;
				float shakeY = Utils.RandomInt(ref num190, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, width, height), new Color(100, 100, 100, 0), 0f, default, 1f, effects, 0f);
			}
		}
	}
	#endregion
	#region Novite Candelabra
	internal class NoviteCandelabra : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Origin = new Point16(1, 0);
			TileObjectData.newTile.WaterDeath = false;
			TileObjectData.newTile.WaterPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AddMapEntry(new Color(253, 221, 3), CreateMapEntryName());
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int x = i - Main.tile[i, j].TileFrameX / 18 % 2;
			int y = j - Main.tile[i, j].TileFrameY / 18 % 2;
			short frameAdjustment = (short)(tile.TileFrameX >= 36 ? -36 : 36);
			Main.tile[x, y].TileFrameX += frameAdjustment;
			Main.tile[x, y + 1].TileFrameX += frameAdjustment;
			Main.tile[x + 1, y].TileFrameX += frameAdjustment;
			Main.tile[x + 1, y + 1].TileFrameX += frameAdjustment;
			Wiring.SkipWire(x, y);
			Wiring.SkipWire(x, y + 1);
			Wiring.SkipWire(x + 1, y);
			Wiring.SkipWire(x + 1, y + 1);
			NetMessage.SendTileSquare(-1, x, y + 1, 3, TileChangeType.None);
		}

		/*public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}*/

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX == 0)
			{
				// We can support different light colors for different styles here: switch (tile.frameY / 54)
				r = 1f;	 //1f more moody
				g = 0.75f;  //0.47f
				b = 0.30f;  //0.0f
			}
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
			{
				Tile tile = Main.tile[i, j];
				short frameX = tile.TileFrameX;
				short frameY = tile.TileFrameY;
				if (Main.rand.NextBool(40) && frameX == 0)
				{
					int style = frameY / 54;
					if (frameY / 18 % 3 == 0)
					{
						int dustChoice = -1;
						if (style == 0)
						{
							dustChoice = 6; // A flame dust.
						}
						// We can support different dust for different styles here
						if (dustChoice != -1)
						{
							int dust = Dust.NewDust(new Vector2(i * 16 + 4, j * 16 + 2), 4, 4, dustChoice, 0f, 0f, 100, default, 1f);
							if (!Main.rand.NextBool(3))
							{
								Main.dust[dust].noGravity = true;
							}
							Main.dust[dust].velocity *= 0.3f;
							Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1.5f;
						}
					}
				}
			}
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SpriteEffects effects = SpriteEffects.None;

			Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			Tile tile = Main.tile[i, j];
			int width = 48;
			int offsetY = 0;
			int height = 48;
			short frameX = tile.TileFrameX;
			short frameY = tile.TileFrameY;
			TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref frameX, ref frameY);
			Texture2D flameTexture = ModContent.Request<Texture2D>("SGAmod/Tiles/Furniture/DankWood/NoviteCandelabra_Flame").Value; // We could also reuse Main.FlameTexture[] textures, but using our own texture is nice.

			ulong num190 = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i);
			// We can support different flames for different styles here: int style = Main.tile[j, i].frameY / 54;
			for (int c = 0; c < 7; c++)
			{
				float shakeX = Utils.RandomInt(ref num190, -10, 11) * 0.15f;
				float shakeY = Utils.RandomInt(ref num190, -10, 1) * 0.35f;
				Main.spriteBatch.Draw(flameTexture, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, width, height), new Color(100, 100, 100, 0), 0f, default, 1f, effects, 0f);
			}
		}
	}
	#endregion
}