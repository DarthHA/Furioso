using Terraria;
using Terraria.ModLoader;

namespace Furioso
{
    public class FuriosoGProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int NerfedDamage = 0;
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage -= NerfedDamage;
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            damage -= NerfedDamage;
        }
        public override void AI(Projectile projectile)
        {
            int damage = Main.expertMode ? projectile.damage * 4 : projectile.damage;
            if (NerfedDamage >= damage && NerfedDamage > 0)
            {
                projectile.active = false;
            }
        }
    }
}