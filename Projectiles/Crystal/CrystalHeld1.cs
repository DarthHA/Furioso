using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Crystal
{
    public class CrystalHeld1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Atelier");
            //DisplayName.AddTranslation(7, "卡莉斯塔工作室");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 50;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<CrystalItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;
            Projectile.Center = owner.Center + new Vector2(0, -4);
            owner.heldProj = Projectile.whoAmI;
            Projectile.rotation = dir > 0 ? -MathHelper.Pi / 12 : MathHelper.Pi / 12;
            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Crystal/CrystalHeld1").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("Furioso/Projectiles/Crystal/CrystalSlash122").Value;
            if (dir > 0)
            {
                Main.spriteBatch.Draw(tex, Projectile.position - Main.screenPosition, null, lightColor * Projectile.Opacity, -MathHelper.Pi / 12, new Vector2(tex.Width, 0), Projectile.scale * 0.75f, SpriteEffects.FlipHorizontally, 0);
                Main.spriteBatch.Draw(tex2, Projectile.Center + new Vector2(-65, 60) - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, tex2.Size() / 2, Projectile.scale * 0.25f, SpriteEffects.None, 0);
            }
            else
            {
                Main.spriteBatch.Draw(tex, Projectile.position - Main.screenPosition, null, lightColor * Projectile.Opacity, MathHelper.Pi / 12, Vector2.Zero, Projectile.scale * 0.75f, SpriteEffects.None, 0);
                Main.spriteBatch.Draw(tex2, Projectile.Center + new Vector2(65, 60) - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, tex2.Size() / 2, Projectile.scale * 0.25f, SpriteEffects.FlipHorizontally, 0);
                Main.spriteBatch.Draw(tex2, Projectile.Center + new Vector2(65, 60) - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, tex2.Size() / 2, Projectile.scale * 0.25f, SpriteEffects.FlipHorizontally, 0);
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