using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalHeldSpecial1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Furioso");
            DisplayName.AddTranslation(GameCulture.Chinese, "Furioso");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 100;
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
            if (!owner.IsHoldingItemOrFurioso())
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
                Vector2 Pos = owner.Center + projectile.rotation.ToRotationVector2() * 85;
                int protmp = Projectile.NewProjectile(Pos + new Vector2(0, 40), projectile.velocity, ModContent.ProjectileType<DurandalSwingSpecial>(), 0, 0, owner.whoAmI);
                (Main.projectile[protmp].modProjectile as DurandalSwingSpecial).RelaPos = Main.projectile[protmp].Center - owner.Center;
            }
            if (projectile.ai[0] == 0)
            {
                projectile.rotation = dir >= 0 ? MathHelper.Pi / 8 * 7 : MathHelper.Pi / 8;
            }
            owner.heldProj = projectile.whoAmI;
            owner.direction = dir;
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