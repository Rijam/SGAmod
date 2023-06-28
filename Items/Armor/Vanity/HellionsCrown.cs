using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armor.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class HellionsCrown : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hellion's Crown");
			// Tooltip.SetDefault("'Become one with the pure chaotic code...'");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 0;
			Item.rare = ItemRarityID.Green;
			Item.vanity = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string addtotip = tooltips[0].Text;
			string thetip = "";
			for (int loc = 0; loc < addtotip.Length; loc += 1)
			{
				char character = addtotip[loc];
				if (Main.rand.Next(30) <= 1)
				{
					character = (char)(33 + Main.rand.Next(15));
				}

				thetip += character;
			}
			tooltips[0].Text = thetip;

		}
		public override void EquipFrameEffects(Player player, EquipType type)
		{
			Item.rare = Main.rand.Next(0, 12);
		}
	}

	public class HellionsCrownSystem : ModSystem
	{
		public override void PostUpdateEverything()
		{
			if (!Main.dedServ)
			{
				Item c0decrown = new();
				c0decrown.SetDefaults(ModContent.ItemType<HellionsCrown>());
				//Main.armorHeadLoaded[c0decrown.headSlot] = true;
				//TextureAssets.ArmorHead[c0decrown.headSlot].Value;
				Asset<Texture2D> headTexture = null;
				while (headTexture == null)
				{
					//headTexture = Main.armorHeadTexture[Main.rand.Next(1, Main.armorHeadTexture.Length)];
					// #TODO This only randomly selects modded head pieces because those are the ones that are always loaded.
					int rand = Main.rand.Next(1, TextureAssets.ArmorHead.Length);
					if (TextureAssets.ArmorHead[rand].IsLoaded)
					{
						headTexture = TextureAssets.ArmorHead[rand];
					}
				}
				//Main.armorHeadTexture[c0decrown.headSlot] = headTexture;
				TextureAssets.ArmorHead[c0decrown.headSlot] = headTexture;
			}
		}
	}
}