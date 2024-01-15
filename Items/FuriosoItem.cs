using Furioso.Projectiles.SuperAttack;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class FuriosoItem : BaseRolandItem
    {
        public override DamageValue.DiceValue DiceDamage => DamageValue.Furioso;

        public override int Slot => 9;

        public override int WeaponCD => 5;

        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<SuperAttackHeld>();
            base.SetDefaults();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.GetModPlayer<RolandPlayer>().IsUsingGlove)
            {
                if (player.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() >= 9)
                {
                    player.GetModPlayer<RolandPlayer>().WeaponCD[Slot] = SkillData.WeaponsCD[Slot];
                    return true;
                }
            }
            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!SearchGlove(Main.LocalPlayer) && !Main.gamePaused)
            {
                Item.TurnToAir();
                return;
            }
            Vector2 Size = ModContent.Request<Texture2D>("Furioso/Items/Roland9").Value.Size() * scale;
            float k = Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() / 9f;
            Vector2 Position = position - Size * 0.7f;
            Size *= 1.4f;
            Rectangle rectangle = new Rectangle((int)Position.X, (int)Position.Y + (int)(Size.Y * k), (int)Size.X + 2, (int)(Size.Y * (1 - k)));
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, frame, Color.Black * 0.75f);

        }


        public override void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.GetModPlayer<RolandPlayer>().BlackSilenceCounter.Clear();
            player.GetModPlayer<RolandPlayer>().MaskImmune = false;
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - player.Center);
            Projectile.NewProjectile(source, player.Center, ShootVel, ModContent.ProjectileType<SuperAttackHeld>(), player.GetRolandDamage(DamageValue.Furioso), knockback, player.whoAmI);
        }
    }
}