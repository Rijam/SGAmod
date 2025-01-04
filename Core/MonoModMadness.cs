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
        
        protected override void LoadInternal()
        {
            
        }
        protected override void UnloadInternal()
        {

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
