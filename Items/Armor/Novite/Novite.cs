using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SGAmod.Items.Materials.Bars;

namespace SGAmod.Items.Armor.Novite
{

	[AutoloadEquip(EquipType.Head)]
	public class NoviteHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novite Helmet");
			// Tooltip.SetDefault("5% increased Technological damage\n+1500 Max Electric Charge\n20% reduced Electric Consumption");
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			// TODO
			//player.GetModPlayer<SGAPlayer>().techdamage += 0.05f;
			//player.GetModPlayer<SGAPlayer>().electricChargeCost *= 0.80f;
			//player.GetModPlayer<SGAPlayer>().electricChargeMax += 1500;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class NoviteChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novite Chestplate");
			// Tooltip.SetDefault("10% increased Technological and Trap damage\n+1 passive Electric Charge Rate\n+2500 Max Electric Charge");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			//player.GetModPlayer<SGAPlayer>().techdamage += 0.10f;
			//player.GetModPlayer<SGAPlayer>().TrapDamageMul += 0.10f;
			//player.GetModPlayer<SGAPlayer>().electricChargeMax += 2500;
			//player.GetModPlayer<SGAPlayer>().electricrechargerate += 1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class NoviteLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Novite Leggings");
			// Tooltip.SetDefault("5% increased Technological damage\nCharge is built up by running around at high speeds (600/Second)\n+1000 Max Electric Charge");
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
			//player.GetModPlayer<SGAPlayer>().techdamage += 0.05f;
			//player.GetModPlayer<SGAPlayer>().Noviteset = Math.Max(player.GetModPlayer<SGAPlayer>().Noviteset, 1);
			//player.GetModPlayer<SGAPlayer>().electricChargeMax += 1000;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}