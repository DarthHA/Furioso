using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Logic
{
    public class LogicHeld2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ateller Logic");
            DisplayName.AddTranslation(GameCulture.Chinese, "逻辑工作室");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 30;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.hide = true;
        }
        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<LogicItem>()))
            {
                projectile.Kill();
                return;
            }

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.Center = owner.Center;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = mod.GetTexture("Projectiles/Logic/LogicGun1");
            float rot = projectile.rotation;
            if (projectile.velocity.X < 0)
            {
                rot = MathHelper.Pi + projectile.rotation;
            }
            SpriteEffects SP = projectile.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(tex, projectile.Center + projectile.rotation.ToRotationVector2() * 30 - Main.screenPosition, null, lightColor, rot, tex.Size() / 2, projectile.scale * 0.65f, SP, 0);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}