using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Consumables.GrabBags
{
	public class TreasureBagSpiderQueen : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Treasure Bag");
			// Tooltip.SetDefault("Right click to open");

			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 32;
			Item.height = 32;
			Item.expert = true;
			Item.rare = ItemRarityID.Expert;
		}

		public override bool CanRightClick()
		{
			return true;
		}
		/*
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(Mod.Find<ModItem>("VialofAcid").Type, Main.rand.Next(35, 60));
			player.QuickSpawnItem(Mod.Find<ModItem>("AlkalescentHeart").Type, 1);
			if (Main.rand.Next(0, 3) == 0)
				player.QuickSpawnItem(Mod.Find<ModItem>("CorrodedShield").Type, 1);
			if (Main.rand.Next(0, 3) == 0)
				player.QuickSpawnItem(Mod.Find<ModItem>("AmberGlowSkull").Type, 1);
			if (Main.rand.Next(7) == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<SpiderQueenMask>());
			}
		}
		*/
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<Items.Armor.Vanity.BossMasks.SpiderQueenMask>(), 7));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Materials.BossDrops.VialOfAcid>(), 1, 35, 60));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Accessories.Expert.AlkalescentHeart>()));
			// itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Weapons.Shields.CorrodedShield>(), 3)); #TODO
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Accessories.Defense.CorrodedSkull>(), 3));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<NPCs.Bosses.SpiderQueen.SpiderQueen>()));
		}
	}
}