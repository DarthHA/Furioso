using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.Allas;
using Furioso.Projectiles.Durandal;
using Terraria.Localization;

namespace Furioso.Items
{
    public class DurandalItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Durandal;

        public override int Slot => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Durandal");
            DisplayName.AddTranslation(GameCulture.Chinese, "杜兰达尔");
            Tooltip.SetDefault(
@"Cooldown time: 2 seconds
Use the Holy Sword Durandal to slash forward twice
Hitting an enemy can temporarily increase the damage of the Black Silence Weapons");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"冷却时间：2秒
使用圣剑杜兰达尔向前劈砍两次
命中敌人可以暂时提高漆黑噤默武器的伤害");
            base.SetStaticDefaults();
            item.knockBack *= 0.5f;
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<DurandalHeld>();
            base.SetDefaults();
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<DurandalHeld>(), player.GetRolandDamage(DamageValue.Durandal), knockBack, player.whoAmI);
        }
    }
}