using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Furioso.Projectiles.Allas;
using Furioso.Projectiles.Durandal;
using Furioso.Projectiles.SuperAttack;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;

namespace Furioso.Items
{
    public class FuriosoItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Furioso;

        public override int Slot => 9;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Furioso");
            DisplayName.AddTranslation(GameCulture.Chinese, "Furioso");
            Tooltip.SetDefault(
@"Can only be used after all nine Black Silence weapons hit the enemy
Take turns using nine weapons to attack, during which you are immune to all damage
All attacks will stun enemies");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"只有在九种漆黑噤默武器全部命中后才能使用
你暂时处于无敌状态，轮流使用九种武器发动攻击
所有攻击均会击晕敌人");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            item.shoot = ModContent.ProjectileType<SuperAttackHeld>();
            base.SetDefaults();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<RolandPlayer>().IsUsingGlove)
            {
                if (player.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() >= 9)
                {
                    player.GetModPlayer<RolandPlayer>().WeaponCD[Slot] = FuriosoList.WeaponsCD[Slot];
                    return true;
                }
            }
            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!SearchGlove(Main.LocalPlayer) && !Main.gamePaused)
            {
                item.TurnToAir();
                return;
            }
            Vector2 Size = Main.itemTexture[item.type].Size() * scale;
            float k = Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() / 9f;
            Vector2 Position = position - Size * 0.2f;
            Size *= 1.4f;
            Rectangle rectangle = new Rectangle((int)Position.X, (int)Position.Y + (int)(Size.Y * k), (int)Size.X + 2, (int)(Size.Y * (1 - k)));
            spriteBatch.Draw(Main.magicPixel, rectangle, frame, Color.Black * 0.75f);

        }


        public override void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.GetModPlayer<RolandPlayer>().BlackSilenceCounter.Clear();
            player.GetModPlayer<RolandPlayer>().MaskImmune = false;
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(player.Center, ShootVel, ModContent.ProjectileType<SuperAttackHeld>(), player.GetRolandDamage(DamageValue.Furioso), knockBack, player.whoAmI);
        }
    }
}