
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Materials.BossDrops;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armor.Fames
{

	[AutoloadEquip(EquipType.Head)]
	public class FamesHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fames Helm");
			// Tooltip.SetDefault("6% increased Throwing crit chance and Throwing Velocity");
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
			ArmorUseGlowHead.RegisterData(Item.headSlot, Texture + "_Head_Glowmask", Color.White);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0,0,75,0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public static void ActivateHungerOfFames(SGAPlayer sgaply)
        {
			if (sgaply.ActionCooldownStack_AddCooldownStack(60 * 60))
            {
				sgaply.Player.AddBuff(ModContent.BuffType<FamesHungerBuff>(),300);
				SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot with { Pitch = 0.5f }, sgaply.Player.Center);
				SoundEngine.PlaySound(SoundID.Zombie35 with { Pitch = -0.5f }, sgaply.Player.Center);

				for(int i = 0; i < 50; i += 1)
                {
					int dust = Dust.NewDust(sgaply.Player.Hitbox.TopLeft() + new Vector2(0, -8), sgaply.Player.Hitbox.Width, sgaply.Player.Hitbox.Height+8, ModContent.DustType<Dusts.AcidDust>());
					Main.dust[dust].scale = 2f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = (sgaply.Player.velocity * Main.rand.NextFloat(0.75f, 1f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-1.2f, 1.2f))*Main.rand.NextFloat(1f,3f);
				}
			}
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			//player.Throwing().thrownCrit += 6;
			//player.Throwing().thrownVelocity += 0.06f;
			player.GetCritChance(DamageClass.Ranged) += 6f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<VialOfAcid>(), 16);
			recipe.AddRecipeGroup("SGAmod:NoviteNovusBars", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class FamesChestplate : FamesHelmet
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fames Chestplate");
			// Tooltip.SetDefault("6% increased Throwing Damage and 10% increased Throwing Velocity");
			ArmorUseGlowBody.RegisterData(Item.bodySlot, Color.White);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 8;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			//player.Throwing().thrownDamage += 0.06f;
			//player.Throwing().thrownVelocity += 0.10f;
			player.GetDamage(DamageClass.Ranged) += 0.06f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<VialOfAcid>(), 24);
			recipe.AddRecipeGroup("SGAmod:NoviteNovusBars", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class FamesLeggings : FamesHelmet
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fames Leggings");
			// Tooltip.SetDefault("6% increased Throwing Damage\n15% increased movement speed");
			ArmorUseGlowLegs.RegisterData(Item.legSlot, Texture + "_Legs_Glow", Color.White);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			//player.Throwing().thrownDamage += 0.06f;
			player.GetDamage(DamageClass.Ranged) += 0.06f;
			player.moveSpeed += 1.15f;
			player.accRunSpeed += 1.5f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<VialOfAcid>(), 12);
			recipe.AddRecipeGroup("SGAmod:NoviteNovusBars", 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	public class FamesHungerBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hunger of Fames");
			// Description.SetDefault("Acidically consume everything, even yourself");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			//sgaply.acidSet = (sgaply.acidSet.Item1, true); // TODO
			player.lifeRegenTime = 0;
			player.lifeRegenCount = 0;

			int dust = Dust.NewDust(player.Hitbox.BottomLeft() + new Vector2(0, -12), player.Hitbox.Width, 12, ModContent.DustType<Dusts.AcidDust>());
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = (player.velocity * Main.rand.NextFloat(0.9f, 1f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(1f, 3f);
		}
	}
}