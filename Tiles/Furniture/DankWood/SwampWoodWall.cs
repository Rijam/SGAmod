using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Tiles.Furniture.DankWood
{

	public class SwampWoodWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			AddMapEntry(new Color(100, 40, 40));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}