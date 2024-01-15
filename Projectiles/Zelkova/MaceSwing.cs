using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Zelkova
{
    public class MaceSwing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zelkova Workshop");
            //DisplayName.AddTranslation(7, "榉树工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 400;
            Projectile.timeLeft = 40;
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

            Projectile.Center = owner.Center + Projectile.velocity;
            if (Projectile.timeLeft > 30)
            {
                Projectile.Opacity = 1;
            }
            else
            {
                Projectile.Opacity = Projectile.timeLeft / 30f;
            }
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Zelkova/MaceSwing").Value;
            int dir = Math.Sign(Projectile.velocity.X);
            SpriteEffects SP = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.85f, SP, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            Player owner = Main.player[Projectile.owner];
            if (Math.Sign(target.Center.X - owner.Center.X + 0.01f) == Math.Sign(owner.direction + 0.01f))
            {
                return null;
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ModifyHitInfo += (ref NPC.HitInfo info) =>
            {
                info.Damage += (int)(target.damage * DamageValue.ZelkovaMeleeModifier);
            };
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Zelkova);
            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}