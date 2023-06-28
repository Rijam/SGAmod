using Microsoft.Xna.Framework;
using SGAmod.Items.Materials.Bars;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.Furniture.DankWood
{
	#region Dank Wood Platform
	public class DankWoodPlatform : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("It still smells funny...");
		}

		public override void SetDefaults()
		{
			Item.width = 8;
			Item.height = 10;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodPlatformTile>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(2);
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Workbench
	public class DankWoodWorkbench : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 14;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodWorkbench>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 10);
			recipe.Register();
		}
	}
    #endregion
    #region Dank Wood Chair
    public class DankWoodChair : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 30;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodChair>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Toilet
	public class DankWoodToilet : ModItem
	{
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.DankWood.DankWoodToilet>());
			Item.width = 16;
			Item.height = 26;
			Item.value = 150;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<DankWoodItem>(), 6)
				.AddTile(TileID.Sawmill)
				.Register();
		}
	}
	#endregion
	#region Dank Wood Table
	public class DankWoodTable : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodTable>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Dresser
	public class DankWoodDresser : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded dresser.");
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodDresser>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 16);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Bed
	public class DankWoodBed : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 2000;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodBed>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 15);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Sofa
	public class DankWoodSofa : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 24;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodSofa>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 5);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Lantern
	internal class DankWoodLantern : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
			Item.autoReuse = true;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodLantern>();
            Item.width = 10;
            Item.height = 24;
            Item.value = 150;
        }
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 6);
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
    #endregion
    #region Dank Wood Lamp
    internal class DankWoodLamp : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodLamp>();
			Item.width = 10;
			Item.height = 24;
			Item.value = 500;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Bookcase
	public class DankWoodBookcase : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded bookcase.");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 32;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodBookcase>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 20);
			recipe.AddIngredient(ItemID.Book, 10);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Torch
	public class DankWoodTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 100;

			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ShimmerTorch;
			ItemID.Sets.SingleUseInGamepad[Type] = true;
			ItemID.Sets.Torches[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.DefaultToTorch(ModContent.TileType<Tiles.Furniture.DankWood.DankWoodTorch>(), 0, false);
			Item.value = 50;
		}

		public override void HoldItem(Player player)
		{
			// This torch cannot be used in water, so it shouldn't spawn particles or light either
			if (player.wet)
			{
				return;
			}
			if (Main.rand.NextBool(player.itemAnimation > 0 ? 7 : 30))
			{
				Dust dust = Dust.NewDustDirect(new Vector2(player.itemLocation.X + (player.direction == -1 ? -16f : 6f), player.itemLocation.Y - 14f * player.gravDir), 4, 4, DustID.Torch, 0f, 0f, 100);
				if (!Main.rand.NextBool(3))
				{
					dust.noGravity = true;
				}

				dust.velocity *= 0.3f;
				dust.velocity.Y -= 1.5f;
				dust.position = player.RotatedRelativePoint(dust.position);
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 1f, 0.75f, 0.30f);
		}

		public override void PostUpdate()
		{
			if (!Item.wet)
			{
				Lighting.AddLight(Item.Center, 1f, 0.75f, 0.30f);
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(3)
				.AddIngredient(ModContent.ItemType<DankWoodItem>(), 1)
				.AddIngredient(ItemID.Gel, 1)
				.Register();
			CreateRecipe(3)
				.AddIngredient(ModContent.ItemType<Materials.Environment.DankCore>(), 1)
				.AddIngredient(ItemID.Torch, 3)
				.Register();
		}
	}
	#endregion
	#region Dank Wood Clock
	public class DankWoodClock : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodClock>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 10);
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 3);
			recipe.AddIngredient(ItemID.Glass, 6);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Piano
	public class DankWoodPiano : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodPiano>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Bone, 4);
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 15);
			recipe.AddIngredient(ItemID.Book, 1);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Sink
	public class DankWoodSink : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodSink>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 6);
			recipe.AddIngredient(ItemID.WaterBucket, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
	#endregion
	#region Dank Wood Bathtub
	public class DankWoodBathtub : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.maxStack = Item.CommonMaxStack;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.DankWoodBathtub>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DankWoodItem>(), 14);
			recipe.AddTile(TileID.Sawmill);
			recipe.Register();
		}
	}
	#endregion
	#region Novite Candle
	internal class NoviteCandle : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.holdStyle = 1;
			Item.noWet = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.NoviteCandle>();
			Item.flame = true;
			Item.value = 0; //candles have no value for some reason.
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<NoviteBar>(), 4)
				.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
		public override void HoldItem(Player player)
		{
			// This torch cannot be used in water, so it shouldn't spawn particles or light either
			if (player.wet)
			{
				return;
			}
			if (Main.rand.NextBool(player.itemAnimation > 0 ? 7 : 30))
			{
				Dust dust = Dust.NewDustDirect(new Vector2(player.itemLocation.X + (player.direction == -1 ? -16f : 6f), player.itemLocation.Y - 14f * player.gravDir), 4, 4, DustID.Torch, 0f, 0f, 100);
				if (!Main.rand.NextBool(3))
				{
					dust.noGravity = true;
				}

				dust.velocity *= 0.3f;
				dust.velocity.Y -= 1.5f;
				dust.position = player.RotatedRelativePoint(dust.position);
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 1f, 0.75f, 0.30f);
		}

		public override void PostUpdate()
		{
			if (!Item.wet)
			{
				Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 1f, 0.75f, 0.30f);
			}
		}
	}
	#endregion
	#region Novite Chandelier
	internal class NoviteChandelier : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.NoviteChandelier>();
			Item.width = 30;
			Item.height = 28;
			Item.value = 3000;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 4);
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 4);
			recipe.AddIngredient(ItemID.Chain);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
	#endregion
	#region Novite Candelabra
	internal class NoviteCandelabra : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Furniture.DankWood.NoviteCandelabra>();
			Item.width = 22;
			Item.height = 28;
			Item.value = 1500;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 3);
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
	#endregion
}