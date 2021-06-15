using Furioso.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.OldBoys
{
    public class OldBoysSwing : ModProjectile
    {
        private bool hit = false;
        public Vector2 RelaPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Old Boys Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "老男孩工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 45;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.friendly = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 120;
            projectile.ownerHitCheck = true;
        }
        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            projectile.Center = RelaPos + owner.Center;
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.ai[0]++;
            if (projectile.ai[0] < 6)
            {
                projectile.Opacity = projectile.ai[0] / 6;
            }
            else
            {
                projectile.Opacity = (45 - projectile.ai[0]) / 39f;
            }

            if (++projectile.localAI[0] > 80)
            {
                projectile.localAI[0] = 0;
                projectile.localAI[1] = 0;
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
                if (proj.active && proj.hostile && !proj.friendly && proj.damage > 0 && proj.GetGlobalProjectile<FuriosoGProj>().NerfedDamage == 0)
                {
                    float point = 0;
                    if (Collision.CheckAABBvLineCollision(proj.position, proj.Size, projectile.Center - projectile.rotation.ToRotationVector2() * 180, projectile.Center + projectile.rotation.ToRotationVector2() * 180, 75, ref point))
                    {
                        proj.GetGlobalProjectile<FuriosoGProj>().NerfedDamage += (int)(projectile.damage * DamageValue.OldBoysBlock);
                        owner.GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.OldBoys);
                        if (projectile.localAI[1] == 0)
                        {
                            projectile.localAI[1] = 1;
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/OldBoysGuard"), owner.Center);
                        }
                        Projectile.NewProjectile(proj.Center, Vector2.Zero, ModContent.ProjectileType<OldBoysGuard>(), 0, 0, 0);
                    }
                }
            }
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            SpriteEffects SP;
            float r;
            if (projectile.velocity.X >= 0)
            {
                SP = SpriteEffects.None;
                r = projectile.rotation;
            }
            else
            {
                SP = SpriteEffects.FlipHorizontally;
                r = projectile.rotation - MathHelper.Pi;
            }
            spriteBatch.Draw(tex, projectile.Center + projectile.rotation.ToRotationVector2() * 30 - Main.screenPosition, null, Color.White * projectile.Opacity, r, tex.Size() / 2, projectile.scale * 0.75f, SP, 0);

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
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center - projectile.rotation.ToRotationVector2() * 180, projectile.Center + projectile.rotation.ToRotationVector2() * 180, 75, ref point);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            int CritRate = Math.Max(Main.player[projectile.owner].meleeCrit, Main.player[projectile.owner].rangedCrit);
            crit = Main.rand.Next(100) <= CritRate;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!hit)
            {
                hit = true;
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/OldBoysAtk"), Main.player[projectile.owner].Center);
            }
            if (!Main.player[projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                Main.player[projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.OldBoys);
            }
            else
            {
                FuriosoUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center - projectile.rotation.ToRotationVector2() * 180, projectile.Center + projectile.rotation.ToRotationVector2() * 180, (75 + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
    }
}