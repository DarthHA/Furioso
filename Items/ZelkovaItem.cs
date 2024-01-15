using Furioso.Projectiles.Zelkova;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class ZelkovaItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Zelkova;

        public override int Slot => 2;

        public override int WeaponCD => 60;

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<ZelkovaHeld>();
            base.SetDefaults();
        }

        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<ZelkovaHeld>(), player.GetRolandDamage(DamageValue.Zelkova), knockback, player.whoAmI);
        }
    }
}