using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Tiles.Environment
{
	public class SwampWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(100, 200, 100));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		// TODO
		/*public override bool CanExplode(int i, int j)
		{
			return SGAWorld.downedMurk > 1;
		}*/
	}
}