using Furioso.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalSwingSpecial : ModProjectile
    {
        public Vector2 RelaPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Furioso");
            //DisplayName.AddTranslation(7, "Furioso");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.Center = RelaPos + owner.Center;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 85)
            {
                Projectile.Opacity = 1;
            }
            else
            {
                Projectile.Opacity = (100 - Projectile.ai[0]) / 15f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/SuperAttack/DurandalSwingSpecial").Value;
            SpriteEffects SP = Projectile.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, tex.Size() / 2, Projectile.scale * 0.15f, SP, 0);

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