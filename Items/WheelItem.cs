using Furioso.Projectiles.Wheel;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class WheelItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Wheel;

        public override int Slot => 8;

        public override int WeaponCD => 240;

        public override void SetDefaults()
        {
            Item.knockBack *= 2;
            Item.shoot = ModContent.ProjectileType<WheelHeld>();
            base.SetDefaults();
        }

        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<WheelHeld>(), player.GetRolandDamage(DamageValue.Wheel), knockback, player.whoAmI);
        }
    }
}