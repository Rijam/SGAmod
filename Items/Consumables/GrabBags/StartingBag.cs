using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Consumables.GrabBags
{
    public class StartingBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("IDG's Starting Bag");
            // Tooltip.SetDefault("Some starting items couldn't fit in your inventory??\n{$CommonItemTooltip.RightClickToOpen}");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.consumable = true;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ItemID.LifeCrystal, 1));
            // itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AnomalyStudyPaper>(), 1)); TODO

            ItemDropRule.Common(ItemID.MiningPotion, 1, 0, 12);
            ItemDropRule.Common(ItemID.BuilderPotion, 1, 0, 12);
            ItemDropRule.Common(ItemID.NightOwlPotion, 1, 0, 12);
            ItemDropRule.Common(ItemID.ShinePotion, 1, 0, 12);

            IItemDropRule[] jumpTypes = new IItemDropRule[] {
                ItemDropRule.Common(ItemID.CloudinaBottle, 1),
                ItemDropRule.Common(ItemID.TsunamiInABottle, 1),
                ItemDropRule.Common(ItemID.FartinaJar, 1),
                ItemDropRule.Common(ItemID.BlizzardinaBottle, 1)
            };

            itemLoot.Add(new OneFromRulesRule(1, jumpTypes));

            IItemDropRule[] bootTypes = new IItemDropRule[] {
                ItemDropRule.Common(ItemID.HermesBoots, 1),
                ItemDropRule.Common(ItemID.SailfishBoots, 1),
                ItemDropRule.Common(ItemID.FlurryBoots, 1)
            };

            itemLoot.Add(new OneFromRulesRule(1, bootTypes));

            IItemDropRule[] pickaxeTypes = new IItemDropRule[] {
                ItemDropRule.Common(ItemID.SilverPickaxe, 1),
                ItemDropRule.Common(ItemID.TungstenPickaxe, 1)
            };

            itemLoot.Add(new OneFromRulesRule(1, pickaxeTypes));

            IItemDropRule[] extraTypes = new IItemDropRule[] {
                ItemDropRule.Common(ItemID.GrapplingHook, 1),
                ItemDropRule.Common(ItemID.MiningHelmet, 1)
                // ItemDropRule.Common(ModContent.ItemType<ThrowersPouch>(), 1) TODO
            };

            itemLoot.Add(new OneFromRulesRule(1, extraTypes));
        }
    }
}