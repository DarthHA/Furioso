using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalHeldSpecial1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Furioso");
            //DisplayName.AddTranslation(7, "Furioso");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                Projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso())
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;
            Projectile.Center = owner.Center;
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 8)
            {
                Projectile.Opacity = 1;
            }
            if (Projectile.ai[1] == 5)
            {
                Vector2 Pos = owner.Center + Projectile.rotation.ToRotationVector2() * 85;
                int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Pos + new Vector2(0, 40), Projectile.velocity, ModContent.ProjectileType<DurandalSwingSpecial>(), 0, 0, owner.whoAmI);
                (Main.projectile[protmp].ModProjectile as DurandalSwingSpecial).RelaPos = Main.projectile[protmp].Center - owner.Center;
            }
            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation = dir >= 0 ? MathHelper.Pi / 8 * 7 : MathHelper.Pi / 8;
            }
            owner.heldProj = Projectile.whoAmI;
            owner.direction = dir;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/SuperAttack/DurandalHeldSpecial1").Value;
            Main.spriteBatch.Draw(tex, Projectile.Center + Projectile.rotation.ToRotationVector2() * 60 - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.None, 0);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}