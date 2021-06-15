using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Allas
{
    public class AllasHeld1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Allas WorkShop");
            DisplayName.AddTranslation(GameCulture.Chinese, "阿拉斯工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 200;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<AllasItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.direction = dir;
            
            owner.heldProj = projectile.whoAmI;
            projectile.rotation = projectile.velocity.ToRotation();

            projectile.localAI[0]++;
            float k = 100;
            if (projectile.localAI[0] == 1)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/AllasAtk"), owner.Center);
            }
            if (projectile.localAI[0] < 5)
            {
                k = -50f + 150f * projectile.localAI[0] / 5;
            }
            if (projectile.localAI[0] == 4)
            {
                //Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity) * 45, ModContent.ProjectileType<AllasPen>(), projectile.damage / 3, projectile.knockBack / 3, owner.whoAmI);
                Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity) * 60, ModContent.ProjectileType<AllasPen2>(), projectile.damage, projectile.knockBack, owner.whoAmI);
            }

            projectile.Center = owner.Center + projectile.rotation.ToRotationVector2() * k;

            if (projectile.localAI[0] >= 55)
            {
                projectile.Kill();
                return;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Texture2D tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, lightColor * projectile.Opacity, projectile.rotation + MathHelper.Pi / 4, tex.Size() / 2, projectile.scale * 0.8f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<AllasPen2>() && proj.owner == projectile.owner)
                {
                    (proj.modProjectile as AllasPen2).DrawAlt(spriteBatch);
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
            Player owner = Main.player[projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center - projectile.rotation.ToRotationVector2() * 125, projectile.Center + projectile.rotation.ToRotationVector2() * 125, (20 + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
        
    }

   
}