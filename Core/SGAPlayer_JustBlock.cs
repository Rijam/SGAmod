using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using Terraria.Audio;
using SGAmod.Items.Weapons.Shields;
using SGAmod.Buffs.Cooldowns;

namespace SGAmod
{
    public partial class SGAPlayer : ModPlayer
    {

        public void StartShieldRecharge()
        {
            SoundEngine.PlaySound(SoundID.Zombie71 with { Pitch = 0.5f }, Player.Center);
        }

        public void ShieldDepleted()
        {
            CauseShieldBreak(60 * 7);
        }

        public void CauseShieldBreak(int time)
        {
            if (!tpdcpu)
            {
                Player.AddBuff(ModContent.BuffType<ShieldBreak>(), time);
                CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Color.Aquamarine, "Shield Break!", true, false);
                SoundEngine.PlaySound(SoundID.NPCHit53 with { Pitch = -0.5f}, Player.Center);
                for (int i = 0; i < 20; i += 1)
                {
                    int dust = Dust.NewDust(new Vector2(Player.Center.X - 4, Player.Center.Y - 8), 8, 16, DustID.Sandnado);
                    Main.dust[dust].scale = 0.50f;
                    Main.dust[dust].noGravity = false;
                    Main.dust[dust].velocity = Main.rand.NextVector2Circular(6f, 6f);
                }

            }
        }
        (int, int) shieldAmmounts = (5, 7);
        public bool TakeShieldHit( int damage)
        {
            int takenshielddamage = (int)(jellybruSet ? damage * Math.Max(Player.manaCost, 0.1f) : damage);

            if (GetEnergyShieldAmountAndRecharge.Item1 > takenshielddamage)
            {
                if (!Player.immune)
                {
                    SoundEngine.PlaySound(SoundID.Item93 with { Pitch = MathHelper.Clamp(-0.8f + ((GetEnergyShieldAmountAndRecharge.Item1 / (float)GetEnergyShieldAmountAndRecharge.Item2) * 1.6f), -0.75f, 0.8f) }, Player.Center);
                    energyShieldAmountAndRecharge.Item3 = 60 * (tpdcpu ? shieldAmmounts.Item1 : shieldAmmounts.Item2);
                    energyShieldAmountAndRecharge.Item1 -= takenshielddamage;

                    Player.immune = true;
                    Player.immuneTime = 20;
                    return true;
                }
                
            }
            damage -= GetEnergyShieldAmountAndRecharge.Item1;

            if (GetEnergyShieldAmountAndRecharge.Item1 > 0)
            {
                ShieldDepleted();
                energyShieldAmountAndRecharge.Item1 = 0;
            }
            return false;
        }
        private bool ShieldJustBlock(int blocktime, Projectile shield, Vector2 where, int damage, int damageSourceIndex)
        {
            if (blocktime < 30 && ActionCooldownStack_AddCooldownStack(60 * 3))
            {
                for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.Pi / 10f)
                {
                    Vector2 randomcircle = Main.rand.NextVector2CircularEdge(2f, 4f).RotatedBy(shield.velocity.ToRotation());
                    int dust = Dust.NewDust(shield.Center, 0, 0, DustID.Shadowflame);
                    Main.dust[dust].scale = 1.5f;
                    Main.dust[dust].velocity = randomcircle * 3f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].shader = GameShaders.Armor.GetShaderFromItemId(ItemID.AcidDye);
                }
                Player.GetModPlayer<SGAPlayer>().realIFrames = 30;
                SoundEngine.PlaySound(SoundID.NPCHit4 with { Pitch = 0.5f });

                (shield.ModProjectile as CorrodedShieldProj).JustBlock(blocktime, where,  damage, damageSourceIndex);

                if(damageSourceIndex > 0)
                {
					//add accessory that affect shield effects here
                }
                return true;
            }
            return false;
        }

        protected bool ShieldDamageCheck(Vector2 where, int damage, int damageSourceIndex)
        {
            Vector2 itavect = where - Player.Center;
            itavect.Normalize();

            if (Player.GetModPlayer<SGAPlayer>().heldShield >= 0 && Player.ownedProjectileCounts[ModContent.ProjectileType<CapShieldToss>()] < 1)
            {
                int heldShield = Player.GetModPlayer<SGAPlayer>().heldShield;
                int foundhim = -1;
                Projectile proj = Main.projectile[heldShield];
                if (proj.active)
                {
                    foreach (Projectile proj2 in Main.projectile.Where(testby => testby.ModProjectile != null && testby.ModProjectile is CorrodedShieldProj))
                    {
                        proj = proj2;
                        foundhim = heldShield;

                        if (foundhim > -1)
                        {
                            CorrodedShieldProj modShieldProj = proj.ModProjectile as CorrodedShieldProj;
                            if (modShieldProj == null)
                                return false;
                            int blocktime = modShieldProj.Blocktimer;
                            bool blocking = modShieldProj.Blocking;

                            if (proj == null || blocktime < 2 || !blocking)
                                continue;// return false;

                            Vector2 itavect2 = Main.projectile[foundhim].Center - Player.Center;
                            itavect2.Normalize();
                            Vector2 angl = Vector2.Normalize(proj.velocity);
                            float diff = Vector2.Dot(itavect, angl);

                            if (diff > (proj.ModProjectile as CorrodedShieldProj).BlockAnglePublic - Player.GetModPlayer<SGAPlayer>().shieldBlockAngle && realIFrames <= 0)
                            {
                                if (ShieldJustBlock(blocktime, proj, where, damage ,damageSourceIndex))
                                    return true;

                                float damageval = 1f - modShieldProj.BlockDamagePublic;
                                damage = (int)(damage * damageval);

                                SoundEngine.PlaySound(SoundID.NPCHit4 with { Volume = 0.6f, Pitch = 1.5f });

                                if (/*!NoHitCharm && */ !(proj.ModProjectile as CorrodedShieldProj).HandleBlock(ref damage, Player))
                                    return true;

                                continue;
                            }

                        }
                    }
                }
                
            }
            return false;
        }
    }
}
