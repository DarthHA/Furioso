using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Durandal
{
    public class DurandalHeld1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Durandal");
            DisplayName.AddTranslation(GameCulture.Chinese, "杜兰达尔");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 45;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.hide = true;
            projectile.alpha = 255;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }
        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<DurandalItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.direction = dir;
            projectile.Center = owner.Center;
            projectile.ai[1]++;
            if (projectile.ai[1] > 8)
            {
                projectile.Opacity = 1;
            }
            if (projectile.ai[1] == 5)
            {
                int protmp;
                if (projectile.ai[0] == 0)
                {
                    Vector2 Pos = owner.Center + projectile.rotation.ToRotationVector2() * 85;
                    protmp = Projectile.NewProjectile(Pos + new Vector2(0, 40), projectile.velocity, ModContent.ProjectileType<DurandalSwing1>(), 0, 0, owner.whoAmI);
                    (Main.projectile[protmp].modProjectile as DurandalSwing1).RelaPos = Main.projectile[protmp].Center - owner.Center;
                }
                else
                {
                    Vector2 Pos = owner.Center + projectile.rotation.ToRotationVector2() * 105;
                    protmp = Projectile.NewProjectile(Pos + new Vector2(0, -28), projectile.velocity, ModContent.ProjectileType<DurandalSwing2>(), 0, 0, owner.whoAmI);
                    (Main.projectile[protmp].modProjectile as DurandalSwing2).RelaPos = Main.projectile[protmp].Center - owner.Center;
                }
               
            }
            if (projectile.ai[0] == 0)
            {
                if (dir > 0)
                {
                    projectile.rotation = MathHelper.Pi / 8 * 7;
                }
                else
                {
                    projectile.rotation = MathHelper.Pi / 8;
                }
            }
            else
            {
                if (dir > 0)
                {
                    projectile.rotation = -MathHelper.Pi / 8 * 7;
                }
                else
                {
                    projectile.rotation = -MathHelper.Pi / 8;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, projectile.Center + projectile.rotation.ToRotationVector2() * 60 - Main.screenPosition, null, lightColor * projectile.Opacity, projectile.rotation, tex.Size() / 2, projectile.scale * 0.75f, SpriteEffects.None, 0);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}