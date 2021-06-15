using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Durandal
{
    public class DurandalHeld : ModProjectile
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<DurandalItem>()))
            {
                projectile.Kill();
                return;
            }
            if (projectile.ai[0] == 45)
            {
                projectile.velocity = new Vector2(Math.Sign(Main.MouseWorld.X - owner.Center.X), 0);
            }

            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.direction = dir;
            projectile.Center = owner.Center;
            owner.direction = dir;
            owner.SetItemTime(5);
            projectile.rotation = dir >= 0 ? MathHelper.Pi / 4 * 3 : MathHelper.Pi / 4;
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);

            projectile.ai[0]++;
            if (projectile.ai[0] == 1)
            {
                Projectile.NewProjectile(owner.Center, projectile.velocity, ModContent.ProjectileType<DurandalHeld1>(), 0, 0, owner.whoAmI, 0);
                Projectile.NewProjectile(owner.Center, projectile.velocity, ModContent.ProjectileType<DurandalHeld2>(), 0, 0, projectile.owner);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/DurandalAtk1"), owner.Center);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlash12>(), projectile.damage, projectile.knockBack, owner.whoAmI, dir);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlash1>(), projectile.damage, projectile.knockBack, owner.whoAmI, dir);
                float dist = owner.Distance(Main.MouseWorld) > 140 ? 140 : owner.Distance(Main.MouseWorld);
                Vector2 Pos = owner.Center + Vector2.Normalize(Main.MouseWorld - owner.Center) * dist;
                int protmp = Projectile.NewProjectile(Pos, Vector2.Zero, ModContent.ProjectileType<DurandalSlash3>(), projectile.damage, 0, projectile.owner);
                Main.projectile[protmp].rotation = MathHelper.Pi / 2 + (Main.rand.NextFloat() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
            }
            if (projectile.ai[0] == 46)
            {
                Projectile.NewProjectile(owner.Center, projectile.velocity, ModContent.ProjectileType<DurandalHeld1>(), 0, 0, owner.whoAmI, 1);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/DurandalAtk2"), owner.Center);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlash22>(), projectile.damage, projectile.knockBack, owner.whoAmI, dir);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlash2>(), projectile.damage, projectile.knockBack, owner.whoAmI, dir);
                float dist = owner.Distance(Main.MouseWorld) > 140 ? 140 : owner.Distance(Main.MouseWorld);
                Vector2 Pos = owner.Center + Vector2.Normalize(Main.MouseWorld - owner.Center) * dist;
                int protmp = Projectile.NewProjectile(Pos, Vector2.Zero, ModContent.ProjectileType<DurandalSlash3>(), projectile.damage, 0, projectile.owner);
                Main.projectile[protmp].rotation = -MathHelper.Pi / 2 + (Main.rand.NextFloat() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
            }
            if (projectile.ai[0] >= 95)
            {
                projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}