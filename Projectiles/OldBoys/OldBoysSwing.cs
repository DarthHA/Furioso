using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ModLoader;

namespace Furioso.Projectiles.OldBoys
{
    public class OldBoysSwing : ModProjectile
    {
        private bool hit = false;
        public Vector2 RelaPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Old Boys Workshop");
            //DisplayName.AddTranslation(7, "老男孩工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 45;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
            Projectile.ownerHitCheck = true;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.Center = RelaPos + owner.Center;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 6)
            {
                Projectile.Opacity = Projectile.ai[0] / 6;
            }
            else
            {
                Projectile.Opacity = (45 - Projectile.ai[0]) / 39f;
            }

            if (!owner.GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                BlockProjs(owner);
            }
        }

        public void BlockProjs(Player owner)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.hostile && !proj.friendly && proj.damage > 0 && proj.GetGlobalProjectile<NerfDamageProj>().NerfedDamage == 0)
                {
                    float point = 0;
                    if (Collision.CheckAABBvLineCollision(proj.position, proj.Size, Projectile.Center - Projectile.rotation.ToRotationVector2() * 180, Projectile.Center + Projectile.rotation.ToRotationVector2() * 180, 75, ref point))
                    {
                        proj.GetGlobalProjectile<NerfDamageProj>().NerfedDamage += (int)(Projectile.damage * DamageValue.OldBoysBlock);

                        owner.GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.OldBoys);
                        if (Projectile.localAI[1] == 0)
                        {
                            Projectile.localAI[1] = 1;
                            SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/OldBoysGuard"));
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), proj.Center, Vector2.Zero, ModContent.ProjectileType<OldBoysGuard>(), 0, 0, 0);
                        }

                    }
                }
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/OldBoys/OldBoysSwing").Value;
            SpriteEffects SP;
            float r;
            if (Projectile.velocity.X >= 0)
            {
                SP = SpriteEffects.None;
                r = Projectile.rotation;
            }
            else
            {
                SP = SpriteEffects.FlipHorizontally;
                r = Projectile.rotation - MathHelper.Pi;
            }
            Main.spriteBatch.Draw(tex, Projectile.Center + Projectile.rotation.ToRotationVector2() * 30 - Main.screenPosition, null, Color.White * Projectile.Opacity, r, tex.Size() / 2, Projectile.scale * 0.75f, SP, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Projectile.rotation.ToRotationVector2() * 180, Projectile.Center + Projectile.rotation.ToRotationVector2() * 180, 75, ref point);
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/OldBoysAtk"));

            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.OldBoys);
            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Terraria.Utils.PlotTileLine(Projectile.Center - Projectile.rotation.ToRotationVector2() * 180, Projectile.Center + Projectile.rotation.ToRotationVector2() * 180, (75 + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }
    }
}