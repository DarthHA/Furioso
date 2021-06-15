using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Furioso.Items
{
    public class LogicItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Logic;

        public override int Slot => 3;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atelier Logic");
            DisplayName.AddTranslation(GameCulture.Chinese, "逻辑工作室");
            Tooltip.SetDefault(
@"Cooldown time: 2 seconds
Use dual revolvers and shotgun to launch long-range attacks");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"冷却时间：2秒
使用双枪和霰弹枪进行远程攻击");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<LogicHeld>();
            base.SetDefaults();
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<LogicHeld>(), player.GetRolandDamage(DamageValue.Logic), knockBack, player.whoAmI);
        }
    }
}