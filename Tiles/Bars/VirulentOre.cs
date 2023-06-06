using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SGAmod.Tiles.Bars
{
	public class VirulentOre: ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileShine[Type] = 800;
			Main.tileShine2[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 200;
			TileID.Sets.Ore[Type] = true;
			HitSound = SoundID.Tink;
			DustType = 128;
			//ItemDrop = ModContent.ItemType<Items.Materials.Bars.VirulentOre>();
			MinPick = 150;
			MineResist = 5f;
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Virulent");
			AddMapEntry(Color.Lime, name);
		}

		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}