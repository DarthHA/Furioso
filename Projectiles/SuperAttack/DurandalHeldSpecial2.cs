using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalHeldSpecial2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Furioso");
            DisplayName.AddTranslation(GameCulture.Chinese, "Furioso");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 200;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso())
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.direction = dir;
            projectile.Center = owner.Center + new Vector2(0, 10);

            projectile.rotation = dir >= 0 ? MathHelper.Pi / 16 * 13 : MathHelper.Pi / 16 * 3;

            projectile.ai[0]++;
            if (projectile.ai[0] >= 100)
            {
                projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, projectile.Center + projectile.rotation.ToRotationVector2() * 30 - Main.screenPosition, null, lightColor * projectile.Opacity, projectile.rotation, tex.Size() / 2, projectile.scale * 0.75f, SpriteEffects.None, 0);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}