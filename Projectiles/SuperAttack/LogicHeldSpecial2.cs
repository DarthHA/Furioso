using Furioso.Items;
using Furioso.Projectiles.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class LogicHeldSpecial2 : ModProjectile
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

            projectile.rotation = projectile.velocity.ToRotation();
            projectile.Center = owner.Center;
            owner.heldProj = projectile.whoAmI;
            owner.direction = Math.Sign(projectile.velocity.X);
            owner.SetItemTime(5);
            int dir = owner.direction;

            projectile.ai[1]++;

            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
            if (projectile.ai[1] == 1)
            {
                int protmp = Projectile.NewProjectile(owner.Center, projectile.velocity, ModContent.ProjectileType<LogicHeld3>(), 0, 0, owner.whoAmI);
                Main.projectile[protmp].timeLeft = 50;
                float rot = projectile.velocity.ToRotation();
                Projectile.NewProjectile(owner.Center + new Vector2(0, -5) + rot.ToRotationVector2() * 150, rot.ToRotationVector2(), ModContent.ProjectileType<LogicGunExplosion2>(), projectile.damage * 2, projectile.knockBack, owner.whoAmI);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/LogicAtk2"), owner.Center);
            }
            if (projectile.ai[1] > 50)
            {
                projectile.Kill();
                return;
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