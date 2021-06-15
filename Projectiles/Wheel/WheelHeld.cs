using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Wheel
{
    public class WheelHeld : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wheel Industry");
            DisplayName.AddTranslation(GameCulture.Chinese, "轮盘重工");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 70;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<WheelItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            if (projectile.ai[0] == 0)
            {
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<WheelSwing>(), projectile.damage, projectile.knockBack, owner.whoAmI, dir);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<WheelSwing2>(), 0, 0, owner.whoAmI, dir);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/WheelAtk"), owner.Center);
            }
            projectile.ai[0]++;

            projectile.direction = dir;
            projectile.Center = owner.Center;
            owner.heldProj = projectile.whoAmI;
            owner.direction = dir;
            owner.SetItemTime(5);

            if (projectile.ai[0] < 8)
            {
                projectile.Opacity = 0;
                float t = projectile.ai[0] / 8f;
                if (owner.direction >= 0)
                {
                    projectile.rotation = -MathHelper.Pi + MathHelper.Pi / 8f * 9f * t;
                }
                else
                {
                    projectile.rotation = 0 - MathHelper.Pi / 8f * 9f * t;
                }
            }
            if (projectile.ai[0] >= 8)
            {
                projectile.Opacity = 1;
                if (owner.direction >= 0)
                {
                    projectile.rotation = MathHelper.Pi / 10;
                }
                else
                {
                    projectile.rotation = MathHelper.Pi / 10 * 9;
                }
            }
            if (projectile.ai[0] == 8)
            {
                Projectile.NewProjectile(owner.Center + new Vector2(170 * owner.direction, 60), Vector2.Zero, ModContent.ProjectileType<WheelExplosion>(), projectile.damage, projectile.knockBack, owner.whoAmI);
                Projectile.NewProjectile(owner.Center + new Vector2(170 * owner.direction, 0),projectile.velocity, ModContent.ProjectileType<WheelExplosion3>(), 0, 0, owner.whoAmI);
            }
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Player owner = Main.player[projectile.owner];
            Texture2D tex = Main.projectileTexture[projectile.type];
            if (owner.direction >= 0)
            {
                spriteBatch.Draw(tex, projectile.Center + projectile.rotation.ToRotationVector2() * 100 - Main.screenPosition, null, lightColor * projectile.Opacity, projectile.rotation + MathHelper.Pi / 4, tex.Size() / 2, projectile.scale * 0.65f, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(tex, projectile.Center + projectile.rotation.ToRotationVector2() * 100 - Main.screenPosition, null, lightColor * projectile.Opacity, projectile.rotation + MathHelper.Pi / 4 * 3, tex.Size() / 2, projectile.scale * 0.65f, SpriteEffects.FlipHorizontally, 0);
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