using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Wheel
{
    public class WheelHeld : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wheel Industry");
            //DisplayName.AddTranslation(7, "轮盘重工");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 70;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                Projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<WheelItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            if (Projectile.ai[0] == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Vector2.Zero, ModContent.ProjectileType<WheelSwing>(), Projectile.damage, Projectile.knockBack, owner.whoAmI, dir);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Vector2.Zero, ModContent.ProjectileType<WheelSwing2>(), 0, 0, owner.whoAmI, dir);
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/WheelAtk"));
            }
            Projectile.ai[0]++;

            Projectile.direction = dir;
            Projectile.Center = owner.Center;
            owner.heldProj = Projectile.whoAmI;
            owner.direction = dir;
            SomeUtils.SetItemTime(owner, 5);

            if (Projectile.ai[0] < 8)
            {
                Projectile.Opacity = 0;
                float t = Projectile.ai[0] / 8f;
                if (owner.direction >= 0)
                {
                    Projectile.rotation = -MathHelper.Pi + MathHelper.Pi / 8f * 9f * t;
                }
                else
                {
                    Projectile.rotation = 0 - MathHelper.Pi / 8f * 9f * t;
                }
            }
            if (Projectile.ai[0] >= 8)
            {
                Projectile.Opacity = 1;
                if (owner.direction >= 0)
                {
                    Projectile.rotation = MathHelper.Pi / 10;
                }
                else
                {
                    Projectile.rotation = MathHelper.Pi / 10 * 9;
                }
            }
            if (Projectile.ai[0] == 8)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center + new Vector2(170 * owner.direction, 60), Vector2.Zero, ModContent.ProjectileType<WheelExplosion>(), Projectile.damage, Projectile.knockBack, owner.whoAmI);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center + new Vector2(170 * owner.direction, 0), Projectile.velocity, ModContent.ProjectileType<WheelExplosion3>(), 0, 0, owner.whoAmI);
            }
            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Player owner = Main.player[Projectile.owner];
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Wheel/WheelHeld").Value;
            if (owner.direction >= 0)
            {
                Main.spriteBatch.Draw(tex, Projectile.Center + Projectile.rotation.ToRotationVector2() * 100 - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation + MathHelper.Pi / 4, tex.Size() / 2, Projectile.scale * 0.65f, SpriteEffects.None, 0);
            }
            else
            {
                Main.spriteBatch.Draw(tex, Projectile.Center + Projectile.rotation.ToRotationVector2() * 100 - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation + MathHelper.Pi / 4 * 3, tex.Size() / 2, Projectile.scale * 0.65f, SpriteEffects.FlipHorizontally, 0);
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