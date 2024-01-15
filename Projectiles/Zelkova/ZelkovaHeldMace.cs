using Furioso.Buffs;
using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Zelkova
{
    public class ZelkovaHeldMace : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zelkova Workshop");
            //DisplayName.AddTranslation(7, "榉树工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 35;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                Projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<ZelkovaItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;
            Projectile.Center = owner.Center;
            if (Projectile.ai[0] == 1)
            {
                Projectile.rotation = dir > 0 ? MathHelper.Pi / 8 * 5 : MathHelper.Pi / 8 * 3;
            }
            else
            {
                Projectile.rotation = dir > 0 ? 0 : MathHelper.Pi;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Zelkova/ZelkovaHeldMace").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("Furioso/Projectiles/Zelkova/MaceSwing2").Value;
            if (Projectile.ai[0] == 0)
            {
                Vector2 OffSet = new Vector2(6, 6);
                if (dir > 0)
                {
                    Main.spriteBatch.Draw(tex, Projectile.Center + OffSet - Main.screenPosition, null, lightColor * Projectile.Opacity, 0, tex.Size(), Projectile.scale * 0.3f, SpriteEffects.None, 0);
                }
                else
                {
                    Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(-OffSet.X, OffSet.Y) - Main.screenPosition, null, lightColor * Projectile.Opacity, 0, new Vector2(0, tex.Height), Projectile.scale * 0.3f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else
            {
                float R1 = MathHelper.Pi / 4 * 3;
                float R2 = dir > 0 ? MathHelper.Pi / 8 * 5 : MathHelper.Pi / 8 * 3;
                R1 += R2;
                Main.spriteBatch.Draw(tex, Projectile.Center - R2.ToRotationVector2() * 6 - Main.screenPosition, null, lightColor * Projectile.Opacity, R1, tex.Size(), Projectile.scale * 0.3f, SpriteEffects.None, 0);
                if (dir > 0)
                {
                    Main.spriteBatch.Draw(tex2, Projectile.Center + new Vector2(-5, 95) - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, tex2.Size() / 2, Projectile.scale * 0.25f, SpriteEffects.None, 0);
                }
                else
                {
                    Main.spriteBatch.Draw(tex2, Projectile.Center + new Vector2(5, 95) - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, tex2.Size() / 2, Projectile.scale * 0.25f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        /*public override bool CanDamage()
        {
            return Projectile.ai[0] == 1;
        }*/

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[0] == 1)       //160
            {
                float point = 0f;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 155 * Projectile.scale, 20 * Projectile.scale, ref point);
            }
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
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