using Furioso.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Furioso
{
    public class FuriosoNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Vector2? StoredVelocity = null;

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (npc.HasBuff(ModContent.BuffType<FuriosoStun>()))
            {
                int realDef = defense;
                realDef -= (npc.ichor ? 20 : 0) + (npc.betsysCurse ? 40 : 0);
                if (realDef < 0) realDef = 0;
                damage += realDef / 2;
            }
            else if (npc.HasBuff(ModContent.BuffType<AllasBrokenArmor>()))
            {
                int realDef = defense;
                realDef -= (npc.ichor ? 20 : 0) + (npc.betsysCurse ? 40 : 0);
                if (realDef < 0) realDef = 0;
                if (realDef < 60)
                {
                    damage += realDef / 2;
                }
                else
                {
                    damage += 30;
                }
            }
            return true;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(ModContent.BuffType<RangaBleeding>()))
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= DamageValue.RangaDot;
                if (damage < DamageValue.RangaDot / 3)
                {
                    damage = DamageValue.RangaDot / 3;
                }
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<WheelStun>()) || npc.HasBuff(ModContent.BuffType<FuriosoStun>()))
            {
                if (StoredVelocity == null)
                {
                    StoredVelocity = npc.velocity;
                }
                npc.velocity = Vector2.Zero;
                return false;
            }
            else
            {
                if (StoredVelocity != null)
                {
                    npc.velocity = StoredVelocity.Value;
                    StoredVelocity = null;
                }
            }
            return true;
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (npc.HasBuff(ModContent.BuffType<FuriosoStun>()))
            {
                return false;
            }
            return true;
        }

        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.HasBuff(ModContent.BuffType<WheelStun>()) || npc.HasBuff(ModContent.BuffType<FuriosoStun>()))
            {
                if (npc.frameCounter > 0)
                {
                    npc.frameCounter--;
                }
            }
        }
    }
}