//From Qwerty's random content mod, used with permission, thank you qwerty!
//https://github.com/qwerty3-14/QwertysRandomContent/blob/master/ItemUseGlow.cs github source
//Has been modifed by IDGCaptainRussia94


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace SGAmod.Core
{
    public class ItemUseGlow : GlobalItem
    {
        public Texture2D glowTexture = null;
        public int glowOffsetY = 0;
        public int glowOffsetX = 0;
        public float angleAdd = 0f;
        public override bool InstancePerEntity => true;
        protected override bool CloneNewInstances =>true;

        public Func<Item, Player, Color> GlowColor = delegate (Item item, Player player)
        {
            return Color.White;
        };
        public Action<Item, PlayerDrawSet, Vector2, float, Color> CustomDraw = delegate (Item item, PlayerDrawSet drawSet, Vector2 position, float angle, Color glowColor)
        {

        };
    }
    public class PlayerUseGlow : PlayerDrawLayer
    {
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			Item item = drawPlayer.HeldItem;
			Texture2D useGlow = null;
			if (item != null && !item.IsAir)
			{
				useGlow = item.GetGlobalItem<ItemUseGlow>().glowTexture;
			}
			if (useGlow != null)
			{
				return true;
			}
			
			else
			{
				return false;
			}
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			//Mod mod = ModLoader.GetMod("QwertysRandomContent");
			if (!drawPlayer.HeldItem.IsAir)
			{
				Item item = drawPlayer.HeldItem;
				Texture2D texture = TextureAssets.Item[drawPlayer.HeldItem.type].Value;
				Color glowColor = item.GetGlobalItem<ItemUseGlow>().GlowColor(item, drawPlayer);
				Action<Item, PlayerDrawSet, Vector2, float, Color> customDraw = item.GetGlobalItem<ItemUseGlow>().CustomDraw;
				Vector2 zero2 = Vector2.Zero;

				if (texture != null && drawPlayer.itemAnimation > 0)
				{
					Vector2 value2 = drawInfo.ItemLocation;
					if(item.useStyle == ItemUseStyleID.Shoot)
					{
						bool isstaff = Item.staff[item.type];
						if (isstaff)
						{
							float rotate = drawPlayer.itemRotation + 0.785f * (float)drawPlayer.direction;
							int revwidth = 0; //Used to reverse the item sprite
							int what = 0; //I genually have no idea what this variable is used for
							Vector2 zero3 = new Vector2(0f, (float)TextureAssets.Item[item.type].Height());
							if (drawPlayer.gravDir == -1f)
							{
								if (drawPlayer.direction == -1)
								{
									rotate += 1.57f;
									zero3 = new Vector2((float)TextureAssets.Item[item.type].Width(), 0f);
									revwidth -= TextureAssets.Item[item.type].Width();
								}
								else
								{
									rotate -= 1.57f;
									zero3 = Vector2.Zero;
								}
							}
							else if(drawPlayer.direction == -1)
							{
								zero3 = new Vector2((float)TextureAssets.Item[item.type].Width(), (float)TextureAssets.Item[item.type].Height());
								revwidth -= TextureAssets.Item[item.type].Width();
							}

							Vector2 drawpos = new Vector2((float)((int)(value2.X - Main.screenPosition.X + zero3.X + (float)revwidth)), (float)((int)(value2.Y - Main.screenPosition.Y + (float)what)));
							float drawAngle = rotate + item.GetGlobalItem<ItemUseGlow>().angleAdd * drawPlayer.direction;
							DrawData value = new DrawData(texture, drawpos, new Rectangle?(new Rectangle(0, 0, TextureAssets.Item[item.type].Width(), TextureAssets.Item[item.type].Height())), glowColor, drawAngle, zero3, item.scale, SpriteEffects.None, 0);
							customDraw(item, drawInfo, drawpos, drawAngle, glowColor);
							drawInfo.DrawDataCache.Add(value);
						}
						else
						{
							Vector2 center = new Vector2((float)(TextureAssets.Item[item.type].Width() / 2), (float)(TextureAssets.Item[item.type].Height() / 2));
							Vector2 offset = new Vector2(10, texture.Height / 2);
							if (item.GetGlobalItem<ItemUseGlow>().glowOffsetX != 0)
							{
								offset.X = item.GetGlobalItem<ItemUseGlow>().glowOffsetX;
							}
							offset.Y += item.GetGlobalItem<ItemUseGlow>().glowOffsetY * drawPlayer.gravDir;
							int hoffset = (int)offset.X;
							center.Y = offset.Y;
							Vector2 origin = new Vector2((float)(-(float)hoffset), (float)(TextureAssets.Item[item.type].Height() / 2));
							if (drawPlayer.direction == -1)
							{
								origin = new Vector2((float)(TextureAssets.Item[item.type].Width() + hoffset), (float)(TextureAssets.Item[item.type].Height() / 2));
							}

							Vector2 drawpos = new Vector2((float)((int)(value2.X - Main.screenPosition.X + center.X)), (float)((int)(value2.Y - Main.screenPosition.Y + center.Y)));
							float drawAngle = drawPlayer.itemRotation + item.GetGlobalItem<ItemUseGlow>().angleAdd * drawPlayer.direction;
							DrawData value = new DrawData(texture, drawpos, new Rectangle?(new Rectangle(0, 0, TextureAssets.Item[item.type].Width(), TextureAssets.Item[item.type].Height())), glowColor, drawAngle, origin, item.scale, SpriteEffects.None, 0);
							customDraw(item, drawInfo, drawpos, drawAngle, glowColor);
							drawInfo.DrawDataCache.Add(value);
						}
					}
					else
					{
						float drawAngle = drawPlayer.itemRotation + item.GetGlobalItem<ItemUseGlow>().angleAdd; //* drawPlayer.direction;
						Vector2 drawPos = new Vector2((float)((int)(value2.X + (item.GetGlobalItem<ItemUseGlow>().glowOffsetX * drawPlayer.direction) - Main.screenPosition.X)),
							(float)((int)(value2.Y + (item.GetGlobalItem<ItemUseGlow>().glowOffsetY * drawPlayer.gravDir) - Main.screenPosition.Y)));
						DrawData value = new DrawData(texture, drawPos, new Rectangle?(new Rectangle(0, 0, texture.Width, texture.Height)), glowColor, drawAngle, new Vector2(texture.Width * 0.5f - texture.Width * 0.5f * (float)drawPlayer.direction, drawPlayer.gravDir == -1 ? 0f : texture.Height), item.scale, drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
						customDraw(item, drawInfo, drawPos, drawAngle, glowColor);
						drawInfo.DrawDataCache.Add(value);
					}
				}
			}
		}

	}
}
