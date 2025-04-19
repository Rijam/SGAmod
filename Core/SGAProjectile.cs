using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod
{
	public class SGAProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		public bool raindown = false;
		public bool grazed = false;
		public double extraApocalypticalChance = 0;

		public Player myPlayer = null;

		public override void SetDefaults(Projectile projectile)
		{
			if (Main.gameMenu)
				return;
		}
	}
}