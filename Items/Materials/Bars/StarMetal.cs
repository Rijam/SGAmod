using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace SGAmod.Items.Materials.Bars
{
	public class StarMetalMold : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Metal Mold");
			Tooltip.SetDefault("A mold used to make Wraith Cores, it seems fit to mold bars from heaven\nIs not consumed in crafting Star Metal Bars");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 20;
			Item.height = 20;
			Item.value = 0;
			Item.rare = ItemRarityID.Yellow;
			Item.consumable = false;
		}
	}
	public class StarMetalBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Metal Bar");
			Tooltip.SetDefault("'This bar is a glimming white sliver that shimmers with stars baring the color of pillars'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 7;
		}
		public override void AddRecipes()
		{
			/*
			Recipe recipe = new StarMetalRecipes(Mod, this.Type, 4);
			recipe.AddIngredient(ModContent.ItemType<StarMetalMold>(), 1);
			recipe.AddIngredient(ItemID.LunarOre, 1);
			recipe.AddRecipeGroup("Fragment", 4);
			recipe.Register();
			*/
		}
	}
}