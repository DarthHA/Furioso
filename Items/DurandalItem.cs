using Furioso.Projectiles.Durandal;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class DurandalItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Durandal;

        public override int Slot => 4;

        public override int WeaponCD => 120;

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<DurandalHeld>();
            Item.knockBack *= 0.5f;
            base.SetDefaults();
        }


        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit += (1 - crit) * 0.5f;
        }

        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<DurandalHeld>(), player.GetRolandDamage(DamageValue.Durandal), knockback, player.whoAmI);
        }
    }
}