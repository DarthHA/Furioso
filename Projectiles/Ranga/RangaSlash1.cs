using Furioso.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Ranga
{
    public class RangaSlash1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ranga WorkShop");
            DisplayName.AddTranslation(GameCulture.Chinese, "琅琊工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 321;
            projectile.height = 100;
            projectile.timeLeft = 30;
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
            projectile.Center = owner.Center;
            projectile.velocity = new Vector2(owner.direction, 0);
            if (projectile.timeLeft >= 25)
            {
                projectile.Opacity = (30 - projectile.timeLeft) / 5f;
            }
            else if (projectile.timeLeft < 15)
            {
                projectile.Opacity = projectile.timeLeft / 15f;
            }
            else
            {
                projectile.Opacity = 1;
            }
            

        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            int dir = Math.Sign(projectile.velocity.X);
            SpriteEffects SP = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 DrawPos1 = projectile.Center + new Vector2(20, 0) * dir + new Vector2(0, -20);
            spriteBatch.Draw(tex, DrawPos1 - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation, tex.Size() / 2, projectile.scale * 0.9f, SP, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!Main.player[projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                FuriosoUtils.DeepAddBuff(target, ModContent.BuffType<RangaBleeding>(), 600, true);

                Main.player[projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Ranga);
            }
            else
            {
                FuriosoUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}