using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace Furioso.Projectiles.OldBoys
{
    public class OldBoysHeld : ModProjectile
    {

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
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                Projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<OldBoysItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            owner.direction = dir;
            owner.heldProj = Projectile.whoAmI;
            SomeUtils.SetItemTime(owner, 5);
            Projectile.direction = dir;
            Projectile.Center = owner.Center;
            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);

            if (Projectile.ai[1] == 0)
            {
                float r = Projectile.rotation;
                if (dir > 0)
                {
                    r -= MathHelper.Pi / 2;
                }
                else
                {
                    r += MathHelper.Pi / 2;
                }
                int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center + r.ToRotationVector2() * 15, Projectile.velocity, ModContent.ProjectileType<OldBoysSwing>(), Projectile.damage, Projectile.knockBack, owner.whoAmI);
                (Main.projectile[protmp].ModProjectile as OldBoysSwing).RelaPos = r.ToRotationVector2() * 15;
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/OldBoysSwing"));
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 6)
            {
                Projectile.Opacity = 1;
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/OldBoys/OldBoysHeld").Value;
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
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor * Projectile.Opacity, r, tex.Size() / 2, Projectile.scale * 0.75f, SP, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }



        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}