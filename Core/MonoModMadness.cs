using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Idglibrary;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Reflection;
using Terraria.GameInput;
using MonoMod.RuntimeDetour.HookGen;
using static MonoMod.Cil.ILContext;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
//using SGAmod.NPCs.Hellion;
using System.Threading.Tasks;
using System.Threading;
using Terraria.GameContent;

namespace SGAmod
{
    public class HookEdit
    {
        public bool loaded = false;

        public void Load()
        {
            if(!loaded)
            {
                LoadInternal();
                loaded = true;
            }
        }
        //Due to how monomod works, I am unsure if all Unload things will be necessary.
        public void Unload()
        {
            if (loaded)
            {
                UnloadInternal();
                loaded = false;
            }
        }
        protected virtual void LoadInternal() { }
        protected virtual void UnloadInternal() { }
        internal HookEdit() { Load(); }
    }
    public class ModifyUI : HookEdit
    {
        Type typeUIModItem;
        /*
        protected override void LoadInternal()
        {
            typeUIModItem = Assembly.GetAssembly(typeof(Main)).GetType("Terraria.ModLoader.UI.UIModItem");//This class is off-limits to us (internal), even to ON and IL, so we have to grab it directly from Main's assembly
            MonoModHooks.Add(typeUIModItem.GetMethod("DrawMenu", SGAmod.UniversalBindingFlags), ModifyUIModILPatch);
            
        }
        protected override void UnloadInternal()
        {

        }*/

        public event Manipulator ModifyUIModManipulator
        {
            add {  }

            remove { }
        }
        //Someone that is better at IL/ON/detours should probably do this.
        private void ModifyUIModILPatch(ILContext context)
        {
            ILCursor c = new ILCursor(context);
            MethodInfo HackTheMethod = typeof(UIElement).GetMethod("DrawMenu", SGAmod.UniversalBindingFlags);
            if (c.TryGotoNext(MoveType.After, i => i.MatchCall(HackTheMethod)))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);

                c.EmitDelegate<UIModDelegate>(UIDrawMethod);
                return;
            }
            SGAmod.Instance.Logger.Error("Hookpoint patch failed");
        }

        private delegate void UIModDelegate(object instance, SpriteBatch sb);

        private void UIDrawMethod(object instance, SpriteBatch sb)
        {
            string ModName = (string)typeUIModItem.GetProperty("ModName", SGAmod.UniversalBindingFlags).GetValue(instance);
            CalculatedStyle style = (CalculatedStyle)typeUIModItem.GetMethod("GetInnerDimensions", SGAmod.UniversalBindingFlags).Invoke(instance, Array.Empty<object>());

            if(ModName == "SGAmod"&& !ModLoader.TryGetMod("ConciseModList", out Mod ConciseModList)) // This mod changes how mods are displayed, and as such, break this utility.
            {
                Texture2D credits = TextureAssets.SettingsPanel2.Value;
                Vector2 buttonOffset = new Vector2(style.Width - 112, style.Height - 38);
                Rectangle boxSize = new Rectangle(credits.Width / 2, 0, credits.Width/2, credits.Height/2);
                Vector2 pos = style.Position() + buttonOffset;
                Rectangle inBox = new Rectangle((int)pos.X, (int)pos.Y, boxSize.Width,boxSize.Height);

                sb.Draw(credits, pos, boxSize, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

                if (inBox.Contains((int)Main.MouseScreen.X,(int)Main.MouseScreen.Y))
                {
                    instance.GetType().GetField("_tooltip", SGAmod.UniversalBindingFlags).SetValue(instance, "SGAmod Credits");
                    Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
                    if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        Credits.CreditsManager.queuedCredits = true;
                    }
                }
            }
        }
    }
    public static class PrivateClassEdits
    {
        //This class is comprised of more direct version of Monomod IL patches/ON Detour Hooks to classes that you normally 'should not' have access to (and by extention, should not be) patching, learned thanks to a very "specific", very talented dev who's serving a not-worth-it mod
        internal static List<HookEdit> hooksList;

        internal static void ApplyPatches()
        {
            SGAmod.Instance.Logger.Debug("Doing some Monomod Hook nonsense... Jesus christ this is alot of vanilla hacking");
            hooksList = new List<HookEdit>();

            Assembly assembly = SGAmod.Instance.Code;
            foreach (Type typeoff in assembly.GetTypes())
            {
                Type hooktype = typeof(HookEdit);
                if (typeoff != hooktype && typeoff.IsSubclassOf(hooktype))
                {
                    HookEdit instancedHook = (assembly.CreateInstance(typeoff.FullName) as HookEdit);
                    hooksList.Add(instancedHook);
                }
            }
        }
        internal static void RemovePatches()
        {
            if (hooksList != null)
            {
                foreach (HookEdit hook in hooksList)
                {
                    hook.Unload();
                }
            }
            
        }
        //There used to be a lot of anti-QoL hooks here
    }
}
