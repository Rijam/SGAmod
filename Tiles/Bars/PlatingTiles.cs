using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Tiles.Bars
{
	public class AdvancedPlatingTile : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			HitSound = SoundID.Tink;
			DustType = DustID.Iron;
			ItemDrop = ModContent.ItemType<Items.Materials.Bars.AdvancedPlating>();
			AddMapEntry(new Color(181, 165, 107));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
	public class VibraniumPlatingTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			HitSound = SoundID.Tink;
			DustType = DustID.Iron;
			ItemDrop = ModContent.ItemType<Items.Materials.Bars.VibraniumPlating>();
			AddMapEntry(new Color(88, 44, 86));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}