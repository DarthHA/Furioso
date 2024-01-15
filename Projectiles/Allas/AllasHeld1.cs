using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Allas
{
    public class AllasHeld1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Allas WorkShop");
            //DisplayName.AddTranslation(7, "阿拉斯工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                Projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<AllasItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;

            owner.heldProj = Projectile.whoAmI;
            Projectile.rotation = Projectile.velocity.ToRotation();

            Projectile.localAI[0]++;
            float k = 100;
            if (Projectile.localAI[0] == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/AllasAtk"));
            }
            if (Projectile.localAI[0] < 5)
            {
                k = -50f + 150f * Projectile.localAI[0] / 5;
            }
            if (Projectile.localAI[0] == 4)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Normalize(Projectile.velocity) * 60, ModContent.ProjectileType<AllasPen2>(), Projectile.damage, Projectile.knockBack, owner.whoAmI);
            }

            Projectile.Center = owner.Center + Projectile.rotation.ToRotationVector2() * k;

            if (Projectile.localAI[0] >= 55)
            {
                Projectile.Kill();
                return;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Allas/AllasHeld1").Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation + MathHelper.Pi / 4, tex.Size() / 2, Projectile.scale * 0.8f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<AllasPen2>() && proj.owner == Projectile.owner)
                {
                    (proj.ModProjectile as AllasPen2).DrawAlt(Main.spriteBatch);
                }
            }
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            Player owner = Main.player[Projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Terraria.Utils.PlotTileLine(Projectile.Center - Projectile.rotation.ToRotationVector2() * 125, Projectile.Center + Projectile.rotation.ToRotationVector2() * 125, (20 + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }

    }


}