using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Ranga
{
    public class RangaSlash1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ranga WorkShop");
            //DisplayName.AddTranslation(7, "琅琊工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 321;
            Projectile.height = 100;
            Projectile.timeLeft = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
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
            Projectile.Center = owner.Center;
            Projectile.velocity = new Vector2(owner.direction, 0);
            if (Projectile.timeLeft >= 25)
            {
                Projectile.Opacity = (30 - Projectile.timeLeft) / 5f;
            }
            else if (Projectile.timeLeft < 15)
            {
                Projectile.Opacity = Projectile.timeLeft / 15f;
            }
            else
            {
                Projectile.Opacity = 1;
            }


        }


        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Ranga/RangaSlash1").Value;
            int dir = Math.Sign(Projectile.velocity.X);
            SpriteEffects SP = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 DrawPos1 = Projectile.Center + new Vector2(20, 0) * dir + new Vector2(0, -20);
            Main.spriteBatch.Draw(tex, DrawPos1 - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.9f, SP, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<RangaBleeding>(), 600, true);

                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Ranga);
            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}