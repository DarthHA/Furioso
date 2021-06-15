using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.Allas;
using Terraria.Localization;

namespace Furioso.Items
{
    public class AllasItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Allas;

        public override int Slot => 6;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Allas Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "阿拉斯工坊");
            Tooltip.SetDefault(
@"Cooldown time: 2 seconds
Use the giant lance to penetrate two times forward
Hitting an enemy can temporarily lower its defense");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"冷却时间：2秒
使用巨骑枪向前发动两次突刺
击中敌人可以暂时降低其防御");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<AllasHeld>();
            base.SetDefaults();
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<AllasHeld>(), player.GetRolandDamage(DamageValue.Allas), knockBack, player.whoAmI);
        }
    }
}