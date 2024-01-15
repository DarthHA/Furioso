using Furioso.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Allas
{
    public class AllasPen : ModProjectile
    {
        /*public override bool Autoload(ref string name)
        {
            return false;
        }*/
        public Vector2 RelaPos = Vector2.Zero;
        public override void SetStaticDefaults()  //600 250
        {
            // DisplayName.SetDefault("Allas Workshop");
            //DisplayName.AddTranslation(7, "阿拉斯工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 45;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ownerHitCheck = true;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                Projectile.Kill();
                return;
            }
            if (RelaPos == Vector2.Zero)
            {
                RelaPos = Projectile.Center - owner.Center;
            }
            else
            {
                RelaPos += Projectile.rotation.ToRotationVector2() * 0.4f;
            }
            Projectile.Center = RelaPos + owner.Center;
            Projectile.velocity *= 0.93f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 20)
            {
                Projectile.scale = 1 + Projectile.ai[0] / 40;
            }
            else
            {
                Projectile.scale = 1.5f;
            }
            if (Projectile.ai[0] < 30)
            {
                Projectile.Opacity = 1;
            }
            else
            {
                Projectile.Opacity = (45 - Projectile.ai[0]) / 15f;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Projectile.rotation.ToRotationVector2() * 150, Projectile.Center + Projectile.rotation.ToRotationVector2() * 250, 250, ref point);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Allas/AllasPen").Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.3f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override void CutTiles()
        {
            Player owner = Main.player[Projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Terraria.Utils.PlotTileLine(Projectile.Center - Projectile.rotation.ToRotationVector2() * 150, Projectile.Center + Projectile.rotation.ToRotationVector2() * 250, (250 + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }


    }
}