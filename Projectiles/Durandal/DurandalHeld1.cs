using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Durandal
{
    public class DurandalHeld1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Durandal");
            //DisplayName.AddTranslation(7, "杜兰达尔");
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<DurandalItem>()))
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
                int protmp;
                if (Projectile.ai[0] == 0)
                {
                    Vector2 Pos = owner.Center + Projectile.rotation.ToRotationVector2() * 85;
                    protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Pos + new Vector2(0, 40), Projectile.velocity, ModContent.ProjectileType<DurandalSwing1>(), 0, 0, owner.whoAmI);
                    (Main.projectile[protmp].ModProjectile as DurandalSwing1).RelaPos = Main.projectile[protmp].Center - owner.Center;
                }
                else
                {
                    Vector2 Pos = owner.Center + Projectile.rotation.ToRotationVector2() * 105;
                    protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Pos + new Vector2(0, -28), Projectile.velocity, ModContent.ProjectileType<DurandalSwing2>(), 0, 0, owner.whoAmI);
                    (Main.projectile[protmp].ModProjectile as DurandalSwing2).RelaPos = Main.projectile[protmp].Center - owner.Center;
                }

            }
            if (Projectile.ai[0] == 0)
            {
                if (dir > 0)
                {
                    Projectile.rotation = MathHelper.Pi / 8 * 7;
                }
                else
                {
                    Projectile.rotation = MathHelper.Pi / 8;
                }
            }
            else
            {
                if (dir > 0)
                {
                    Projectile.rotation = -MathHelper.Pi / 8 * 7;
                }
                else
                {
                    Projectile.rotation = -MathHelper.Pi / 8;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Durandal/DurandalHeld1").Value;
            Main.spriteBatch.Draw(tex, Projectile.Center + Projectile.rotation.ToRotationVector2() * 60 - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.None, 0);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}