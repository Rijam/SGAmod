using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using System;

namespace SGAmod.Items.Materials.Bars
{
	public class Glowrock : ModItem //, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowrock");
			Tooltip.SetDefault("These rocks seem to give the Asteriods a glow; Curious.\nExtract it via an Extractinator for some goodies!\nDoesn't have much other use, outside of illegal interests");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
			ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
		}

		/*
		public static void DoFallenSpaceRocks()
		{
			if (Main.dayTime || !Main.hardMode)
				return;

			if (Main.rand.Next(400000) < Main.maxTilesX / (Main.netMode == NetmodeID.SinglePlayer ? 6 : 3))
			{
				int farside = (int)((Main.maxTilesX * 16) * 0.2f);
				int closeside = (int)((Main.maxTilesX * 16) * 0.8f);

				int xTile = (int)(Main.rand.NextBool() ? Main.rand.Next(farside) : closeside + Main.rand.Next(farside));


				Projectile.NewProjectile(new Vector2(xTile, 50), Vector2.UnitY.RotatedBy((Main.rand.NextFloat(-1f, 1f) * MathHelper.Pi) * 0.10f) * Main.rand.NextFloat(3f, 6f), ModContent.ProjectileType<Dimensions.FallingSpaceRock>(), 1000, 10);
			}
		}

		public static void CrashComet(Vector2 pos, Vector2 velocity)
		{
			Microsoft.Xna.Framework.Audio.SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.DD2_BetsysWrathImpact, pos);

			if (snd != null)
			{
				snd.Pitch = Main.rand.NextFloat(0.25f, 0.60f);
			}

			float velocityAngle = velocity.ToRotation().AngleLerp(MathHelper.PiOver2, 0.75f);

			for (int i = 0; i < 60; i += 1)
			{
				Vector2 offset = (velocityAngle + (MathHelper.Pi + Main.rand.NextFloat(-0.35f, 0.35f))).ToRotationVector2();
				int dust = Dust.NewDust(new Vector2(pos.X, pos.Y), 0, 0, DustID.BlueCrystalShard);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].alpha = 150;
				Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(1f * Main.rand.NextFloat(0f, 3f) * (i / 5f));
				Main.dust[dust].noGravity = true;
			}

			for (int num1181 = 0; num1181 < 20; num1181++)
			{
				float num1182 = (float)num1181 / 15f;
				Vector2 vector123 = new Vector2((num1182 * 0.80f) + 0.15f, 0f).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f));
				Gore gore = Gore.NewGoreDirect(pos, Vector2.Zero, Utils.SelectRandom<int>(Main.rand, 375, 376, 377), 0.75f);

				if (gore != null)
				{
					gore.velocity = vector123.RotatedBy(velocityAngle + MathHelper.Pi) * 12f;
					//gore.velocity.Y -= 2f;
					//gore.timeLeft = 90;
				}
			}


			for (int num1181 = 0; num1181 < 16; num1181++)
			{
				float num1182 = (float)num1181 / 20f;
				Vector2 vector123 = new Vector2(Main.rand.NextFloat() * 10f, 0f).RotatedBy(num1182 * -(float)Math.PI + Main.rand.NextFloat() * 0.1f - 0.05f);
				int itemz = Item.NewItem(pos + vector123 * 3, ModContent.ItemType<Glowrock>(), Main.rand.Next(1, 3));
				if (itemz >= 0)
				{
					Main.item[itemz].velocity = (Vector2.Normalize(vector123) * 0.50f) + (vector123 * 0.75f);
				}
			}
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (Main.dayTime && Main.rand.Next(50) < 1 && Item.velocity.LengthSquared() < 2 && Dimensions.SGAPocketDim.WhereAmI == null)
			{
				Item.active = false;
				Tiles.TechTiles.HopperTile.CleanUpGlitchedItems();

				var snd = SoundEngine.PlaySound(SoundID.NPCDeath7, Item.Center);

				if (snd != null)
				{
					snd.Pitch = Main.rand.NextFloat(-0.75f, -0.25f);
				}

				for (float num475 = 0.5f; num475 < 6f + (Item.stack / 8f); num475 += 0.25f)
				{
					float anglehalf = Main.rand.NextFloat(MathHelper.TwoPi);
					Vector2 startloc = Item.Center;
					int dust = Dust.NewDust(startloc, 0, 0, DustID.BlueCrystalShard);

					float anglehalf2 = anglehalf + ((float)Math.PI / 2f);
					Main.dust[dust].position += anglehalf2.ToRotationVector2() * (float)((Main.rand.Next(-200, 200) / 10f));

					Main.dust[dust].scale = 2f - Math.Abs(num475) / 4f;
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Main.dust[dust].velocity = (randomcircle / 3f) * num475;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
				}


				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, Item.whoAmI);
				}
			}
		}

		public override void ExtractinatorUse(ref int resultType, ref int resultStack)
		{
			if (Main.rand.Next(8) < 4)
				return;

			WeightedRandom<(int, int)> WR = new WeightedRandom<(int, int)>();

			if (NPC.downedPlantBoss)
			{
				WR.Add((ItemID.Ectoplasm, 1), 1);
			}

			if (NPC.downedMoonlord)
				WR.Add((ItemID.LunarOre, Main.rand.Next(1, 3)), 1);

			WR.Add((ItemID.SoulofLight, 1), 1);
			WR.Add((ItemID.SoulofNight, 1), 1);
			WR.Add((ItemID.DarkBlueSolution, Main.rand.Next(1, 9)), 0.50);
			WR.Add((ItemID.BlueSolution, Main.rand.Next(1, 9)), 0.50);

			WR.needsRefresh = true;
			(int, int) thing = WR.Get();
			resultType = thing.Item1;
			resultStack = thing.Item2;
		}
		*/

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Blue.ToVector3() * 0.55f);
		}

		public int RadioactiveHeld()
		{
			return 2;
		}

		public int RadioactiveInventory()
		{
			return 1;
		}
	}
	public class CelestineChunk : ModItem //, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestine Chunk");
			Tooltip.SetDefault("Inert and radioactive Luminite...");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
		}

		public override string Texture => "Terraria/Images/Item_" + ItemID.LunarOre;

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(Color.DarkGray, Color.Gray, 0.50f + (float)Math.Sin(Main.GlobalTimeWrappedHourly / 2f) / 2f);
		}

		public override void AddRecipes()
		{
			/*
			Recipe.Create(ItemID.LunarBar, 2)
				.AddIngredient(ItemID.LunarOre, 1)
				.AddIngredient(this, 4)
				.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 1)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
			*/
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.55f);
		}

		public int RadioactiveHeld()
		{
			return 2;
		}

		public int RadioactiveInventory()
		{
			return 1;
		}
	}
	public class OverseenCrystal : ModItem //, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overseen Crystal");
			Tooltip.SetDefault("Celestial Shards manifested from Phaethon's creators; resonates with charged forgotten spirits\nMay be used to fuse several strong materials together with ease\nSurely a shady dealer will also be interested in trading for these...");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;
		}

		public int RadioactiveHeld()
		{
			return 3;
		}

		public int RadioactiveInventory()
		{
			return 2;
		}

		public override void AddRecipes()
		{
			/* NOTE: Move to respective items/Global Items
			int tileType = ModContent.TileType<Tiles.CraftingStations.ReverseEngineeringStation>();

			Recipe.Create(ModContent.GetInstance<PrismalOre>().Type, 8)
				.AddIngredient(ModContent.ItemType<NovusOre>(), 2)
				.AddIngredient(ModContent.ItemType<NoviteOre>(), 2)
				.AddIngredient(this, 4)
				.AddTile(tileType)
				.Register();

			Recipe.Create(ModContent.GetInstance<VibraniumPlating>().Type, 2)
				.AddIngredient(ModContent.ItemType<AncientFabricItem>(), 5)
				.AddIngredient(ModContent.ItemType<AdvancedPlating>(), 2)
				.AddIngredient(this, 2)
				.AddTile(tileType)
				.Register();

			Recipe.Create(ModContent.GetInstance<OmniSoul>().Type, 2)
				.AddIngredient(ItemID.SoulofLight, 1)
				.AddIngredient(ItemID.SoulofNight, 1)
				.AddIngredient(this, 2)
				.AddTile(tileType)
				.Register();

			Recipe.Create(ItemID.DefenderMedal, 1)
				.AddIngredient(ItemID.FossilOre, 2)
				.AddIngredient(this, 1)
				.AddTile(tileType)
				.Register();

			Recipe.Create(ModContent.GetInstance<Consumables.DivineShower>().Type, 1)
				.AddIngredient(ItemID.HallowedBar, 4)
				.AddIngredient(ItemID.SoulofFright, 1)
				.AddIngredient(ItemID.SoulofMight, 1)
				.AddIngredient(ItemID.SoulofSight, 1)
				.AddIngredient(this, 5)
				.AddTile(tileType)
				.Register();
			*/
		}
	}
}