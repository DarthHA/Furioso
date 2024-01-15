using Furioso.Projectiles.Allas;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class AllasItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Allas;

        public override int Slot => 6;

        public override int WeaponCD => 120;


        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<AllasHeld>();
            base.SetDefaults();
        }

        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<AllasHeld>(), player.GetRolandDamage(DamageValue.Allas), knockback, player.whoAmI);
        }
    }
}