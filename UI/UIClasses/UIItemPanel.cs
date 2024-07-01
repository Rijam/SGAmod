using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace SGAmod.UI.UIClasses
{
    internal class UIItemPanel : UIPanel
    {
        internal const float panelwidth = 50f;
        internal const float panelheight = 50f;
        internal const float panelpadding = 0f;

        public Item item;

        public string HintText
        {
            get;
            set;
        }
        public Texture2D HintTexture
        {
            get; 
            set;
        }
        public UIItemPanel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null, float width = 50f, float height = 50f)
        {
            Width.Set(width, 0f);
            Height.Set(height, 0f);
            SetPadding(0f);
            item = new Item();
            item.netDefaults(netID);
            item.stack = stack;
            HintTexture = hintTexture;
            HintText = hintText;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawSelf(spriteBatch);
            CalculatedStyle innerDimensions = GetInnerDimensions();
            Texture2D texture2D;
            Color color;
            if(HintTexture != null && item.IsAir)
            {
                texture2D = HintTexture;
                color = Color.LightGray * 0.5f;
                if(IsMouseHovering) Main.hoverItemName = HintText ?? string.Empty;
            }
            else
            {
                if(item.IsAir)
                {
                    return;
                }
                texture2D = TextureAssets.Item[item.type].Value;
                color = item.GetAlpha(Color.White);
                if (IsMouseHovering)
                {
                    Main.hoverItemName = item.Name;
                    Main.HoverItem = item.Clone();
                    Item itemtype = Main.HoverItem;
                    string itemname = Main.HoverItem.Name;
                    ModItem moditem = Main.HoverItem.ModItem;
                    string ModType;
                    if (moditem == null) ModType = null;
                    else
                    {
                        string ModName = moditem.Mod.Name;
                        ModType = ModName.Insert(((moditem != null) ? new int?(moditem.Mod.Name.Length) : null).Value, "]").Insert(0, "[");
                        itemtype.SetNameOverride(itemname + ModType);
                    }
                }
            }
            Rectangle rectangle = (!item.IsAir && Main.itemAnimations[item.type] != null) ? Main.itemAnimations[item.type].GetFrame(texture2D) : texture2D.Frame(1, 1, 0, 0);
            float num = 1f;
            if((float)rectangle.Width > innerDimensions.Width || (float)rectangle.Height > innerDimensions.Height) 
            {
                if(rectangle.Width > rectangle.Height)
                {
                    num = innerDimensions.Width / (float)rectangle.Width;
                }
                else
                {
                    num = innerDimensions.Width / rectangle.Height;
                }
            }
            float num2 = num;
            Color white = Color.White;
            ItemSlot.GetItemLight(ref white, ref num, item.type, false) ;
            Vector2 position = new Vector2(innerDimensions.X, innerDimensions.Y);
            position.X += innerDimensions.Width * 1f / 2f - (float)rectangle.Width * num / 2f;
            position.Y += innerDimensions.Height * 1f / 2f - (float)rectangle.Height * num / 2f;
            if (item.ModItem == null || item.ModItem.PreDrawInInventory(spriteBatch, position, rectangle, color, color, Vector2.Zero, num))
            {
                spriteBatch.Draw(texture2D, position, new Rectangle?(rectangle), color, 0f, Vector2.Zero, num, SpriteEffects.None, 0f);
                if (item == null || item.color != default(Color))
                {
                    spriteBatch.Draw(texture2D, position, new Rectangle?(rectangle), color, 0f, Vector2.Zero, num, SpriteEffects.None, 0f);
                }
            }
            ModItem modItem = item.ModItem;
            if (modItem != null)
            {
                modItem.PostDrawInInventory(spriteBatch, position, rectangle, color,color, Vector2.Zero, num);
            }
            if(item != null && item.stack > 1)
            {
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, Math.Min(9999, item.stack).ToString(), innerDimensions.Position().X + 10f, innerDimensions.Position().Y + 26f, Color.White, Color.Black, Vector2.Zero, num2 * 0.8f);
            }
        }
    }
}
