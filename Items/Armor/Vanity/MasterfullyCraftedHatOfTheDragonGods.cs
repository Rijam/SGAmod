using Idglibrary;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace SGAmod.Items.Armor.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class MasterfullyCraftedHatOfTheDragonGods : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Masterfully Crafted Hat Of The Dragon Gods");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.LightPurple;
			Item.vanity = true;
			Item.defense = 0;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5f) % 1f, 0.45f, 0.65f);
			tooltips.Add(new TooltipLine(Mod, "Dedicated", Idglib.ColorText(c, "Dedicated to a stupid Heroforge meme")));
		}
	}
}