using SGAmod.Items.Materials.Bars;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.Items.Armor.Novus
{

	[AutoloadEquip(EquipType.Head)]
	public class NovusHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novus Hood");
			// Tooltip.SetDefault("5% faster item use times");
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
		}
        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}
        public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
            sgaplayer.UseTimeMul += 0.05f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NovusBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class NovusBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novus Breastplate");
			// Tooltip.SetDefault("5% increased crit chance");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 5;
			player.GetCritChance(DamageClass.Ranged) += 5;
			player.GetCritChance(DamageClass.Magic) += 5;
			//player.Throwing().thrownCrit += 5;
			player.GetCritChance(DamageClass.Ranged) += 5;
		}		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NovusBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class NovusLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novus Leggings");
			// Tooltip.SetDefault("5% increased movement speed\n10% increased acceleration and max running speed");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed *= 1.05f;
			player.accRunSpeed *= 1.1f;
			player.maxRunSpeed *= 1.1f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NovusBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}