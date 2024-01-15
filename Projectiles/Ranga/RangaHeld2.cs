using Furioso.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Ranga
{
    public class RangaHeld2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ranga WorkShop");
            //DisplayName.AddTranslation(7, "琅琊工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 30;
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
            Projectile.Center = owner.Center;
            Projectile.velocity = new Vector2(owner.direction, 0);
            owner.heldProj = Projectile.whoAmI;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Ranga/RangaHeld2").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("Furioso/Projectiles/Ranga/Slash22").Value;
            int dir = Math.Sign(Projectile.velocity.X);
            SpriteEffects SP = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 DrawPos1 = Projectile.Center + new Vector2(15, 0) * dir + new Vector2(0, 6);
            Vector2 DrawPos2 = Projectile.Center + new Vector2(15, 0) * dir + new Vector2(0, 12);
            Vector2 origin2 = new Vector2(tex2.Width - 126, 46);
            if (dir < 0) origin2 = new Vector2(126, 46);
            Main.spriteBatch.Draw(tex, DrawPos1 - Main.screenPosition, null, lightColor * Projectile.Opacity, 0, tex.Size() / 2, Projectile.scale * 0.25f, SP, 0);
            Main.spriteBatch.Draw(tex2, DrawPos2 - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, origin2, Projectile.scale * 0.25f, SP, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            foreach (Projectile slash in Main.projectile)
            {
                if (slash.active && slash.type == ModContent.ProjectileType<RangaSlash2>() && slash.owner == Projectile.owner)
                {
                    (slash.ModProjectile as RangaSlash2).DrawAlt(Main.spriteBatch);
                }
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}