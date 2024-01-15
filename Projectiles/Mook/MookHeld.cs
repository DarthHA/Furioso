using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Mook
{
    public class MookHeld : ModProjectile
    {
        Vector2 ShootPos = Vector2.Zero;
        float ops2 = 0;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mook Workshop");
            //DisplayName.AddTranslation(7, "墨工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 99999;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<MookItem>()))
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[0] == 0)
            {
                ShootPos = Vector2.Normalize(Main.MouseWorld - owner.Center) * 250;
            }

            Projectile.ai[0]++;
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;
            Projectile.Center = owner.Center + new Vector2(-25 * dir, 15);
            owner.heldProj = Projectile.whoAmI;
            owner.direction = dir;
            SomeUtils.SetItemTime(owner, 5);
            float r = MathHelper.Pi / 2 - dir * MathHelper.Pi / 6;
            owner.itemRotation = (float)Math.Atan2(r.ToRotationVector2().Y * dir, r.ToRotationVector2().X * dir);
            if (Projectile.ai[0] == 5)
            {
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/MookStart"));
            }
            if (Projectile.ai[0] == 10)
            {
                Vector2 SlashPos = owner.Center + ShootPos;
                int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), SlashPos, Vector2.Zero, ModContent.ProjectileType<MookSlash1>(), Projectile.damage / 120, 0, owner.whoAmI);
                Main.projectile[protmp].rotation = ShootPos.ToRotation();
                Main.projectile[protmp].scale = 2.5f;
            }
            if (Projectile.ai[0] == 20)
            {
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/MookAtk"));
            }
            if (Projectile.ai[0] == 50)
            {
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/MookEnd"));
            }
            if (Projectile.ai[0] == 60)
            {
                Vector2 SlashPos = owner.Center + ShootPos;
                int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), SlashPos, Vector2.Zero, ModContent.ProjectileType<MookSlash1>(), Projectile.damage / 120, 0, owner.whoAmI);
                Main.projectile[protmp].rotation = (-ShootPos).ToRotation();
                Main.projectile[protmp].scale = 1.5f;
            }
            if (Projectile.ai[0] > 61 && Projectile.ai[0] <= 69)
            {
                ops2 = (69 - Projectile.ai[0]) / 8;
            }
            if (Projectile.ai[0] > 15 && Projectile.ai[0] < 55)
            {
                if (Projectile.ai[0] % 3 == 2)
                {
                    Vector2 SlashPos = owner.Center + ShootPos + (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2() * Main.rand.Next(40);
                    int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), SlashPos, Vector2.Zero, ModContent.ProjectileType<MookSlash2>(), Projectile.damage / 160, 0, owner.whoAmI);
                    Main.projectile[protmp].rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Main.projectile[protmp].scale = 0.6f + Main.rand.NextFloat() * 0.8f;
                    Main.projectile[protmp].ai[0] = Main.rand.Next(2) * 2 - 1;
                    Main.projectile[protmp].ai[1] = (float)Math.Pow(MathHelper.Lerp(0.1f, 1f, Main.rand.NextFloat()), 2);
                }
                if (Projectile.ai[0] % 10 == 2)
                {
                    Vector2 SlashPos = owner.Center + ShootPos;
                    int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), SlashPos, Vector2.Zero, ModContent.ProjectileType<MookSlash1>(), Projectile.damage / 160, 0, owner.whoAmI);
                    Main.projectile[protmp].rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
                    Main.projectile[protmp].scale = MathHelper.Lerp(0.2f, 1.8f, Main.rand.NextFloat());
                }
            }

            if (Projectile.ai[0] > 75)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Player owner = Main.player[Projectile.owner];
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Mook/MookHeld").Value;
            if ((Projectile.ai[0] > 5 && Projectile.ai[0] < 15) || (Projectile.ai[0] > 50 && Projectile.ai[0] < 60))
            {
                tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Mook/MookHeld2").Value;
            }
            SpriteEffects SP = owner.direction >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, 0, tex.Size() / 2, Projectile.scale * 0.65f, SP, 0);
            if (ops2 > 0)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Texture2D tex2 = ModContent.Request<Texture2D>("Furioso/Projectiles/Mook/MookExplosion").Value;
                float k = 1 - ops2;
                Main.spriteBatch.Draw(tex2, owner.Center + ShootPos - Main.screenPosition, null, Color.White * ops2 * 0.4f, 0, tex2.Size() / 2, Projectile.scale * (k * 2f + 0.5f), SP, 0);
            }
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