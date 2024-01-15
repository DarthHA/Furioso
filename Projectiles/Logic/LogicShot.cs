using Furioso.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Logic
{
    public class LogicShot : ModProjectile
    {
        private float ExRot = 0;
        public override void SetStaticDefaults() //180
        {
            // DisplayName.SetDefault("Ateller Logic");
            //DisplayName.AddTranslation(7, "逻辑工作室");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            if (ExRot == 0)
            {
                ExRot = Main.rand.NextFloat() * MathHelper.TwoPi;
            }
            if (Projectile.ai[0] == 0)
            {
                Projectile.alpha -= 50;
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                    Projectile.ai[0] = 1;
                }
            }
            else
            {
                Projectile.alpha += 7;
                if (Projectile.alpha > 250)
                {
                    Projectile.Kill();
                    return;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Logic/LogicShot" + (Projectile.ai[1] + 1).ToString()).Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation + ExRot, tex.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.None, 0);
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