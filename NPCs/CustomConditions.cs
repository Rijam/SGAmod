using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SGAmod.NPCs
{
    public class NotWarned : IItemDropRuleCondition 
    {
        private static LocalizedText Description;

        public NotWarned()
        {
            Description ??= Language.GetOrRegister("Mods.SGAmod.Common.Conditions.NotWarned");
        }
        public bool CanDrop(DropAttemptInfo info)
        {
            return SGAWorld.craftwarning <= 30;
        }
        public bool CanShowItemDropInUI()
        {
            return true;
        }
        public string GetConditionDescription()
        {
            return Description.Value;
        }
    }
}
