using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Logic
{
    public class LogicGunExplosion1 : ModProjectile
    {
        public int CanHit = 0;
        public override void SetStaticDefaults() //180
        {
            // DisplayName.SetDefault("Ateller Logic");
            //DisplayName.AddTranslation(7, "逻辑工作室");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 65;
            Projectile.extraUpdates = 1;
            Projectile.ownerHitCheck = true;
        }
        public override void AI()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 99999;
            Projectile.rotation = Projectile.velocity.ToRotation();
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
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Logic/LogicGunExplosion1").Value;
            SpriteEffects SP = Projectile.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (Projectile.velocity.X < 0)
            {
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation + MathHelper.Pi, tex.Size() / 2, Projectile.scale * 0.5f, SP, 0);
            }
            else
            {
                Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.5f, SP, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2000, 160, ref point);
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<LogicShot>(), 0, 0, 0);
            Main.projectile[protmp].ai[1] = 0;
            CanHit++;

            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Logic);
            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (CanHit > 3)
            {
                return false;
            }
            return null;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Terraria.Utils.PlotTileLine(Projectile.Center - Projectile.rotation.ToRotationVector2() * 128, Projectile.Center + Projectile.rotation.ToRotationVector2() * 2000, (160 + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
        

    }
}