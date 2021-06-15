using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Ranga
{
    public class RangaHeld1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ranga WorkShop");
            DisplayName.AddTranslation(GameCulture.Chinese, "琅琊工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 30;
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
            projectile.Center = owner.Center;
            projectile.velocity = new Vector2(owner.direction, 0);
            owner.heldProj = projectile.whoAmI;
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            Texture2D tex2 = mod.GetTexture("Projectiles/Ranga/Slash12");
            int dir = Math.Sign(projectile.velocity.X);
            SpriteEffects SP = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 DrawPos1 = projectile.Center + new Vector2(20, 0) * dir + new Vector2(0, -10);
            Vector2 DrawPos2 = projectile.Center + new Vector2(20, 0) * dir + new Vector2(0, -15);
            spriteBatch.Draw(tex, DrawPos1 - Main.screenPosition, null, lightColor * projectile.Opacity, projectile.rotation, tex.Size() / 2, projectile.scale * 0.25f, SP, 0);
            spriteBatch.Draw(tex2, DrawPos2 - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation, tex2.Size() / 2, projectile.scale * 0.25f, SP, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;

        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}