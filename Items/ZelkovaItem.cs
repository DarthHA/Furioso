using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.Allas;
using Furioso.Projectiles.Zelkova;
using Terraria.Localization;

namespace Furioso.Items
{
    public class ZelkovaItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Zelkova;

        public override int Slot => 2;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zelkova Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "榉树工坊");
            Tooltip.SetDefault(
@"Cooldown time: 1 seconds
Use war axe and mace to smash forward
Attacks will add enemies' melee damage to them");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"冷却时间：1秒
使用短柄斧和战锤向前砸击
攻击会附加敌人的近战伤害");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<ZelkovaHeld>();
            base.SetDefaults();
        }

        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<ZelkovaHeld>(), player.GetRolandDamage(DamageValue.Zelkova), knockBack, player.whoAmI);
        }
    }
}