using Furioso.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Ranga
{
    public class RangaHeld : ModProjectile
    {
        public float vel = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ranga WorkShop");
            DisplayName.AddTranslation(GameCulture.Chinese, "琅琊工坊");
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<RangaItem>()))
            {
                projectile.Kill();
                return;
            }
            int dir = Math.Sign(projectile.velocity.X + 0.01f);
            projectile.Center = owner.Center;
            projectile.direction = dir;
            projectile.rotation = dir > 0 ? 0 : MathHelper.Pi;
            owner.heldProj = projectile.whoAmI;
            owner.direction = dir;
            owner.SetItemTime(5);
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
            owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = projectile.type;
            projectile.localAI[0]++;
            if (projectile.localAI[0] == 1)
            {
                vel = 125;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
                if (projectile.localAI[1] < 2)
                {
                    Projectile.NewProjectile(owner.Center, new Vector2(dir, 0), ModContent.ProjectileType<RangaHeld1>(), projectile.damage, 0, owner.whoAmI);
                    Projectile.NewProjectile(owner.Center, new Vector2(dir, 0), ModContent.ProjectileType<RangaSlash1>(), projectile.damage, 0, owner.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(owner.Center, new Vector2(dir, 0), ModContent.ProjectileType<RangaHeld2>(), projectile.damage, 0, owner.whoAmI);
                    int protmp = Projectile.NewProjectile(owner.Center + new Vector2(dir * 160, 20), new Vector2(dir, 0), ModContent.ProjectileType<RangaSlash2>(), projectile.damage, 0, owner.whoAmI);
                    Main.projectile[protmp].rotation = dir > 0 ? 0 : MathHelper.Pi;
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Ranga" + ((int)projectile.localAI[1] + 1).ToString()), owner.Center);
            }

            if (projectile.localAI[0] < 20)
            {
                vel *= 0.75f;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
            }
            if (projectile.localAI[0] >= 20 && projectile.localAI[0] < 30)
            {
                vel = 0;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
            }
            owner.velocity = new Vector2(owner.direction, 0) * vel;
            if (projectile.localAI[0] >= 30)
            {
                projectile.localAI[0] = 0;
                if (++projectile.localAI[1] >= 3)
                {
                    owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = -1;
                    owner.GetModPlayer<RolandPlayer>().Ram = false;
                    FuriosoUtils.GivePlayerImmune(owner, 30);
                    projectile.Kill();
                    return;
                }
                else
                {
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