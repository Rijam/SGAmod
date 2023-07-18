using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armor.Nomadic
{

	[AutoloadEquip(EquipType.Head)]
	public class NomadicHood : ModItem, IFormerThrowingItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nomadic Hood");
			// Tooltip.SetDefault("5% increased throwing crit chance");
		}

		public static void ActivateSandySwiftness(SGAPlayer sgaply)
        {
			if (sgaply.ActionCooldownStack_AddCooldownStack(60 * 45))
			{
				sgaply.Player.AddBuff(ModContent.BuffType<SandySwiftnessBuff>(), 60 * 16);
				sgaply.sandStormTimer = 30;
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast with { Pitch = -0.5f }, sgaply.Player.Center);
			}
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 5000;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 1;
		}
		public override void UpdateEquip(Player player)
		{
			//player.Throwing().thrownCrit += 5;
			player.GetCritChance(DamageClass.Ranged) += 5f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HardenedSand, 20);
			recipe.AddIngredient(ItemID.Silk, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class NomadicShirt : NomadicHood, IFormerThrowingItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nomadic Shirt");
			// Tooltip.SetDefault("5% increased throwing damage");
			ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
			ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = false;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 4000;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			//player.Throwing().thrownDamage += 0.05f;
			player.GetDamage(DamageClass.Ranged) += 0.05f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AntlionMandible, 1);
			recipe.AddIngredient(ItemID.HardenedSand, 10);
			recipe.AddIngredient(ItemID.Cactus, 25);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class NomadicPants : NomadicHood, IFormerThrowingItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nomadic Pants");
			// Tooltip.SetDefault("20% increase to movement speed on sand");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 5000;
			Item.rare = ItemRarityID.Blue;
		}
		public override void UpdateEquip(Player player)
		{
			if (player.velocity.Y == 0)
			{
				for (int i = 0; i < 6; i += 1)
				{
					Point therePointpre = new((int)player.MountedCenter.X, (int)player.position.Y);
					Point therePoint = new(therePointpre.X >> 4, (therePointpre.Y >> 4) + i);
					Tile tile = Framing.GetTileSafely(therePoint);
					if (WorldGen.InWorld(therePoint.X, therePoint.Y) && tile.HasTile && (TileID.Sets.Conversion.Sand[tile.TileType] || TileID.Sets.Conversion.HardenedSand[tile.TileType]))
					{
						if (Math.Abs(player.velocity.X) > 1)
						{
							//Main.NewText("tests");
							player.moveSpeed += 1.20f;
							player.accRunSpeed += 1.20f;

							for (int ii = 0; ii < 3; ii += 1)
							{
								Vector2 pos = new(player.position.X, player.position.Y + player.Hitbox.Size().Y-4);
								int dust = Dust.NewDust(pos, (int)player.Hitbox.Size().X, 4, DustID.Sandstorm, (player.velocity.X) + Math.Sign(player.direction) * -6f, Main.rand.NextFloat(-4f,-1f), 0, Color.Yellow, 1f);
								int slot = 2;
								Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(player.dye[slot].dye, player);
								Main.dust[dust].noGravity = false;
							}
						}

						break;
					}
				}
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AntlionMandible, 2);
			recipe.AddIngredient(ItemID.HardenedSand, 20);
			recipe.AddIngredient(ItemID.SandBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}

	public class ManifestedSandTosser : ModItem, IManifestedItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sand Tosser");
			// Tooltip.SetDefault("Throws sand from your inventory");
		}
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SandBallFalling;

        public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			Item.width = 12;
			Item.height = 24;
			Item.rare = ItemRarityID.Blue;
			Item.value = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			//Item.Throwing().thrown = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 10;
			Item.shootSpeed = 4f;
			Item.shoot = ProjectileID.SandBallGun;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 12;
			Item.height = 24;
			Item.knockBack = 1;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Sand;
		}
		public static void DrawManifestedItem(Item item,SpriteBatch spriteBatch, Vector2 position, Rectangle frame,float scale)
        {
			Texture2D inner = TextureAssets.Item[item.type].Value;

			Vector2 slotSize = new(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new(inner.Width / 2, inner.Height / 2);

			for (float i = 0; i < 1f; i += 0.20f)
			{
				spriteBatch.Draw(inner, drawPos, null, Color.White * (1f - ((i + (Main.GlobalTimeWrappedHourly / 2f)) % 1f)) * 0.75f, i * MathHelper.TwoPi, textureOrigin, Main.inventoryScale * (0.5f + 3.00f * (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f)), SpriteEffects.None, 0f);
			}

		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			DrawManifestedItem(Item,spriteBatch, position, frame, scale);

			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int probg = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			//Main.projectile[probg].Throwing().thrown = true;
			Main.projectile[probg].DamageType = DamageClass.Ranged;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(10));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAProjectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAProjectile>();
			modeproj.myPlayer = player;
			Main.projectile[probg].netUpdate = true;
			IdgProjectile.Sync(probg);
			return false;
		}
	}

	public class SandySwiftnessBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sandy Swiftness");
			// Description.SetDefault("The desert winds aid your beconing call!");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{

			if (!Terraria.GameContent.Events.Sandstorm.Happening)
			{
				typeof(Terraria.GameContent.Events.Sandstorm).GetMethod("StartSandstorm", SGAmod.UniversalBindingFlags).Invoke(null, new object[0]);
				//Terraria.GameContent.Events.Sandstorm.Happening = true;
				Terraria.GameContent.Events.Sandstorm.TimeLeft = player.buffTime[buffIndex] + 30;
				//typeof(Terraria.GameContent.Events.Sandstorm).GetMethod("ChangeSeverityIntentions", SGAmod.UniversalBindingFlags).Invoke(null,new object[0]);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.WorldData, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				}
			}

			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.sandStormTimer = (int)MathHelper.Max(sgaply.sandStormTimer, 10);

			if (sgaply.sandStormTimer > 10)
			{
				int timerSand = sgaply.sandStormTimer - 10;
				for (int i = 1; i < 8+ timerSand; i++)
				{
					Vector2 offset = Main.rand.NextVector2CircularEdge(timerSand, timerSand) * 2f;
					int dust = Dust.NewDust(player.Hitbox.TopLeft() + Main.rand.NextVector2CircularEdge(timerSand, timerSand)*2f, player.Hitbox.Width, player.Hitbox.Height, timerSand > 0 ? 269 : 268);
					Main.dust[dust].scale = (i+ (20-timerSand)/6f) / 5f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].alpha = 75;
					Main.dust[dust].velocity = (Vector2.Normalize(offset) /4f)+((player.velocity * Main.rand.NextFloat(0.0f, (8f - i) * 0.25f))+ (offset/4f) + (new Vector2(Main.windPhysicsStrength * 100f, 0)*(timerSand>0 ? 0f : 1f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.2f, 0.2f)) * Main.rand.NextFloat(1f, 3f));
				}
			}

			for (int i = 1; i < 8; i++)
			{
				int dust = Dust.NewDust(player.Hitbox.TopLeft() + new Vector2(0, 0), player.Hitbox.Width, player.Hitbox.Height, DustID.Sandstorm);
				Main.dust[dust].scale = i/4f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].alpha = 150;
				Main.dust[dust].velocity = (player.velocity * Main.rand.NextFloat(0.0f, (8f-i)*0.25f))+new Vector2(Main.windPhysicsStrength * 100f, 0) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.2f, 0.2f)) * Main.rand.NextFloat(1f, 3f);
			}
		}
	}
}