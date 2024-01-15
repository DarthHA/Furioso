using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Durandal
{
    public class DurandalHeld2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Durandal");
            //DisplayName.AddTranslation(7, "杜兰达尔");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                Projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<DurandalItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = owner.direction;
            Projectile.direction = dir;
            Projectile.Center = owner.Center + new Vector2(0, 6);
            owner.heldProj = Projectile.whoAmI;
            Projectile.rotation = dir >= 0 ? MathHelper.Pi / 4 * 3 : MathHelper.Pi / 4;

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 95)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Durandal/DurandalHeld2").Value;
            Main.spriteBatch.Draw(tex, Projectile.Center + Projectile.rotation.ToRotationVector2() * 30 - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.None, 0);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}