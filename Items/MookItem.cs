using Furioso.Projectiles.Mook;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class MookItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Mook;

        public override int Slot => 5;

        public override int WeaponCD => 120;

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<MookHeld>();
            base.SetDefaults();
            Item.knockBack = 0;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit = 0;
        }

        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<MookHeld>(), player.GetRolandDamage(DamageValue.Mook), 0, player.whoAmI);
        }
    }
}