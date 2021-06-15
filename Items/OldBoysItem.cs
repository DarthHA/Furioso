using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.OldBoys;
using Terraria.Localization;

namespace Furioso.Items
{
    public class OldBoysItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.OldBoy;

        public override int Slot => 1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Boys Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "老男孩工坊");
            Tooltip.SetDefault(
@"Cooldown time: 1 seconds
Use the hammer to swing forward
This attack can weaken hostile projectiles
If the projectile damage is low enough, it can destroy the projectile");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"冷却时间：1秒
用锤子向前挥击
本攻击可以削弱敌对弹幕伤害
若弹幕伤害过低则可以直接摧毁弹幕");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<OldBoysHeld>();
            base.SetDefaults();
            item.knockBack *= 0.5f;
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<OldBoysHeld>(), player.GetRolandDamage(DamageValue.OldBoy), knockBack, player.whoAmI);
        }
    }
}