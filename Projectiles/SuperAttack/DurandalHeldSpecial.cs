using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalHeldSpecial : ModProjectile
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
            if (!owner.IsHoldingItemOrFurioso())
            {
                projectile.Kill();
                return;
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
                Projectile.NewProjectile(owner.Center, projectile.velocity, ModContent.ProjectileType<DurandalHeldSpecial2>(), 0, 0, projectile.owner);
                
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/DurandalFinalAtk"), owner.Center);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlashSpecial2>(), projectile.damage, projectile.knockBack, owner.whoAmI, dir);
                Projectile.NewProjectile(owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlashSpecial1>(), projectile.damage, projectile.knockBack, owner.whoAmI, dir);
                Projectile.NewProjectile(owner.Center, projectile.velocity, ModContent.ProjectileType<DurandalHeldSpecial1>(), 0, 0, owner.whoAmI, 0);
                float dist = owner.Distance(Main.MouseWorld) > 140 ? 140 : owner.Distance(Main.MouseWorld);
                Vector2 Pos = owner.Center + Vector2.Normalize(Main.MouseWorld - owner.Center) * dist;
                int protmp = Projectile.NewProjectile(Pos, Vector2.Zero, ModContent.ProjectileType<DurandalSlashSpecial3>(), projectile.damage, 0, projectile.owner);
                Main.projectile[protmp].rotation = MathHelper.Pi / 2 + (Main.rand.NextFloat() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
            }
            if (projectile.ai[0] >= 80)
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