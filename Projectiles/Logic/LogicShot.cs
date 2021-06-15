using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Logic
{
    public class LogicShot : ModProjectile
    {
        private float ExRot = 0;
        public override void SetStaticDefaults() //180
        {
            DisplayName.SetDefault("Ateller Logic");
            DisplayName.AddTranslation(GameCulture.Chinese, "逻辑工作室");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 600;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            if (ExRot == 0)
            {
                ExRot = Main.rand.NextFloat() * MathHelper.TwoPi;
            }
            if (projectile.ai[0] == 0)
            {
                projectile.alpha -= 50;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                    projectile.ai[0] = 1;
                }
            }
            else
            {
                projectile.alpha += 7;
                if (projectile.alpha > 250)
                {
                    projectile.Kill();
                    return;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = mod.GetTexture("Projectiles/Logic/LogicShot" + (projectile.ai[1] + 1).ToString());
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation + ExRot, tex.Size() / 2, projectile.scale * 0.75f, SpriteEffects.None, 0);
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