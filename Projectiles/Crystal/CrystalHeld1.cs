using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Crystal
{
    public class CrystalHeld1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Atelier");
            DisplayName.AddTranslation(GameCulture.Chinese, "卡莉斯塔工作室");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 50;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<CrystalItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.direction = dir;
            projectile.Center = owner.Center + new Vector2(0, -4);
            owner.heldProj = projectile.whoAmI;
            projectile.rotation = dir > 0 ? -MathHelper.Pi / 12 : MathHelper.Pi / 12;
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            Texture2D tex = Main.projectileTexture[projectile.type];
            Texture2D tex2 = mod.GetTexture("Projectiles/Crystal/CrystalSlash122");
            if (dir > 0)
            {
                spriteBatch.Draw(tex, projectile.position - Main.screenPosition, null, lightColor * projectile.Opacity, -MathHelper.Pi / 12, new Vector2(tex.Width, 0), projectile.scale * 0.75f, SpriteEffects.FlipHorizontally, 0);
                spriteBatch.Draw(tex2, projectile.Center + new Vector2(-65, 60) - Main.screenPosition, null, Color.White * projectile.Opacity, 0, tex2.Size() / 2, projectile.scale * 0.25f, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(tex, projectile.position - Main.screenPosition, null, lightColor * projectile.Opacity, MathHelper.Pi / 12, Vector2.Zero, projectile.scale * 0.75f, SpriteEffects.None, 0);
                spriteBatch.Draw(tex2, projectile.Center + new Vector2(65, 60) - Main.screenPosition, null, Color.White * projectile.Opacity, 0, tex2.Size() / 2, projectile.scale * 0.25f, SpriteEffects.FlipHorizontally, 0);
            }
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