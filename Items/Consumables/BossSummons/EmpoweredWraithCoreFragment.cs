using SGAmod.Items.Materials.BossDrops;
using SGAmod.NPCs.Bosses.CobaltWraith;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Consumables.BossSummons
{
    public class EmpoweredWraithCoreFragment : WraithCoreFragment
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Pink;
        }
        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<CobaltWraith>());
            SoundEngine.PlaySound(SoundID.Roar);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("SGAmod:Tier1HardmodeOre", 10)
                .AddIngredient<WraithCoreFragment>()
                .AddIngredient<BronzeWraithShard>()
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
