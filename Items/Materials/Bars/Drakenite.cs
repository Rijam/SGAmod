using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace SGAmod.Items.Materials.Bars
{
	public class DrakeniteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drakenite Bar");
			Tooltip.SetDefault("A Bar forged from the same powers that created Draken...");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.consumable = false;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.Bars.BarTiles>();
			Item.placeStyle = 9;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.Mod == "Terraria" && line.Name == "ItemName")
				{
					line.OverrideColor = Color.Lerp(Color.DarkGreen, Color.White, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 8f));
				}
			}
		}
		public static Texture2D[] staticeffects = new Texture2D[32];
		public static void CreateTextures()
		{
			if (!Main.dedServ)
			{
				Texture2D atex = ModContent.Request<Texture2D>("SGAmod/Items/Materials/Bars/DrakeniteBarHalf").Value;
				int width = atex.Width; int height = atex.Height;
				for (int index = 0; index < staticeffects.Length; index++)
				{
					Texture2D tex = new(Main.graphics.GraphicsDevice, width, height);

					var datacolors2 = new Color[atex.Width * atex.Height];
					atex.GetData(datacolors2);
					tex.SetData(datacolors2);

					DrakeniteBar.staticeffects[index] = new Texture2D(Main.graphics.GraphicsDevice, width, height);
					Color[] dataColors = new Color[atex.Width * atex.Height];

					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x += 1)
						{
							if (Main.rand.NextBool(0, 16))
							{
								int therex = (int)MathHelper.Clamp((x), 0, width);
								int therey = (int)MathHelper.Clamp((y), 0, height);
								if (datacolors2[(int)therex + therey * width].A > 0)
								{

									dataColors[(int)therex + therey * width] = Main.hslToRgb(Main.rand.NextFloat(0f, 1f) % 1f, 0.6f, 0.8f) * (0.5f);
								}
							}
							if (Main.rand.Next(0, 8) > Math.Abs(x - (index - 8)))
							{
								int therex = (int)MathHelper.Clamp((x), 0, width);
								int therey = (int)MathHelper.Clamp((y), 0, height);
								if (datacolors2[(int)therex + therey * width].A > 0)
								{
									dataColors[(int)therex + therey * width] = Main.hslToRgb(((float)(index - 8) / (float)width) % 1f, 0.9f, 0.75f) * (0.80f * (1f - (Math.Abs((float)x - ((float)index - 8f)) / 8f)));
								}
							}
						}
					}
					DrakeniteBar.staticeffects[index].SetData(dataColors);
				}
			}

		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
                /*
				Texture2D texture = DrakeniteBar.staticeffects[(int)(Main.GlobalTimeWrappedHourly * 20f) % DrakeniteBar.staticeffects.Length];
				Vector2 slotSize = new(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale * 2f, SpriteEffects.None, 0f);
                */
			}
		}

		public override void AddRecipes()
		{
			/*
			Recipe.Create(1)
				.AddIngredient(ItemID.LunarBar, 1)
				.AddIngredient(ModContent.ItemType<ByteSoul>(), 10)
				.AddIngredient(ModContent.ItemType<WatchersOfNull>(), 1)
				.AddIngredient(ModContent.ItemType<AncientFabricItem>(), 25)
				.AddIngredient(ModContent.ItemType<HopeHeart>(), 1)
				.AddTile(ModContent.TileType<Tiles.CraftingStations.ReverseEngineeringStation>())
				.Register();
			*/
		}

	}
}