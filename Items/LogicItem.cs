using Furioso.Projectiles.Logic;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class LogicItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Logic;

        public override int Slot => 3;

        public override int WeaponCD => 120;

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<LogicHeld>();
            base.SetDefaults();
        }

        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<LogicHeld>(), player.GetRolandDamage(DamageValue.Logic), knockback, player.whoAmI);
        }
    }
}