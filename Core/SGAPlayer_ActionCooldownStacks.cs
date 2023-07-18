
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SGAmod
{
	public class ActionCooldownStack
	{
		public int timeleft;
		public int timerup;
		public int maxtime;
		public int index;

		public ActionCooldownStack(int timeleft, int index)
		{
			this.timeleft = timeleft;
			this.maxtime = timeleft;
			this.index = index;
			timerup = 0;
		}
	}

	public partial class SGAPlayer : ModPlayer
	{
		public List<ActionCooldownStack> CooldownStacks;
		public bool ActionCooldown = false;
		public int MaxCooldownStacks = 1;
		public float actionCooldownRate = 1f;
		public int activestacks = 0;
		public bool noCooldownRate = false;

		public float ActionCooldownRate
		{
			get
			{
				return actionCooldownRate;
			}
			set
			{
				if (value < 0.50f)
				{
					actionCooldownRate = 0.50f - (float)Math.Pow(-(actionCooldownRate + 0.50f), 0.75f);
				}

				actionCooldownRate = value;// Math.Max(0.5f, actionCooldownRate);
			}
		}

		public void ActionCooldownStack_ResetEffects()
		{
			ActionCooldown = false;
			MaxCooldownStacks = 1;
			actionCooldownRate = 1f;
			noCooldownRate = false;
		}

		public void ActionCooldownStack_PreUpdate()
		{
			if (CooldownStacks == null)
			{
				CooldownStacks = new List<ActionCooldownStack>();
			}
		}

		public void ActionCooldownStack_LoadData(TagCompound tag)
		{
			CooldownStacks = new List<ActionCooldownStack>();
		}

		public bool ActionCooldownStack_AddCooldownStack(int time, int count = 1, bool testOnly = false)
		{
			bool weHaveStacks = CooldownStacks.Count + (count - 1) < MaxCooldownStacks;

			bool worked = false;

			for (int i = 0; i < count; i += 1)
			{

				/*bool illuSet = illuminantSet.Item1 > 4 && weHaveStacks && !testOnly && Main.rand.Next(4) == 0;

				if (illuSet)
				{
					worked = true;
					continue;
				}*/
				if (weHaveStacks)
				{
					//if (player.HasBuff(mod.BuffType("CondenserBuff")))
					//	time = (int)((float)time * 1.15f);

					if (!testOnly)
					{
						int time2 = (int)((float)time * ActionCooldownRate);

						CooldownStacks.Add(new ActionCooldownStack(time2, CooldownStacks.Count));
					}
					worked = true;
				}
			}
			return worked && weHaveStacks;

		}
		public int ActionCooldownStack_ActionStackOverflow(ActionCooldownStack stack, int stackIndex)
		{
			if (MaxCooldownStacks <= stackIndex && stack.timerup % 2 == 0)
				return 0;

			return 1;
		}
		public void ActionCooldownStack_PostUpdateEquips() //DoCooldownUpdate()
		{
			if (Main.netMode != NetmodeID.Server)//ugh
			{
				if (!noCooldownRate && CooldownStacks.Count > 0)
				{
					for (int stackindex = 0; stackindex < CooldownStacks.Count; stackindex += 1)
					{
						ActionCooldownStack stack = CooldownStacks[stackindex];
						stack.timeleft -= ActionCooldownStack_ActionStackOverflow(stack, stackindex);
						stack.timerup += 1;
						if (stack.timeleft < 1)
							CooldownStacks.RemoveAt(stackindex);
					}
				}
				activestacks = CooldownStacks.Count;
			}
		}
	}
}