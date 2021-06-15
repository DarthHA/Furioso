using Furioso.Buffs;
using Furioso.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace Furioso
{
    public static class DamageValue
    {
		public static int BlackSilenceCardCounter = 0;

        public struct DiceValue
        {
            public float Min;
            public float Max;
            public DiceValue(float min, float max)
            {
                Min = min;
                Max = max;
            }
            public float GetDamage()
            {
                return Main.rand.NextFloat() * (Max - Min) + Min;
            }
        }
        /*
         * 逻辑工作室：无特效，左轮四穿霰弹枪无限穿
         * 阿拉斯：命中破甲
         * 老男孩：抵挡伤害和弹幕
         * 墨工坊：额外真伤
         * 琅琊：上dot
         * 榉树：暂无特效,反伤
         * 轮盘重工：击晕
         * 卡莉斯塔：闪避攻击时回血
         * 杜兰达尔：提高攻击和暴击
         * Furioso：击晕 + 防御暂时归0 + 接触伤害大幅降低 + dot，处在该攻击状态时无敌
         * 认知阻碍面具：提供间歇性闪避
        */
        public static DiceValue Logic = new DiceValue(5, 8);
        public static DiceValue Allas = new DiceValue(7, 15);
        public static DiceValue OldBoy = new DiceValue(4, 8);
        public static DiceValue Mook = new DiceValue(6, 10);
        public static DiceValue Ranga = new DiceValue(3, 7);
        public static DiceValue Zelkova = new DiceValue(4, 8);
        public static DiceValue Wheel = new DiceValue(14, 24);
        public static DiceValue Crystal = new DiceValue(6, 10);
        public static DiceValue Durandal = new DiceValue(5, 9);
        public static DiceValue Furioso = new DiceValue(20, 39);

        public static float OldBoysBlock = 0.05f;
        public static float MookTrueDamage = 0.4f;
        public static int RangaDot = 900;
		public static float ZelkovaDamage = 5;

        public static int GetRolandDamage(this Player player, DiceValue value)
        {
            float dmg = value.GetDamage() * 250;
			float modifier = 1 + MathHelper.Clamp(player.meleeDamage - 1, 0, 100) + MathHelper.Clamp(player.rangedDamage - 1, 0, 100);
            if (player.HasBuff(ModContent.BuffType<DurandalPower>()))
            {
				modifier += 0.5f;
            }
			BlackSilenceCardCounter++;
            if (BlackSilenceCardCounter >= 3)
            {
				BlackSilenceCardCounter = 0;
				if (player.IsBlackSilence())
				{
					modifier += 0.5f;
				}
            }
            return (int)(dmg * modifier);
        }
    }

    public static class FuriosoUtils
    {
		public static bool IsHoldingItemOrFurioso(this Player player, int type = -1)
		{
			bool result = false;
			if (player.HeldItem.type == ModContent.ItemType<FuriosoItem>()) result = true;
			if (type != -1)
			{
				if (player.HeldItem.type == type)
				{
					result = true;
				}
			}
			return result;
		}

		public static void SetItemTime(this Player player,int time)
        {
			player.itemTime = Math.Max(time, player.itemTime);
			player.itemAnimation = Math.Max(time, player.itemTime);
        }

		public static bool IsBlackSilence(this Player player)
		{
			if (player.name == "Roland" || player.name == "Black Silence" ||
				player.name == "罗兰" || player.name == "漆黑噤默")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static void GivePlayerImmune(Player player, int time)
		{
			player.immune = true;
			player.immuneTime = time;
			player.immuneNoBlink = true;
			for (int i = 0; i < player.hurtCooldowns.Length; i++)
			{
				if (player.hurtCooldowns[i] < time)
				{
					player.hurtCooldowns[i] = time;
				}
			}
		}

		public static void DeepAddBuff(NPC target, int buffType, int buffTime, bool dot = false)
		{
			if (!dot || target.realLife == -1)
			{
				target.buffImmune[buffType] = false;
				target.AddBuff(buffType, buffTime);
			}
			if (target.realLife >= 0)
			{
				if (Main.npc[target.realLife].active)
				{
					Main.npc[target.realLife].buffImmune[buffType] = false;
					Main.npc[target.realLife].AddBuff(buffType, buffTime);
					if (!dot)
					{
						foreach (NPC npc in Main.npc)
						{
							if (npc.active)
							{
								if (npc.active && npc.realLife == target.realLife)
								{
									if (npc.whoAmI != target.whoAmI && npc.whoAmI != target.realLife)
									{
										npc.buffImmune[buffType] = false;
										npc.AddBuff(buffType, buffTime);
									}
								}
							}
						}
					}

				}
			}
		}

		public static double StrikeNPC(NPC npc, int Damage, float knockBack, int hitDirection, bool crit = false, bool fromNet = false)
		{
			bool flag = Main.netMode == NetmodeID.SinglePlayer;
			var ReflectTarget = typeof(NPC).GetField("ignorePlayerInteractions", BindingFlags.NonPublic | BindingFlags.Static);

			if (flag && (int)ReflectTarget.GetValue(new NPC()) > 0)
			{
				ReflectTarget.SetValue(new NPC(), (int)ReflectTarget.GetValue(new NPC()) - 1);
				flag = false;
			}

			if (!npc.active || npc.life <= 0)
			{
				return 0.0;
			}
			double dmg = Damage;
			int def = npc.defense;
			if (npc.ichor)
			{
				def -= 20;
			}
			if (npc.betsysCurse)
			{
				def -= 40;
			}
			if (def < 0)
			{
				def = 0;
			}

			double OriginalDmg = dmg;
			NPCLoader.StrikeNPC(npc, ref dmg, def, ref knockBack, hitDirection, ref crit);
			dmg = Main.CalculateDamage((int)OriginalDmg, def);
			if (crit)
			{
				dmg *= 2.0;
			}
			if (npc.takenDamageMultiplier > 1f)
			{
				dmg *= npc.takenDamageMultiplier;
			}

			if ((npc.takenDamageMultiplier > 1f || Damage != 9999) && npc.lifeMax > 1)
			{
				if (npc.friendly)
				{
					Color color = crit ? CombatText.DamagedFriendlyCrit : CombatText.DamagedFriendly;
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color, (int)dmg, crit, false);
				}
				else
				{
					Color color2 = crit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile;
					if (fromNet)
					{
						color2 = crit ? CombatText.OthersDamagedHostileCrit : CombatText.OthersDamagedHostile;
					}
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, (int)dmg, crit, false);
				}
			}
			if (dmg >= 1.0)
			{
				if (flag)
				{
					npc.PlayerInteraction(Main.myPlayer);
				}
				npc.justHit = true;
				if (npc.townNPC)
				{
					bool flag2 = npc.aiStyle == 7 && (npc.ai[0] == 3f || npc.ai[0] == 4f || npc.ai[0] == 16f || npc.ai[0] == 17f);
					if (flag2)
					{
						NPC npctmp = Main.npc[(int)npc.ai[2]];
						if (npctmp.active)
						{
							npctmp.ai[0] = 1f;
							npctmp.ai[1] = 300 + Main.rand.Next(300);
							npctmp.ai[2] = 0f;
							npctmp.localAI[3] = 0f;
							npctmp.direction = hitDirection;
							npctmp.netUpdate = true;
						}
					}
					npc.ai[0] = 1f;
					npc.ai[1] = 300 + Main.rand.Next(300);
					npc.ai[2] = 0f;
					npc.localAI[3] = 0f;
					npc.direction = hitDirection;
					npc.netUpdate = true;
				}
				if (npc.aiStyle == 8 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (npc.type == NPCID.RuneWizard)
					{
						npc.ai[0] = 450f;
					}
					else if (npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored)
					{
						if (Main.rand.Next(2) == 0)
						{
							npc.ai[0] = 390f;
							npc.netUpdate = true;
						}
					}
					else if (npc.type == NPCID.DesertDjinn)
					{
						if (Main.rand.Next(3) != 0)
						{
							npc.ai[0] = 181f;
							npc.netUpdate = true;
						}
					}
					else
					{
						npc.ai[0] = 400f;
					}
					npc.TargetClosest(true);
				}
				if (npc.aiStyle == 97 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.localAI[1] = 1f;
					npc.TargetClosest(true);
				}
				if (npc.type == NPCID.DetonatingBubble)
				{
					dmg = 0.0;
					npc.ai[0] = 1f;
					npc.ai[1] = 4f;
					npc.dontTakeDamage = true;
				}
				if (npc.type == NPCID.SantaNK1 && npc.life >= npc.lifeMax * 0.5 && npc.life - dmg < npc.lifeMax * 0.5)
				{
					Gore.NewGore(npc.position, npc.velocity, 517, 1f);
				}
				if (npc.type == NPCID.SpikedIceSlime)
				{
					npc.localAI[0] = 60f;
				}
				if (npc.type == NPCID.SlimeSpiked)
				{
					npc.localAI[0] = 60f;
				}
				if (npc.type == NPCID.SnowFlinx)
				{
					npc.localAI[0] = 1f;
				}
				if (!npc.immortal)
				{
					if (npc.realLife >= 0)
					{
						Main.npc[npc.realLife].life -= (int)dmg;
						npc.life = Main.npc[npc.realLife].life;
						npc.lifeMax = Main.npc[npc.realLife].lifeMax;
					}
					else
					{
						npc.life -= (int)dmg;
					}
				}
				if (knockBack > 0f && npc.knockBackResist > 0f)
				{
					float num3 = knockBack * npc.knockBackResist;
					if (num3 > 8f)
					{
						float num4 = num3 - 8f;
						num4 *= 0.9f;
						num3 = 8f + num4;
					}
					if (num3 > 10f)
					{
						float num5 = num3 - 10f;
						num5 *= 0.8f;
						num3 = 10f + num5;
					}
					if (num3 > 12f)
					{
						float num6 = num3 - 12f;
						num6 *= 0.7f;
						num3 = 12f + num6;
					}
					if (num3 > 14f)
					{
						float num7 = num3 - 14f;
						num7 *= 0.6f;
						num3 = 14f + num7;
					}
					if (num3 > 16f)
					{
						num3 = 16f;
					}
					if (crit)
					{
						num3 *= 1.4f;
					}
					int num8 = (int)dmg * 10;
					if (Main.expertMode)
					{
						num8 = (int)dmg * 15;
					}
					if (num8 > npc.lifeMax)
					{
						if (hitDirection < 0 && npc.velocity.X > -num3)
						{
							if (npc.velocity.X > 0f)
							{
								npc.velocity.X -= num3;
							}
							npc.velocity.X -= num3;
							if (npc.velocity.X < -num3)
							{
								npc.velocity.X = -num3;
							}
						}
						else if (hitDirection > 0 && npc.velocity.X < num3)
						{
							if (npc.velocity.X < 0f)
							{
								npc.velocity.X += num3;
							}
							npc.velocity.X += num3;
							if (npc.velocity.X > num3)
							{
								npc.velocity.X = num3;
							}
						}
						if (npc.type == NPCID.SnowFlinx)
						{
							num3 *= 1.5f;
						}
						if (!npc.noGravity)
						{
							num3 *= -0.75f;
						}
						else
						{
							num3 *= -0.5f;
						}
						if (npc.velocity.Y > num3)
						{
							npc.velocity.Y += num3;
							if (npc.velocity.Y < num3)
							{
								npc.velocity.Y = num3;
							}
						}
					}
					else
					{
						if (!npc.noGravity)
						{
							npc.velocity.Y = -num3 * 0.75f * npc.knockBackResist;
						}
						else
						{
							npc.velocity.Y = -num3 * 0.5f * npc.knockBackResist;
						}
						npc.velocity.X = num3 * hitDirection * npc.knockBackResist;
					}
				}
				if ((npc.type == NPCID.WallofFlesh || npc.type == NPCID.WallofFleshEye) && npc.life <= 0)
				{
					for (int i = 0; i < 200; i++)
					{
						if (Main.npc[i].active && (Main.npc[i].type == NPCID.WallofFlesh || Main.npc[i].type == NPCID.WallofFleshEye))
						{
							Main.npc[i].HitEffect(hitDirection, dmg);
						}
					}
				}
				else
				{
					npc.HitEffect(hitDirection, dmg);
				}
				if (npc.HitSound != null)
				{
					Main.PlaySound(npc.HitSound, npc.position);
				}
				if (npc.realLife >= 0)
				{
					Main.npc[npc.realLife].checkDead();
				}
				else
				{
					npc.checkDead();
				}
				return dmg;
			}
			return 0.0;
		}
	}

	


	public class BlackSilenceCounter
	{
		public enum Attack
		{
			Ranga,
			OldBoys,
			Zelkova,
			Logic,
			Durandal,
			Mook,
			Allas,
			Crystal,
			Wheel,
		}

		private bool[] _attacks = new bool[9];

		public BlackSilenceCounter()
        {
			for(int i = 0; i < 9; i++)
            {
				_attacks[i] = false;
            }
        } 

		public void AddCounter(Attack info)
        {
			_attacks[(int)info] = true;
        }

		public bool IsUsed(Attack info)
        {
			return _attacks[(int)info];
        }

		public int GetCounter()
        {
			int num = 0;
			for (int i = 0; i < 9; i++)
			{
                if (_attacks[i])
                {
					num++;
                }
			}
			return num;
		}

		public void Clear()
        {
			for (int i = 0; i < 9; i++)
			{
				_attacks[i] = false;
			}
		}
	}
}