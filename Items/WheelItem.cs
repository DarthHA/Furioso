using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.Allas;
using Furioso.Projectiles.Wheel;
using Terraria.Localization;

namespace Furioso.Items
{
    public class WheelItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Wheel;

        public override int Slot => 8;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wheel Industry");
            DisplayName.AddTranslation(GameCulture.Chinese, "轮盘重工");
            Tooltip.SetDefault(
@"Cooldown time: 4 seconds
Use the giant blade to smash forward and stun the enemy");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"冷却时间：4秒
使用巨刃向前砸击，并击晕敌人");
            base.SetStaticDefaults();
            item.knockBack *= 2;
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<WheelHeld>();
            base.SetDefaults();
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<WheelHeld>(), player.GetRolandDamage(DamageValue.Wheel), knockBack, player.whoAmI);
        }
    }
}