using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.Allas;
using Furioso.Projectiles.Ranga;
using Terraria.Localization;

namespace Furioso.Items
{
    public class RangaItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Ranga;

        public override int Slot => 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ranga Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "琅琊工坊");
            Tooltip.SetDefault(
@"Cooldown time: 1 seconds
Use the claws to stab in the selected direction twice, and then use the dagger to stab once
Hitting an enemy can apply bleeding debuff");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"冷却时间：1秒
使用钢爪向指定方向冲刺突击两次，然后使用匕首突刺一次
击中敌人可以施加流血debuff");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<RangaHeld>();
            base.SetDefaults();
            item.knockBack *= 0.5f;
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            float dist = Main.MouseWorld.X - player.Center.X;
            dist = MathHelper.Clamp(dist, -150, 150) + player.Center.X;
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<RangaHeld>(), player.GetRolandDamage(DamageValue.Ranga), knockBack, player.whoAmI, dist, 0);
        }
    }
}