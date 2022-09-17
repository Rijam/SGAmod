using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Tiles.Bars
{
	public class Entrophite : ModTile // TODO: Inherit Fabric with the DrawStatic
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			/*
			Main.tileMerge[Type][(ushort)Mod.Find<ModTile>("Fabric").Type] = true;
			Main.tileMerge[Type][(ushort)Mod.Find<ModTile>("AnicentFabric").Type] = true;
			Main.tileMerge[Type][(ushort)Mod.Find<ModTile>("HopeOre").Type] = true;
			Main.tileMerge[Type][(ushort)Mod.Find<ModTile>("HardenedFabric").Type] = true;
			*/
			TileID.Sets.ChecksForMerge[Type] = true;
			MinPick = 200;
			HitSound = SoundID.Grab;
			MineResist = 3f;
			DustType = DustID.Smoke;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			ItemDrop = ModContent.ItemType<Items.Materials.Bars.Entrophite>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Entrophite");
			AddMapEntry(new Color(30, 0, 25), name);
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}
