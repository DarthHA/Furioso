using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Logic
{
    public class LogicHeld : ModProjectile
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<LogicItem>()))
            {
                projectile.Kill();
                return;
            }

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.Center = owner.Center;
            owner.heldProj = projectile.whoAmI;
            owner.direction = Math.Sign(projectile.velocity.X);
            owner.SetItemTime(5);


            if (projectile.ai[1] == 0)
            {
                float rot = projectile.velocity.ToRotation();
                
                Projectile.NewProjectile(owner.Center, rot.ToRotationVector2(), ModContent.ProjectileType<LogicHeld2>(), 0, 0, owner.whoAmI);
                if (projectile.velocity.X < 0)
                {
                    Projectile.NewProjectile(owner.Center, (MathHelper.Pi / 4).ToRotationVector2(), ModContent.ProjectileType<LogicHeld1>(), 0, 0, owner.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(owner.Center, (MathHelper.Pi / 4 * 3).ToRotationVector2(), ModContent.ProjectileType<LogicHeld1>(), 0, 0, owner.whoAmI);
                }

                Projectile.NewProjectile(owner.Center + new Vector2(0, -5) + rot.ToRotationVector2() * 80, rot.ToRotationVector2(), ModContent.ProjectileType<LogicGunExplosion1>(), projectile.damage, projectile.knockBack, owner.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/LogicAtk1"), owner.Center);
            }
            if (projectile.ai[1] <= 30)
            {
                float r = MathHelper.Pi / 2;
                int dir = owner.direction;
                owner.itemRotation = (float)Math.Atan2(r.ToRotationVector2().Y * dir, r.ToRotationVector2().X * dir);
            }
            else
            {
                int dir = owner.direction;
                owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
            }
            if (projectile.ai[1] == 31)
            {
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - owner.Center) * 10;
                float rot = projectile.velocity.ToRotation();
                Projectile.NewProjectile(owner.Center, rot.ToRotationVector2(), ModContent.ProjectileType<LogicHeld1>(), projectile.damage, projectile.knockBack, owner.whoAmI);
                if (projectile.velocity.X < 0)
                {
                    Projectile.NewProjectile(owner.Center, (MathHelper.Pi / 8 * 3).ToRotationVector2(), ModContent.ProjectileType<LogicHeld2>(),0, 0, owner.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(owner.Center, (MathHelper.Pi / 8 * 5).ToRotationVector2(), ModContent.ProjectileType<LogicHeld2>(), 0, 0, owner.whoAmI);
                }

                Projectile.NewProjectile(owner.Center + new Vector2(0, -5) + rot.ToRotationVector2() * 80, rot.ToRotationVector2(), ModContent.ProjectileType<LogicGunExplosion1>(), projectile.damage, projectile.knockBack, owner.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/LogicAtk1"), owner.Center);
            }
            if (projectile.ai[1] == 62)
            {
                projectile.velocity = Vector2.Normalize(Main.MouseWorld - owner.Center) * 10;
                Projectile.NewProjectile(owner.Center, projectile.velocity, ModContent.ProjectileType<LogicHeld3>(), 0, 0, owner.whoAmI);
                float rot = projectile.velocity.ToRotation();
                Projectile.NewProjectile(owner.Center + new Vector2(0, -5) + rot.ToRotationVector2() * 150, rot.ToRotationVector2(), ModContent.ProjectileType<LogicGunExplosion2>(), (int)(projectile.damage * 1.5f), projectile.knockBack * 2f, owner.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/LogicAtk2"), owner.Center);
            }
            if (projectile.ai[1] > 140)
            {
                projectile.Kill();
                return;
            }
            projectile.ai[1]++;
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