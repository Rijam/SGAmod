using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;


namespace SGAmod.Items.Armor.Vanity.AncientSpaceDiver
{

	[AutoloadEquip(EquipType.Head)]
	public class AncientSpaceDiverHelm : ModItem, IDedicatedPhilBill
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Space Diver Helm");
			ArmorUseGlowHead.RegisterData(Item.headSlot, Texture + "_Head_Glowmask", Color.White);
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightPurple;
			Item.vanity = true;
			Item.defense = 0;
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AncientSpaceDiverChestplate : ModItem, IDedicatedPhilBill
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Space Diver Chestplate");
			ArmorUseGlowBody.RegisterData(Item.bodySlot, Color.White);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightPurple;
			Item.vanity = true;
			Item.defense = 0;
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AncientSpaceDiverLeggings : ModItem, IDedicatedPhilBill
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ancient Space Diver Leggings");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.LightPurple;
			Item.vanity = true;
			Item.defense = 0;
		}
	}
}