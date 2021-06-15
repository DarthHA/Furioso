using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Crystal
{
    public class CrystalHeld : ModProjectile
    {
        public float vel = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Atelier");
            DisplayName.AddTranslation(GameCulture.Chinese, "卡莉斯塔工作室");
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<CrystalItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.Center = owner.Center;
            projectile.direction = dir;
            projectile.rotation = dir > 0 ? 0 : MathHelper.Pi;
            owner.direction = dir;
            owner.SetItemTime(5);
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
            owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = projectile.type;
            projectile.localAI[0]++;
            if (projectile.localAI[0] == 1)
            {
                vel = 200;
                owner.GetModPlayer<RolandPlayer>().Ram = true;

                Projectile.NewProjectile(projectile.Center, new Vector2(dir, 0), ModContent.ProjectileType<CrystalHeld1>(), projectile.damage, projectile.knockBack, projectile.owner);
                Projectile.NewProjectile(projectile.Center, new Vector2(dir, 0), ModContent.ProjectileType<CrystalHeld2>(), projectile.damage, projectile.knockBack, projectile.owner);
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<CrystalSlash1>(), projectile.damage, projectile.knockBack, projectile.owner, dir);
                int protmp = Projectile.NewProjectile(projectile.Center, new Vector2(dir * 9f, 24f), ModContent.ProjectileType<CrystalSlash1>(), projectile.damage, projectile.knockBack, projectile.owner, dir);
                Main.projectile[protmp].rotation = -dir * 0.2f;
                //Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<CrystalSlash12>(), projectile.damage, projectile.knockBack, projectile.owner, dir);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Crystal"), owner.Center);
            }
            if (projectile.localAI[0] == 4 && projectile.localAI[1] == 1)
            {
                Vector2 ForePos = projectile.Center;// + new Vector2(400, 0) * dir;
                Projectile.NewProjectile(ForePos, (MathHelper.Pi / 6).ToRotationVector2(), ModContent.ProjectileType<CrystalSlash2>(), projectile.damage, 0, projectile.owner, dir);
                Projectile.NewProjectile(ForePos, (-MathHelper.Pi / 6).ToRotationVector2(), ModContent.ProjectileType<CrystalSlash2>(), projectile.damage, 0, projectile.owner, dir);
            }

            if (projectile.localAI[0] < 30)
            {
                vel *= 0.75f;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
            }
            if (projectile.localAI[0] >= 30 && projectile.localAI[0] < 50)
            {
                vel = 0;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
            }
            owner.velocity = new Vector2(owner.direction, 0) * vel;
            if (projectile.localAI[0] >= 50)
            {
                projectile.localAI[0] = 0;
                if (++projectile.localAI[1] >= 2)
                {
                    owner.GetModPlayer<RolandPlayer>().Ram = false;
                    owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = -1;
                    FuriosoUtils.GivePlayerImmune(owner, 30);
                    projectile.Kill();
                    return;
                }
                else
                {
                    owner.GetModPlayer<RolandPlayer>().CrystalDodge = false;
                    projectile.velocity = new Vector2(Math.Sign(projectile.ai[0] - owner.Center.X + 0.01f), 0);
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