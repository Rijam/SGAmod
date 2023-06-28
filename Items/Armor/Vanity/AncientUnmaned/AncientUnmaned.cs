using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;


namespace SGAmod.Items.Armor.Vanity.AncientUnmaned
{

	[AutoloadEquip(EquipType.Head)]
	public class AncientUnmanedHood : ModItem, IDedicatedPhilBill
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Unmaned Hood");
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightRed;
			Item.vanity = true;
			Item.defense = 0;
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AncientUnmanedBreastplate : ModItem, IDedicatedPhilBill
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Unmaned Breastplate");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightRed;
			Item.vanity = true;
			Item.defense = 0;
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AncientUnmanedLeggings : ModItem, IDedicatedPhilBill
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Unmaned Leggings");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightRed;
			Item.vanity = true;
			Item.defense = 0;
		}
	}
}