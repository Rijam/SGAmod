using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Consumables.BossSummons
{
	public class BaseBossSummon : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod)
		{
			return GetType() != typeof(BaseBossSummon);
		}

		public override bool CanUseItem(Player player)
		{
			//if (SGAmod.anysubworld)
			//{
			//	if (player == Main.LocalPlayer)
			//		Main.NewText("This cannot be used outside the normal folds of reality...", 75, 75, 80);

			//	return false;
			//}
			return base.CanUseItem(player);
		}
	}
}