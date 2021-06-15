using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.Allas;
using Furioso.Projectiles.Crystal;
using Terraria.Localization;

namespace Furioso.Items
{
    public class CrystalItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Crystal;

        public override int Slot => 7;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Atelier");
            DisplayName.AddTranslation(GameCulture.Chinese, "卡莉斯塔工作室");
            Tooltip.SetDefault(
@"Cooldown time: 3 seconds
Use dual sword to dash and chop twice in the selected direction
Can dodge enemy attacks and restore health equal as the damage");
            Tooltip.AddTranslation(GameCulture.Chinese,
 @"冷却时间：3秒
使用双刀向指定方向冲刺劈砍两次
可以闪避敌人攻击并回复对应伤害量");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<CrystalHeld>();
            base.SetDefaults();
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            float dist = Main.MouseWorld.X - player.Center.X;
            dist = MathHelper.Clamp(dist, -150, 150) + player.Center.X;
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<CrystalHeld>(), player.GetRolandDamage(DamageValue.Crystal), knockBack, player.whoAmI, dist, 0);
        }
    }
}