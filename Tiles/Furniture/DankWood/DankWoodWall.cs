using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SGAmod.Items.Placeable.Furniture.DankWood;

namespace SGAmod.Tiles.Furniture.DankWood
{
	public class DankWoodWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeable.Furniture.DankWood.DankWoodWall>();
			AddMapEntry(new Color(41, 31, 23));
		}
	}
	public class DankWoodFence : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeable.Furniture.DankWood.DankWoodFence>();
			AddMapEntry(new Color(49, 41, 26));
		}
	}
	public class BrokenDankWoodFence : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<Items.Placeable.Furniture.DankWood.BrokenDankWoodFence>();
			AddMapEntry(new Color(31, 33, 13));
		}
	}
}