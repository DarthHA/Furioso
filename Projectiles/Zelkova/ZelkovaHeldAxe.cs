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
    public class ZelkovaHeldAxe : ModProjectile
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
            owner.heldProj = projectile.whoAmI;
            if (projectile.ai[0] == 0)
            {
                projectile.rotation = dir > 0 ? MathHelper.Pi / 6 : MathHelper.Pi / 6 * 5;
            }
            else
            {
                projectile.rotation = dir > 0 ? 0 : MathHelper.Pi;
            }
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            Texture2D tex = Main.projectileTexture[projectile.type];
            Texture2D tex2 = mod.GetTexture("Projectiles/Zelkova/AxeSwing2");
            if (projectile.ai[0] == 0)
            {
                Vector2 OffSet = new Vector2(-12, -6);
                if (dir > 0)
                {
                    spriteBatch.Draw(tex, projectile.Center + OffSet - Main.screenPosition, null, lightColor * projectile.Opacity, 0, Vector2.Zero, projectile.scale * 0.3f, SpriteEffects.None, 0);
                    spriteBatch.Draw(tex2, projectile.Center + new Vector2(67, -22) - Main.screenPosition, null, Color.White * projectile.Opacity, 0, tex2.Size() / 2, projectile.scale * 0.25f, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(tex, projectile.Center + new Vector2(-OffSet.X, OffSet.Y) - Main.screenPosition, null, lightColor * projectile.Opacity, 0, new Vector2(tex.Width, 0), projectile.scale * 0.3f, SpriteEffects.FlipHorizontally, 0);
                    spriteBatch.Draw(tex2, projectile.Center + new Vector2(-67, -22) - Main.screenPosition, null, Color.White * projectile.Opacity, 0, tex2.Size() / 2, projectile.scale * 0.25f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else
            {
                if (dir > 0)
                {
                    //spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, MathHelper.Pi / 4, new Vector2(tex.Width, 0), projectile.scale * 0.3f, SpriteEffects.FlipHorizontally, 0);
                    spriteBatch.Draw(tex, projectile.Center + new Vector2(0, 6) - Main.screenPosition, null, lightColor * projectile.Opacity, MathHelper.Pi, Vector2.Zero, projectile.scale * 0.3f, SpriteEffects.None, 0);
                }
                else
                {
                    //spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, -MathHelper.Pi / 4, Vector2.Zero, projectile.scale * 0.3f, SpriteEffects.None, 0);
                    spriteBatch.Draw(tex, projectile.Center + new Vector2(0, 6) - Main.screenPosition, null, lightColor * projectile.Opacity, MathHelper.Pi, new Vector2(tex.Width, 0), projectile.scale * 0.3f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            foreach(Projectile proj in Main.projectile)
            {
                if(proj.active && proj.type == ModContent.ProjectileType<AxeSwing>() && proj.owner == projectile.owner)
                {
                    (proj.modProjectile as AxeSwing).DrawAlt(spriteBatch);
                }
            }

            return false;
        }
        public override bool CanDamage()
        {
            return projectile.ai[0] == 0;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projectile.ai[0] == 0)       //123
            {
                float point = 0f;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.rotation.ToRotationVector2() * 110 * projectile.scale, 20 * projectile.scale, ref point);
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