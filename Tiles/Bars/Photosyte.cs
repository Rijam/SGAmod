using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Tiles.Bars
{
	public class Photosyte : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileShine2[Type] = false;
			Main.tileSpelunker[Type] = false;
			Main.tileOreFinderPriority[Type] = 20;
			TileID.Sets.Ore[Type] = true;
			HitSound = SoundID.NPCHit9;
			DustType = 128;
			ItemDrop = ModContent.ItemType<Items.Materials.Bars.Photosyte>();
			MinPick = 10;
			MineResist = 1f;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Photosyte");
			AddMapEntry(new Color(40, 150, 40), name);
		}
	}
}