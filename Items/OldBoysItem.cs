using Furioso.Projectiles.OldBoys;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class OldBoysItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.OldBoy;

        public override int Slot => 1;

        public override int WeaponCD => 60;

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<OldBoysHeld>();
            base.SetDefaults();
            Item.knockBack *= 0.5f;
        }

        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<OldBoysHeld>(), player.GetRolandDamage(DamageValue.OldBoy), knockback, player.whoAmI);
        }
    }
}