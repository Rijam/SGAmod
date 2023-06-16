using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.DamageClasses
{
	public class ApplyDamageClasses : GlobalItem
	{
		public static List<int> VanillaThrowingItems = new()
		{ 
			ItemID.AleThrowingGlove, ItemID.Snowball, ItemID.Shuriken, ItemID.RottenEgg,
			ItemID.ThrowingKnife, ItemID.PoisonedKnife, ItemID.Beenade, ItemID.BoneDagger,
			ItemID.StarAnise, ItemID.SpikyBall, ItemID.Javelin, ItemID.FrostDaggerfish,
			ItemID.Bone, ItemID.MolotovCocktail, ItemID.BoneJavelin, ItemID.PartyGirlGrenade,
			ItemID.Grenade, ItemID.StickyGrenade, ItemID.BouncyGrenade // Bone Glove not included
		};

		public override bool AppliesToEntity(Item entity, bool lateInstantiation)
		{
			return VanillaThrowingItems.Contains(entity.type);
		}

		public override void SetDefaults(Item entity)
		{
			// Disabled
			//entity.DamageType = ModContent.GetInstance<RangedThrowing>();
		}
	}
}