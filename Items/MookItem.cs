using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.Mook;
using Terraria.Localization;

namespace Furioso.Items
{
    public class MookItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Mook;

        public override int Slot => 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mook Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "墨工坊");
            Tooltip.SetDefault(
@"Cooldown time: 2 seconds
Use the katana to slash forward at high speed
Each attack will add additional damage");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"冷却时间：2秒
使用太刀向前方高速斩击
每次攻击都会附加额外伤害");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<MookHeld>();
            base.SetDefaults();
            item.knockBack = 0;
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<MookHeld>(), player.GetRolandDamage(DamageValue.Mook), 0, player.whoAmI);
        }
    }
}