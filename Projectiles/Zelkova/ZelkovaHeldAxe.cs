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
    public class ZelkovaHeldAxe : ModProjectile
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
            owner.heldProj = Projectile.whoAmI;
            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = dir > 0 ? MathHelper.Pi / 6 : MathHelper.Pi / 6 * 5;
            }
            else
            {
                Projectile.rotation = dir > 0 ? 0 : MathHelper.Pi;
            }
            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Zelkova/ZelkovaHeldAxe").Value;
            Texture2D tex2 = ModContent.Request<Texture2D>("Furioso/Projectiles/Zelkova/AxeSwing2").Value;
            if (Projectile.ai[0] == 0)
            {
                Vector2 OffSet = new Vector2(-12, -6);
                if (dir > 0)
                {
                    Main.spriteBatch.Draw(tex, Projectile.Center + OffSet - Main.screenPosition, null, lightColor * Projectile.Opacity, 0, Vector2.Zero, Projectile.scale * 0.3f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(tex2, Projectile.Center + new Vector2(67, -22) - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, tex2.Size() / 2, Projectile.scale * 0.25f, SpriteEffects.None, 0);
                }
                else
                {
                    Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(-OffSet.X, OffSet.Y) - Main.screenPosition, null, lightColor * Projectile.Opacity, 0, new Vector2(tex.Width, 0), Projectile.scale * 0.3f, SpriteEffects.FlipHorizontally, 0);
                    Main.spriteBatch.Draw(tex2, Projectile.Center + new Vector2(-67, -22) - Main.screenPosition, null, Color.White * Projectile.Opacity, 0, tex2.Size() / 2, Projectile.scale * 0.25f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else
            {
                if (dir > 0)
                {
                    //spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, MathHelper.Pi / 4, new Vector2(tex.Width, 0), projectile.scale * 0.3f, SpriteEffects.FlipHorizontally, 0);
                    Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(0, 6) - Main.screenPosition, null, lightColor * Projectile.Opacity, MathHelper.Pi, Vector2.Zero, Projectile.scale * 0.3f, SpriteEffects.None, 0);
                }
                else
                {
                    //spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, -MathHelper.Pi / 4, Vector2.Zero, projectile.scale * 0.3f, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(tex, Projectile.Center + new Vector2(0, 6) - Main.screenPosition, null, lightColor * Projectile.Opacity, MathHelper.Pi, new Vector2(tex.Width, 0), Projectile.scale * 0.3f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<AxeSwing>() && proj.owner == Projectile.owner)
                {
                    (proj.ModProjectile as AxeSwing).DrawAlt(Main.spriteBatch);
                }
            }

            return false;
        }

        /*public override bool CanDamage()
        {
            return projectile.ai[0] == 0;
        }*/

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[0] == 0)       //123
            {
                float point = 0f;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 110 * Projectile.scale, 20 * Projectile.scale, ref point);
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