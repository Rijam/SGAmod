using Microsoft.Xna.Framework;
using SGAmod.Items.Materials.Bars;
using SGAmod.Items.Materials.Environment;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armor.Blazewyrm
{

	[AutoloadEquip(EquipType.Head)]
	public class BlazewyrmHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blazewyrm Helm");
			// Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n20% faster melee speed and 16% more melee damage");
			ArmorUseGlowHead.RegisterData(Item.headSlot, Texture + "_Head_Glow", Color.White);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			player.GetAttackSpeed(DamageClass.Melee) += 0.20f;
			player.GetDamage(DamageClass.Melee) += 0.16f;
			sgaplayer.apocalypticalChance[0] += 1.0;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MoltenHelmet, 1);
			recipe.AddIngredient(ModContent.ItemType<NovusBar>(), 6);
			recipe.AddIngredient(ModContent.ItemType<FieryShard>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//tooltips.Add(new TooltipLine(Mod, "accapocalypticaltext", SGAGlobalItem.apocalypticaltext));
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class BlazewyrmBreastplate : BlazewyrmHelm
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blazewyrm Breastplate");
			// Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n12% increased melee crit chance");
			ArmorUseGlowBody.RegisterData(Item.bodySlot, Color.White);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 14;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 12;
			player.GetModPlayer<SGAPlayer>().apocalypticalChance[0] += 1.0;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MoltenBreastplate, 1);
			recipe.AddIngredient(ModContent.ItemType<NovusBar>(), 8);
			recipe.AddIngredient(ModContent.ItemType<FieryShard>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class BlazewyrmLeggings : BlazewyrmHelm
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blazewyrm Leggings");
			// Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n25% increase to movement speed\nEven faster in lava");
			ArmorUseGlowLegs.RegisterData(Item.legSlot, Texture + "_Legs_Glow", Color.White);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 1.25f;
			player.accRunSpeed += 1.5f;
			if (player.lavaWet)
			{
				player.moveSpeed *= 1.2f;
				player.accRunSpeed *= 1.2f;
			}
			player.GetModPlayer<SGAPlayer>().apocalypticalChance[0] += 1.0;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MoltenGreaves, 1);
			recipe.AddIngredient(ModContent.ItemType<NovusBar>(), 6);
			recipe.AddIngredient(ModContent.ItemType<FieryShard>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}