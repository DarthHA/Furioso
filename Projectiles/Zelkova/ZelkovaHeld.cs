using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Zelkova
{
    public class ZelkovaHeld : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zelkova Workshop");
            DisplayName.AddTranslation(GameCulture.Chinese, "榉树工坊");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }

        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 999;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<ZelkovaItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.direction = dir;
            projectile.Center = owner.Center;
            owner.SetItemTime(5);

            projectile.localAI[0]++;
            if (projectile.localAI[0] == 1)
            {
                Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<ZelkovaHeldAxe>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.localAI[1]);
                Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<ZelkovaHeldMace>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.localAI[1]);
                if (projectile.localAI[1] == 0)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/ZelkovaAxe"), owner.Center);
                    Vector2 RelaPos = new Vector2(dir, 1) * -30 + new Vector2(0, -55);
                    Projectile.NewProjectile(projectile.Center, RelaPos, ModContent.ProjectileType<AxeSwing>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
                else
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/ZelkovaMace"), owner.Center);
                    Vector2 RelaPos = new Vector2(dir, 1) * 55 + new Vector2(0, -55);
                    Projectile.NewProjectile(projectile.Center, RelaPos, ModContent.ProjectileType<MaceSwing>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
            }
            if (projectile.localAI[0] >= 45)
            {
                projectile.localAI[0] = 0;
                owner.direction = Math.Sign(Main.MouseWorld.X - owner.Center.X + 0.01f);
                projectile.velocity = new Vector2(owner.direction, 0);
                if (++projectile.localAI[1] >= 2)
                {
                    projectile.Kill();
                    return;
                }
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