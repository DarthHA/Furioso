using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.OldBoys
{
    public class OldBoysHeld : ModProjectile
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Boys Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "老男孩工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 45;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<OldBoysItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.rotation = projectile.velocity.ToRotation();
            owner.direction = dir;
            owner.heldProj = projectile.whoAmI;
            owner.SetItemTime(5);
            projectile.direction = dir;
            projectile.Center = owner.Center;
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);

            if (projectile.ai[1] == 0)
            {
                float r = projectile.rotation;
                if (dir > 0)
                {
                    r -= MathHelper.Pi / 2;
                }
                else
                {
                    r += MathHelper.Pi / 2;
                }
                int protmp = Projectile.NewProjectile(owner.Center + r.ToRotationVector2() * 15, projectile.velocity, ModContent.ProjectileType<OldBoysSwing>(), projectile.damage, projectile.knockBack, owner.whoAmI);
                (Main.projectile[protmp].modProjectile as OldBoysSwing).RelaPos = r.ToRotationVector2() * 15;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/OldBoysSwing"), owner.Center);
            }
            projectile.ai[1]++;
            if (projectile.ai[1] >= 6)
            {
                projectile.Opacity = 1;
            }
            
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            SpriteEffects SP;
            float r;
            if (projectile.velocity.X >= 0)
            {
                SP = SpriteEffects.None;
                r = projectile.rotation;
            }
            else
            {
                SP = SpriteEffects.FlipHorizontally;
                r = projectile.rotation - MathHelper.Pi;
            }
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, lightColor * projectile.Opacity, r, tex.Size() / 2, projectile.scale * 0.75f, SP, 0);

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