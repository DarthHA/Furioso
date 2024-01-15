using Terraria;
using Terraria.ModLoader;

namespace Furioso.Systems
{
    public class NerfDamageProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int NerfedDamage = 0;
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ModifyHitInfo += (ref NPC.HitInfo info) =>
            {
                info.Damage -= NerfedDamage;
            };
        }


        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
            {
                info.Damage -= NerfedDamage;
                if (NerfedDamage > 0)
                {
                    info.Knockback = 0;
                    info.SoundDisabled = true;
                    info.DustDisabled = true;
                }
                if (info.Damage <= 1)
                {
                    info.Damage = 1;
                }
            };
        }


        public override void AI(Projectile projectile)
        {
            int damage = (Main.masterMode ? 6 : Main.expertMode ? 4 : 2) * projectile.damage;
            if (NerfedDamage >= damage && NerfedDamage > 0)
            {
                projectile.active = false;
            }
        }
    }
}