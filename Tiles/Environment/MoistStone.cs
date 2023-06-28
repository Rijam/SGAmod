using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;
using Terraria.Enums;
using Terraria.WorldBuilding;
using ReLogic.Content;

namespace SGAmod.Tiles.Environment
{
	public class MoistStone : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			//Main.tileLighted[Type] = true;
			MinPick = 50;
			HitSound = SoundID.Tink;
			MineResist = 5f;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			//drop = mod.ItemType("Moist Stone");
			AddMapEntry(new Color(100, 160, 100));
		}

		// TODO
		/*public override bool CanExplode(int i, int j)
		{
			return SGAWorld.downedMurk > 1 || SGAWorld.downedCaliburnGuardians > 2;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return SGAWorld.downedMurk > 1 || SGAWorld.downedCaliburnGuardians > 2;
		}*/

		/*
		public override void RandomUpdate(int i, int j)
		{

			for (int xi = 1; xi <= 1; xi += 1)
			{
				if (!Main.tile[i, j - xi].HasTile)
				{
					//if (Main.rand.Next(6) == 1)
					//{
						int[] onts = new int[] { ModContent.TileType<SwampGrassGrow>(), ModContent.TileType<SwampGrassGrow2>(), ModContent.TileType<SwampGrassGrow3>() };
					if (xi<0)
					onts = new int[] { ModContent.TileType<SwampGrassGrowTop>(), ModContent.TileType<SwampGrassGrowTop2>(), ModContent.TileType<SwampGrassGrowTop3>() };

					WorldGen.PlaceObject(i, j - xi, onts[Main.rand.Next(onts.Length)], true);
					//}
				}
			}
		}
		*/
	}
	public class SwampGrassGrow : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			DustType = 40;
			HitSound = SoundID.Grass;
			//TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);

			//TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			//TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorTop = AnchorData.Empty;

			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				ModContent.TileType<MoistStone>()
			};
			TileObjectData.addTile(Type);
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			/*if (Main.tile[i, j - 2].type == ModContent.TileType<MoistStone>())
			{
				spriteEffects = SpriteEffects.FlipVertically;
			}*/
		}

		/*public override bool Drop(int i, int j) // tModPorter Note: Removed. Use CanDrop to decide if an item should drop. Use GetItemDrops to decide which item drops. Item drops based on placeStyle are handled automatically now, so this method might be able to be removed altogether.
		{
			int stage = Main.tile[i, j].TileFrameX / 18;
			if (stage == 2 && Main.rand.Next(5)<1)
			{
				Item.NewItem(i * 16, j * 16, 0, 0, Mod.Find<ModItem>("SwampSeeds").Type,Main.rand.Next(1,4));
			}
			return false;
		}*/

		public override void RandomUpdate(int i, int j)
		{
			if (Main.tile[i, j].TileFrameX == 0)
			{
				Main.tile[i, j].TileFrameX += 18;
			}
			else if (Main.tile[i, j].TileFrameX == 18)
			{
				Main.tile[i, j].TileFrameX += 18;
			}
		}
	}

	public class SwampGrassGrowTop : SwampGrassGrow
	{
		public override string Texture => "SGAmod/Tiles/Environment/SwampGrassGrow";

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			DustType = 40;
			HitSound = SoundID.Grass;
			//TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);

			//TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			//TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				Mod.Find<ModTile>("MoistStone").Type
			};
			TileObjectData.addTile(Type);
		}
	}

	public class SwampGrassGrowTop2 : SwampGrassGrowTop
	{
		public override string Texture => "SGAmod/Tiles/Environment/SwampGrassGrow2";
	}
	public class SwampGrassGrowTop3 : SwampGrassGrowTop
	{
		public override string Texture => "SGAmod/Tiles/Environment/SwampGrassGrow3";
	}

	public class SwampGrassGrow2 : SwampGrassGrow
	{

    }
	public class SwampGrassGrow3 : SwampGrassGrow
	{

	}


	public class MoistSand : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSand[Type] = true;
			Main.tileMerge[Type][59] = true;
			Main.tileMerge[59][Type] = true;
			Main.tileMerge[Type][53] = true;
			Main.tileMerge[53][Type] = true;
			Main.tileMerge[Type][397] = true;
			Main.tileMerge[397][Type] = true;
			Main.tileLighted[Type] = true;
			MinPick = 0;
			//soundType = 21;
			//mineResist = 4f;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			TileID.Sets.Conversion.HardenedSand[Type] = true;
			TileID.Sets.Mud[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = Mod.Find<ModItem>("MoistSand").Type;
			AddMapEntry(new Color(140, 160, 100));
			//SetModCactus(new MudCactus())/* tModPorter Note: Removed. Assign GrowsOnTileId to this tile type in ModCactus.SetStaticDefaults instead */;
		}
	}

	public class MudCactus : ModCactus
	{
		public override void SetStaticDefaults()
		{
			// Makes Example Cactus grow on MoistSand
			GrowsOnTileId = new int[1] { ModContent.TileType<MoistSand>() };
		}

		public override Asset<Texture2D> GetTexture()
		{
			return ModContent.Request<Texture2D>("Tiles/Environment/MudCactus");
		}
		// This would be where the Cactus Fruit Texture would go, if we had one.
		public override Asset<Texture2D> GetFruitTexture()
		{
			return null;
		}
	}
}