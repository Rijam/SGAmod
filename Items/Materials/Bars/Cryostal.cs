using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using System;

namespace SGAmod.Items.Materials.Bars
{
	public class CryostalBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryostal Bar");
			Tooltip.SetDefault("Condensed ice magic has formed into this bar");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = 1000;
			Item.rare = ItemRarityID.Pink;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 3;
		}
	}
}