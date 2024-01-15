using Furioso.Buffs;
using Furioso.Items;
using Furioso.Systems;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Utils
{
    public static class SomeUtils
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

        public static void SetItemTime(this Player player, int time)
        {
            player.itemTime = Math.Max(time, player.itemTimeMax);
            player.itemAnimation = Math.Max(time, player.itemAnimationMax);
        }

        public static bool IsBlackSilence(this Player player)
        {
            if (player.name == "Roland" || player.name == "roland" || player.name == "Orlando" || player.name == "orlando" || player.name == "Black Silence" || player.name == "BlackSilence" || player.name == "blacksilence" || player.name == "black silence" ||
                player.name == "罗兰" || player.name == "漆黑噤默" || player.name == "漆黑缄默" || player.name == "奥兰多")
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
            if (buffType == ModContent.BuffType<FuriosoStun>())
            {
                DeepAddBuffFuriosoStun(target, buffTime);
            }
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


        private static void DeepAddBuffFuriosoStun(NPC target, int buffTime)
        {
            if (target.realLife == -1)
            {
                target.GetGlobalNPC<FuriosoNPC>().FuriosoStun = buffTime;
            }
            if (target.realLife >= 0)
            {
                if (Main.npc[target.realLife].active)
                {
                    Main.npc[target.realLife].GetGlobalNPC<FuriosoNPC>().FuriosoStun = buffTime;

                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active)
                        {
                            if (npc.active && npc.realLife == target.realLife)
                            {
                                if (npc.whoAmI != target.whoAmI && npc.whoAmI != target.realLife)
                                {
                                    npc.GetGlobalNPC<FuriosoNPC>().FuriosoStun = buffTime;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 便宜真伤
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="dmg"></param>
        public static void DealTrueDamage(NPC npc, int dmg)
        {
            NPC realTarget = npc.realLife == -1 ? npc : Main.npc[npc.realLife];
            if (realTarget.immortal) return;
            realTarget.life -= dmg;
            Main.LocalPlayer.addDPS(dmg);
            if (realTarget.life <= 0)
            {
                realTarget.checkDead();
            }
        }

        /// <summary>
        /// 便宜翻译
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetTranslation(string key)
        {
            return Language.GetTextValue("Mods.Furioso." + key);
        }
    }

}