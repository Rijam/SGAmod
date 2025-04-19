using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using SGAmod.NPCs.Bosses.CopperWraith;
using SGAmod.NPCs.Bosses.CobaltWraith;
using Terraria.Audio;

namespace SGAmod.Items.Consumables.BossSummons
{
    public class WraithCoreFragment : BaseBossSummon
    {
        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.netMode == NetmodeID.Server)
                SGAmod.Instance.Logger.Warn("DEBUG SERVER: item canuse");
            if (Main.netMode == NetmodeID.MultiplayerClient)
                SGAmod.Instance.Logger.Warn("DEBUG CLIENT: item canuse");
            if (!NPC.AnyNPCs(ModContent.NPCType<CopperWraith>()) && !NPC.AnyNPCs(ModContent.NPCType<CobaltWraith>())) // && !NPC.AnyNPCs(ModContent.NPCType<LuminiteWraith>())
            {
                return base.CanUseItem(player);
            }
            else
            {
                return false;
            }
        }
        public override bool? UseItem(Player player)
        {
            if (Main.netMode == NetmodeID.Server)
                SGAmod.Instance.Logger.Warn("DEBUG SERVER: item used");
            if (Main.netMode == NetmodeID.MultiplayerClient)
                SGAmod.Instance.Logger.Warn("DEBUG CLIENT: item used");

            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<CopperWraith>());
            SoundEngine.PlaySound(SoundID.Roar);
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("SGAmod:Tier1Ore", 15)
                .AddIngredient(ItemID.FallenStar, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
