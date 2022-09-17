using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod
{
	public class ItemSets : GlobalItem
	{
		public static List<int> isWhip = new()
		{ 
			ItemID.BlandWhip, ItemID.ThornWhip, ItemID.BoneWhip, ItemID.FireWhip, ItemID.CoolWhip, ItemID.SwordWhip, ItemID.MaceWhip, ItemID.ScytheWhip,
			ItemID.RainbowWhip 
		};
		public static List<int> isJoustingLance = new()
		{
			ItemID.JoustingLance, ItemID.HallowJoustingLance, ItemID.ShadowJoustingLance
		};
		public static List<int> isRevolver = new() { };
		public static List<int> havocItem = new() { };

		public override void Unload()
		{
			isWhip.Clear();
			isJoustingLance.Clear();
			isRevolver.Clear();
			havocItem.Clear();
		}
	}
}