using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Allas
{
    public class AllasPen2 : ModProjectile
    {
        public override void SetStaticDefaults()  //442 144 400 130
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
            Projectile.localNPCHitCooldown = 200;
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
            Projectile.Center = owner.Center + Projectile.velocity;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 5)
            {
                Projectile.Opacity = Projectile.ai[0] / 5f;
            }
            else if (Projectile.ai[0] >= 30)
            {
                Projectile.Opacity = (45 - Projectile.ai[0]) / 15f;
            }
            else
            {
                Projectile.Opacity = 1;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Projectile.rotation.ToRotationVector2() * 200, Projectile.Center + Projectile.rotation.ToRotationVector2() * 200, 130 * Projectile.scale, ref point);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            bool Exist = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<AllasHeld1>() && proj.owner == Projectile.owner)
                {
                    Exist = true;
                }
            }
            if (!Exist)
            {
                DrawAlt(Main.spriteBatch);
            }
            return false;
        }

        public void DrawAlt(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Allas/AllasPen2").Value;
            spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void CutTiles()
        {
            Player owner = Main.player[Projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Terraria.Utils.PlotTileLine(Projectile.Center - Projectile.rotation.ToRotationVector2() * 200, Projectile.Center + Projectile.rotation.ToRotationVector2() * 200, (130 + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<AllasBrokenArmor>(), 900);
                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Allas);
            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}