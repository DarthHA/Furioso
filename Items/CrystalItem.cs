using Furioso.Projectiles.Crystal;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class CrystalItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Crystal;

        public override int Slot => 7;

        public override int WeaponCD => 180;

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<CrystalHeld>();
            base.SetDefaults();
        }

        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            float dist = Main.MouseWorld.X - player.Center.X;
            dist = MathHelper.Clamp(dist, -150, 150) + player.Center.X;
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<CrystalHeld>(), player.GetRolandDamage(DamageValue.Crystal), knockback, player.whoAmI, dist, 0);
        }
    }
}