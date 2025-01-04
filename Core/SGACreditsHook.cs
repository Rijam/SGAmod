using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using MonoMod.RuntimeDetour;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;


namespace SGAmod
{
	public partial class SGAmod : Mod
	{
		public static Type UIModItemType;
		public static FieldInfo _modName;
		/*public override void Load()
		{
			base.Load();
			UIModItemType = typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.UI.UIModItem");
			_modName = UIModItemType.GetField("_modName" ,SGAmod.UniversalBindingFlags);

		}*/
		private void LoadModList()
		{
			UIModItemType = typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.UI.UIModItem");
			_modName = UIModItemType.GetField("_modName", SGAmod.UniversalBindingFlags);
		}
		public override void PostSetupContent()
		{
			base.PostSetupContent();
			On_UIElement.Draw += CreditsDraw;
		}

		private static void CreditsDraw(On_UIElement.orig_Draw orig, UIElement s, SpriteBatch sb)
		{
			orig(s, sb);
			//ConciseModList and CompactMods change how the mod list works and looks and thus break this.
			if (s.GetType().Equals(UIModItemType) && (string)UIModItemType.GetProperty("ModName", UniversalBindingFlags).GetValue(s) == "SGAmod" && (!ModLoader.TryGetMod("ConciseModList", out Mod ConciseModList) || !ModLoader.TryGetMod("CompactMods", out Mod CompactMods)))
			{
				Texture2D credits = ModContent.Request<Texture2D>("Terraria/Images/UI/Settings_Inputs_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				CalculatedStyle style = (CalculatedStyle)UIModItemType.GetMethod("GetInnerDimensions", UniversalBindingFlags).Invoke(s, Array.Empty<object>());
				Vector2 buttonOffset = new Vector2(style.Width - 112, style.Height - 38);
				Rectangle boxSize = new Rectangle(credits.Width / 2, 0, credits.Width / 2, credits.Height / 2);
				Vector2 pos = style.Position() + buttonOffset;
				Rectangle inBox = new Rectangle((int)pos.X, (int)pos.Y, boxSize.Width, boxSize.Height);

				sb.Draw(credits, pos, boxSize, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

				if (inBox.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
				{
					s.GetType().GetField("_tooltip", UniversalBindingFlags).SetValue(s, "SGAmod Credits");
					Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
					if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
					{
						Credits.CreditsManager.queuedCredits = true;

					}
				}

				//The dragon is walking, please do not disturb them theyre very shy.
				Texture2D Draken = ModContent.Request<Texture2D>("SGAmod/NPCs/TownNPCs/Dergon_oldish", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				int frame = (int)(Main.GlobalTimeWrappedHourly * 8f) % 14 + 2;

				Vector2 offset = new Vector2(180, style.Height - 10);


				Vector2 frameSize = new Vector2(Draken.Width, Draken.Height);

				Rectangle rect = new Rectangle(0, (int)(frame * (frameSize.Y / 25)), (int)frameSize.X, (int)(frameSize.Y / 25));

				SpriteEffects backAndForth = Main.GlobalTimeWrappedHourly % 38 > 19 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

				offset += new Vector2((Main.GlobalTimeWrappedHourly % 38) + (Main.GlobalTimeWrappedHourly % 38 < 19 ? 0 : ((Main.GlobalTimeWrappedHourly - 19) % 19) * -2), 0) * 12;

				sb.Draw(Draken, style.Position() + offset, rect, Color.White, 0, new Vector2(frameSize.X, frameSize.Y / 25f) / 2f, 0.50f, backAndForth, 0f);
			}
		}
	
	}
}
