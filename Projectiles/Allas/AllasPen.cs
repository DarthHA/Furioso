using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Allas
{
    public class AllasPen : ModProjectile
    {
        public override bool Autoload(ref string name)
        {
            return false;
        }
        public Vector2 RelaPos = Vector2.Zero;
        public override void SetStaticDefaults()  //600 250
        {
            DisplayName.SetDefault("Allas Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "阿拉斯工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 45;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.ownerHitCheck = true;
        }
        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (RelaPos == Vector2.Zero)
            {
                RelaPos = projectile.Center - owner.Center;
            }
            else
            {
                RelaPos += projectile.rotation.ToRotationVector2() * 0.4f;
            }
            projectile.Center = RelaPos + owner.Center;
            projectile.velocity *= 0.93f;
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.ai[0]++;
            if (projectile.ai[0] < 20)
            {
                projectile.scale = 1 + projectile.ai[0] / 40;
            }
            else
            {
                projectile.scale = 1.5f;
            }
            if (projectile.ai[0] < 30)
            {
                projectile.Opacity = 1;
            }
            else
            {
                projectile.Opacity = (45 - projectile.ai[0]) / 15f;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center - projectile.rotation.ToRotationVector2() * 150, projectile.Center + projectile.rotation.ToRotationVector2() * 250, 250, ref point);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation, tex.Size() / 2, projectile.scale * 0.3f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override void CutTiles()
        {
            Player owner = Main.player[projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center - projectile.rotation.ToRotationVector2() * 150, projectile.Center + projectile.rotation.ToRotationVector2() * 250, (250 + 16) * projectile.scale, DelegateMethods.CutTiles);
        }


    }
}