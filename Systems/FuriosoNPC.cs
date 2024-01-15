using Furioso.Buffs;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Furioso.Systems
{
    public class FuriosoNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Vector2? StoredVelocity = null;

        public int FuriosoStun = 0;

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (FuriosoStun > 0)
            {
                modifiers.Defense *= 0;
                modifiers.ModifyHitInfo += (ref NPC.HitInfo info) =>
                {
                    info.Damage += (int)(npc.damage * DamageValue.FuriosoMeleeModifier);
                };
            }
            if (npc.HasBuff(ModContent.BuffType<AllasBrokenArmor>()))
            {
                modifiers.Defense.Flat -= 60;
            }
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (FuriosoStun > 0)
            {
                int trueDamage = (int)(damageDone * DamageValue.FuriosoTrueDamage);
                if (trueDamage < DamageValue.FuriosoTrueDamageCap) trueDamage = DamageValue.FuriosoTrueDamageCap;
                SomeUtils.DealTrueDamage(npc, trueDamage);
            }
        }


        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            int dot = 0;

            if (npc.HasBuff(ModContent.BuffType<RangaBleeding>()))
            {
                dot += DamageValue.RangaDot;
            }
            if (FuriosoStun > 0)
            {
                dot += DamageValue.FuriosoDot;
            }
            if (dot > 0)
            {
                if (npc.lifeRegen > 0) npc.lifeRegen = 0;
                npc.lifeRegen -= dot;
                if (damage < dot / 3)
                {
                    damage = dot / 3;
                }
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<WheelStun>()) || FuriosoStun > 0)
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

        public override void PostAI(NPC npc)
        {
            if (FuriosoStun > 0) FuriosoStun--;
        }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (FuriosoStun > 0)
            {
                return false;
            }
            return true;
        }

        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (FuriosoStun > 0)
            {
                return false;
            }
            return true;
        }

        public override void FindFrame(NPC npc, int frameHeight)
        {
            if (npc.HasBuff(ModContent.BuffType<WheelStun>()) || FuriosoStun > 0)
            {
                if (npc.frameCounter > 0)
                {
                    npc.frameCounter--;
                }
            }
        }
    }
}