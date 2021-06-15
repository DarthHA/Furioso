using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Mook
{
    public class MookHeld : ModProjectile
    {
        Vector2 ShootPos = Vector2.Zero;
        float ops2 = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mook Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "墨工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 99999;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<MookItem>()))
            {
                projectile.Kill();
                return;
            }
            if (projectile.ai[0] == 0)
            {
                ShootPos = Vector2.Normalize(Main.MouseWorld - owner.Center) * 250;
            }

            projectile.ai[0]++;
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.direction = dir;
            projectile.Center = owner.Center + new Vector2(-25 * dir, 15);
            owner.heldProj = projectile.whoAmI;
            owner.direction = dir;
            owner.SetItemTime(5);
            float r = MathHelper.Pi / 2 - dir * MathHelper.Pi / 6;
            owner.itemRotation = (float)Math.Atan2(r.ToRotationVector2().Y * dir, r.ToRotationVector2().X * dir);
            if (projectile.ai[0] == 5)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MookStart"), owner.Center);
            }
            if (projectile.ai[0] == 10)
            {
                Vector2 SlashPos = owner.Center + ShootPos;
                int protmp = Projectile.NewProjectile(SlashPos, Vector2.Zero, ModContent.ProjectileType<MookSlash1>(), projectile.damage / 120, 0, owner.whoAmI);
                Main.projectile[protmp].rotation = ShootPos.ToRotation();
                Main.projectile[protmp].scale = 2.5f;
            }
            if (projectile.ai[0] == 20)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MookAtk"), owner.Center);
            }
            if (projectile.ai[0] == 50)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/MookEnd"), owner.Center);
            }
            if (projectile.ai[0] == 60)
            {
                Vector2 SlashPos = owner.Center + ShootPos;
                int protmp = Projectile.NewProjectile(SlashPos, Vector2.Zero, ModContent.ProjectileType<MookSlash1>(), projectile.damage / 120, 0, owner.whoAmI);
                Main.projectile[protmp].rotation = (-ShootPos).ToRotation();
                Main.projectile[protmp].scale = 1.5f;
            }
            if (projectile.ai[0] > 61 && projectile.ai[0] <= 69)
            {
                ops2 = (69 - projectile.ai[0]) / 8;
            }
            if (projectile.ai[0] > 15 && projectile.ai[0] < 55)
            {
                if (projectile.ai[0] % 3 == 2) 
                {
                    Vector2 SlashPos = owner.Center + ShootPos + (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2() * Main.rand.Next(40);
                    int protmp = Projectile.NewProjectile(SlashPos, Vector2.Zero, ModContent.ProjectileType<MookSlash2>(), projectile.damage / 160, 0, owner.whoAmI);
                    Main.projectile[protmp].rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Main.projectile[protmp].scale = 0.6f + Main.rand.NextFloat() * 0.8f;
                    Main.projectile[protmp].ai[0] = Main.rand.Next(2) * 2 - 1;
                    Main.projectile[protmp].ai[1] = (float)Math.Pow(MathHelper.Lerp(0.1f, 1f, Main.rand.NextFloat()), 2);
                }
                if (projectile.ai[0] % 10 == 2)
                {
                    Vector2 SlashPos = owner.Center + ShootPos;
                    int protmp = Projectile.NewProjectile(SlashPos, Vector2.Zero, ModContent.ProjectileType<MookSlash1>(), projectile.damage / 160, 0, owner.whoAmI);
                    Main.projectile[protmp].rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Main.projectile[protmp].scale = MathHelper.Lerp(0.2f, 1.8f, Main.rand.NextFloat());
                }
            }

            if (projectile.ai[0] > 75)
            {
                projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Player owner = Main.player[projectile.owner];
            Texture2D tex = Main.projectileTexture[projectile.type];
            if ((projectile.ai[0] > 5 && projectile.ai[0] < 15) || (projectile.ai[0] > 50 && projectile.ai[0] < 60))
            {
                tex = mod.GetTexture("Projectiles/Mook/MookHeld2");
            }
            SpriteEffects SP = owner.direction >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, lightColor, 0, tex.Size() / 2, projectile.scale * 0.65f, SP, 0);
            if (ops2 > 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Texture2D tex2 = mod.GetTexture("Projectiles/Mook/MookExplosion");
                float k = 1 - ops2;
                spriteBatch.Draw(tex2, owner.Center + ShootPos - Main.screenPosition, null, Color.White * ops2 * 0.4f, 0, tex2.Size() / 2, projectile.scale * (k * 2f + 0.5f), SP, 0);
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