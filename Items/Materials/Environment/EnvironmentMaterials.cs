using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Materials.Environment
{
	public class DankCore : ModItem
	{
		public override void SetDefaults()
		{
			Item.value = 2500;
			Item.rare = ItemRarityID.Green;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dank Core");
			// Tooltip.SetDefault("'Dark, Dank, Dangerous...'");
		}
	}
	public class FieryShard : ModItem, IFormerHavocItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fiery Shard");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 25;
			Item.rare = ItemRarityID.Orange;
			Item.alpha = 30;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Orange.ToVector3() * 0.55f * Main.essScale);
		}
	}

	public class FrigidShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Frigid Shard");
			// Tooltip.SetDefault("Raw essence of ice");
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 14;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Aqua.ToVector3() * 0.25f);
		}
	}

	public class Fridgeflame : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fridgeflame");
			// Tooltip.SetDefault("Alloy of hot and cold essences");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = 200;
			Item.rare = ItemRarityID.LightPurple;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.65f * Main.essScale);
		}
		public override void AddRecipes()
		{
			CreateRecipe(2)
				.AddIngredient(ModContent.ItemType<FieryShard>(), 1)
				.AddIngredient(ModContent.ItemType<FrigidShard>(), 1)
				.AddCondition(Condition.NearWater)
				.AddCondition(Condition.NearLava)
				.AddTile(TileID.CrystalBall)
				.Register();
		}
	}
	public class IceFairyDust : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Fairy Dust");
			// Tooltip.SetDefault("It doesn't feel like it's from this universe");
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 14;
			Item.value = 75;
			Item.rare = ItemRarityID.Pink;
		}
	}
	public class IlluminantEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Illuminant Essence");
			// Tooltip.SetDefault("'Shards of Heaven'");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.HotPink.ToVector3() * 0.55f * Main.essScale);
		}
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Purple;
		}
	}

	
}