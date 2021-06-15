using Furioso.Buffs;
using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Zelkova
{
    public class ZelkovaHeldMace : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zelkova Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "榉树工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 35;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 50;
            projectile.friendly = true;
        }
        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<ZelkovaItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.direction = dir;
            projectile.Center = owner.Center;
            if (projectile.ai[0] == 1)
            {
                projectile.rotation = dir > 0 ? MathHelper.Pi / 8 * 5 : MathHelper.Pi / 8 * 3;
            }
            else
            {
                projectile.rotation = dir > 0 ? 0 : MathHelper.Pi;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            Texture2D tex = Main.projectileTexture[projectile.type];
            Texture2D tex2 = mod.GetTexture("Projectiles/Zelkova/MaceSwing2");
            if (projectile.ai[0] == 0)
            {
                Vector2 OffSet = new Vector2(6, 6);
                if (dir > 0)
                {
                    spriteBatch.Draw(tex, projectile.Center + OffSet - Main.screenPosition, null, lightColor * projectile.Opacity, 0, tex.Size(), projectile.scale * 0.3f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(tex, projectile.Center + new Vector2(-OffSet.X, OffSet.Y) - Main.screenPosition, null, lightColor * projectile.Opacity, 0, new Vector2(0, tex.Height), projectile.scale * 0.3f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else
            {
                float R1 = MathHelper.Pi / 4 * 3;
                float R2 = dir > 0 ? MathHelper.Pi / 8 * 5 : MathHelper.Pi / 8 * 3;
                R1 += R2;
                spriteBatch.Draw(tex, projectile.Center - R2.ToRotationVector2() * 6 - Main.screenPosition, null, lightColor * projectile.Opacity, R1, tex.Size(), projectile.scale * 0.3f, SpriteEffects.None, 0);
                if (dir > 0)
                {
                    spriteBatch.Draw(tex2, projectile.Center + new Vector2(-5, 95) - Main.screenPosition, null, Color.White * projectile.Opacity, 0, tex2.Size() / 2, projectile.scale * 0.25f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(tex2, projectile.Center + new Vector2(5, 95) - Main.screenPosition, null, Color.White * projectile.Opacity, 0, tex2.Size() / 2, projectile.scale * 0.25f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override bool CanDamage()
        {
            return projectile.ai[0] == 1;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projectile.ai[0] == 1)       //160
            {
                float point = 0f;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.rotation.ToRotationVector2() * 155 * projectile.scale, 20 * projectile.scale, ref point);
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            int CritRate = Math.Max(Main.player[projectile.owner].meleeCrit, Main.player[projectile.owner].rangedCrit);
            crit = Main.rand.Next(100) <= CritRate;
            damage += (int)(target.damage * DamageValue.ZelkovaDamage);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!Main.player[projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                Main.player[projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Zelkova);
            }
            else
            {
                FuriosoUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}