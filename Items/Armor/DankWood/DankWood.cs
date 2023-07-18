using SGAmod.Items.Materials.Environment;
using SGAmod.Items.Placeable.Furniture.DankWood;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.Items.Armor.DankWood
{
    [AutoloadEquip(EquipType.Head)]

    public class DankWoodHelmet : ModItem, IFormerHavocItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dank Wood Helmet");
            // Tooltip.SetDefault("4% increased critical strike chance\n15% DoT resistance");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 4f;
            player.GetModPlayer<SGAPlayer>().DoTResist *= 0.85f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 50);
            recipe.AddIngredient(ModContent.ItemType<DankCore>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    [AutoloadEquip(EquipType.Body)]
    public class DankWoodChestplate : ModItem, IFormerHavocItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dank Wood Chestplate");
            // Tooltip.SetDefault("8% increased item use rate, improved life regen\n25% DoT resistance");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.lifeRegen = 1;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
            sgaplayer.UseTimeMul += 0.08f;
            sgaplayer.DoTResist *= 0.75f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 30);
            recipe.AddIngredient(ModContent.ItemType<DankCore>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class DankWoodLeggings : ModItem, IFormerHavocItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dank Wood Leggings");
            // Tooltip.SetDefault("20% Improved movement speed and faster acceleration\n10% DoT resistance");
        }

        public override void SetDefaults()
		{
			Item.width = 18;
            Item.height = 12;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
            player.moveSpeed += 0.2f;
            player.accRunSpeed += 0.05f;
            player.GetModPlayer<SGAPlayer>().DoTResist *= 0.90f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 25);
            recipe.AddIngredient(ModContent.ItemType<DankCore>(), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}