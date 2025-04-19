using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Materials.BossDrops
{
	public class VialOfAcid : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vial of Acid");
			// Tooltip.SetDefault("Highly Corrosive");
		}

		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 16;
			Item.height = 16;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
		}
	}


    public class CopperWraithNotch : ModItem
    {
		
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 14;
            Item.height = 14;
            Item.value = 20;
            Item.rare = ItemRarityID.White;

			
        }
		
	}

	public class CopperWraithShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<TinWraithShard>();
		}
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 14;
			Item.height= 14;
			Item.value = 5;
			Item.rare= ItemRarityID.White;
		}
		
	}
	public class TinWraithShard : CopperWraithShard
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<CopperWraithShard>();
		}
	}
	public class BronzeWraithShard : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.width = 14;
			Item.height = 14;
			Item.value = 25;
			Item.rare = ItemRarityID.Orange;
		}
        public override void AddRecipes()
        {
			CreateRecipe(2)
				.AddIngredient(ModContent.ItemType<CopperWraithShard>(), 2)
				.AddIngredient(ItemID.TinOre, 4)
				.AddTile(TileID.Hellforge)
				.Register();
			CreateRecipe(2)
				.AddIngredient<TinWraithShard>(2)
				.AddIngredient(ItemID.CopperOre, 4)
				.AddTile(TileID.Hellforge)
				.Register();

        }
    }

	public class CobaltWraithNotch : ModItem
	{
		
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			
		}
		
	}

	public class CobaltWraithShard : ModItem
	{
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 30;
			Item.rare = ItemRarityID.Green;
		}
	}
}