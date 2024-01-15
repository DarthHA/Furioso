using Furioso.Buffs;
using Terraria;
using Terraria.ModLoader;

namespace Furioso.Utils
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
        public static DiceValue Logic = new(5, 8);
        public static DiceValue Allas = new(7, 15);
        public static DiceValue OldBoy = new(4, 8);
        public static DiceValue Mook = new(6, 10);
        public static DiceValue Ranga = new(3, 7);
        public static DiceValue Zelkova = new(4, 8);
        public static DiceValue Wheel = new(14, 24);
        public static DiceValue Crystal = new(6, 10);
        public static DiceValue Durandal = new(5, 9);
        public static DiceValue Furioso = new(20, 39);

        public static float OldBoysBlock = 0.1f;
        public static float MookTrueDamage = 0.4f;
        public static int MookTrueDamageCap = 20;
        public static int RangaDot = 900;
        public static float ZelkovaMeleeModifier = 5;
        public static int CrystalHealLife = 200;

        public static int FuriosoDot = 500;
        public static float FuriosoTrueDamage = 0.4f;
        public static int FuriosoTrueDamageCap = 50;
        public static float FuriosoMeleeModifier = 10;


        public static int GetRolandDamage(this Player player, DiceValue value)
        {
            float dmg = value.GetDamage() * 250;
            dmg = player.GetDamage(DamageClass.Ranged).ApplyTo(dmg);
            dmg = player.GetDamage(DamageClass.Melee).ApplyTo(dmg);
            dmg = player.GetDamage(DamageClass.Generic).ApplyTo(dmg);

            float modifier = 1;// + MathHelper.Clamp(player.meleeDamage - 1, 0, 100) + MathHelper.Clamp(player.rangedDamage - 1, 0, 100);
            if (player.HasBuff(ModContent.BuffType<DurandalPower>()))
            {
                modifier += 0.5f;
            }
            BlackSilenceCardCounter++;
            if (BlackSilenceCardCounter >= 3)
            {
                BlackSilenceCardCounter = 0;
                modifier += 0.5f;
            }
            return (int)(dmg * modifier);
        }
    }
}
