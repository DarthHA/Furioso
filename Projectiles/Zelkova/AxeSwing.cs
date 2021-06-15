using Furioso.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Zelkova
{
    public class AxeSwing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zelkova Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "榉树工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 370;
            projectile.height = 370;
            projectile.timeLeft = 40;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 50;
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

            projectile.Center = owner.Center + projectile.velocity;
            if (projectile.timeLeft > 30)
            {
                projectile.Opacity = 1;
            }
            else
            {
                projectile.Opacity = projectile.timeLeft / 30f;
            }
        }

        public void DrawAlt(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            int dir = Math.Sign(projectile.velocity.X);
            SpriteEffects SP = dir < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation, tex.Size() / 2, projectile.scale * 0.85f, SP, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            bool Exist = false;
            foreach(Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<ZelkovaHeldAxe>() && proj.owner == projectile.owner)
                {
                    Exist = true;
                    break;
                }
            }
            if (!Exist)
            {
                DrawAlt(spriteBatch);
            }
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            Player owner = Main.player[projectile.owner];
            if (Math.Sign(target.Center.X - owner.Center.X + 0.01f) == Math.Sign(owner.direction + 0.01f))
            {
                return null;
            }
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