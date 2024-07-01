using Idglibrary;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SGAmod.Items
{
	public class FlavorTooltips : GlobalItem
	{
		/*public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			return entity.ModItem is IFormerHavocItem or IFormerThrowingItem or IDedicatedPhilBill or null;
		}*/

		/// <summary>
		/// Finds the index of a certain tooltip line.
		/// </summary>
		/// <param name="tooltips">The list of tooltips.</param>
		/// <param name="name">The name of the tooltip to find. See the docs for all of the names.</param>
		/// <param name="mod">Which mod the tooltip line is from. "Terraria" for vanilla.</param>
		/// <param name="index">Out: the index of the tooltip line. 0 if not found.</param>
		/// <returns>True if found.</returns>
		public static bool FindTooltipIndex(List<TooltipLine> tooltips, string name, string mod, out int index)
		{
			TooltipLine tooltipLine = tooltips.FirstOrDefault(x => x.Name == name && x.Mod == mod);
			if (tooltipLine != null)
			{
				index = tooltips.IndexOf(tooltipLine);
				return true;
			}
			index = 0;
			SGAmod.Instance.Logger.WarnFormat("Tooltip line {0} from mod {1} not found!", name, mod);
			return false;
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.ModItem is IFormerHavocItem)
			{
				Color c = Main.hslToRgb(0.9f, 0.5f, 0.35f);
				tooltips.Add(new TooltipLine(Mod, "HavocItem", Idglib.ColorText(c, Language.GetTextValue("Mods.SGAmod.Common.Tooltip.FormerHavocItem"))));
			}
			if (item.ModItem is IFormerThrowingItem)
			{
				Color c = new(110, 110, 110);
				tooltips.Add(new TooltipLine(Mod, "ThrowingItem", Idglib.ColorText(c, Language.GetTextValue("Mods.SGAmod.Common.Tooltip.FormerThrowingItem"))));
			}
			if (item.ModItem is IDedicatedPhilBill)
			{
				Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5f) % 1f, 0.45f, 0.65f);
				tooltips.Add(new TooltipLine(Mod, "Dedicated", Idglib.ColorText(c, Language.GetTextValue("Mods.SGAmod.Common.Tooltip.DedicatedPhilBill"))));
			}
<<<<<<< Updated upstream
            
				
                if (SGAWorld.downedWraiths < 1)
                {
                    Color c = Main.hslToRgb(0.5f, 0.1f, 0.7f);
                    
					Recipe recipe = Main.recipe[item.type];
                    if (recipe.HasTile(TileID.Furnaces) && (recipe.HasResult(ItemID.CopperBar) || recipe.HasResult(ItemID.TinBar) || recipe.HasResult(ItemID.IronBar) || recipe.HasResult(ItemID.LeadBar) || recipe.HasResult(ItemID.SilverBar) || recipe.HasResult(ItemID.TungstenBar) || recipe.HasResult(ItemID.GoldBar) || recipe.HasResult(ItemID.PlatinumBar)))
						tooltips.Add(new TooltipLine(Mod, "WraithClue", Idglib.ColorText(c, Language.GetTextValue("Mods.SGAmod.Common.Tooltip.BarWarning"))));
					
                }
        }
=======
<<<<<<< Updated upstream
=======
			
			if (!SGAWorld.downedCopperWraith)
			{
				for (int i = 0; i < Recipe.numRecipes; i++)
				{
					Recipe recipe = Main.recipe[i];

					if ((recipe.HasTile(TileID.Furnaces) || recipe.requiredTile.Any(tile => tile == TileID.Furnaces)) && !SGAWorld.downedCopperWraith)
					{
						if (recipe.HasResult(item.type))
						{
							Color c = Main.hslToRgb(0.5f, 0.1f, 0.7f);
							tooltips.Add(new TooltipLine(Mod, "WraithClue", Idglib.ColorText(c, Language.GetTextValue("Mods.SGAmod.Common.Tooltip.BarWarning"))));
						}
					}
				}
			}
			if (!SGAWorld.downedCobaltWraith)
			{
				for(int i = 0;i < Recipe.numRecipes; i++)
				{
					Recipe recipe = Main.recipe[i];
					if ((recipe.HasResult(ItemID.AdamantiteForge) || recipe.HasResult(ItemID.TitaniumForge)) && recipe.HasResult(item.type))
					{
						Color c = Main.hslToRgb(0.3f, 0.1f, 0.7f);
						tooltips.Add(new TooltipLine(Mod, "WraithClue", Idglib.ColorText(c, Language.GetTextValue("Mods.SGAmod.Common.Tooltip.ForgeLock"))));
					}
				}
			}
>>>>>>> Stashed changes
		}
>>>>>>> Stashed changes
	}
}