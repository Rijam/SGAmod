using Terraria.ModLoader;

namespace SGAmod.Backgrounds
{
	public class DankShrineBackgroundStyle : ModUndergroundBackgroundStyle
	{
		public override void FillTextureArray(int[] textureSlots)
		{
			// The textures have to be in that path for some unknown reason. Also the namespace is wacky.
			textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/DankShrineBiome0");
			textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/DankShrineBiome1");
			textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/DankShrineBiome2");
			textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/DankShrineBiome3");
		}
	}
}