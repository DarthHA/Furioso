using Furioso.Items;
using Furioso.Projectiles.Allas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class AllasHeldSpecial : ModProjectile
    {
        public float vel = 0;
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
            projectile.damage = 90;
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
            
            owner.direction = dir;
            owner.SetItemTime(5);
            projectile.rotation = projectile.velocity.ToRotation();
            owner.itemRotation = (float)Math.Atan2(projectile.rotation.ToRotationVector2().Y * dir, projectile.rotation.ToRotationVector2().X * dir);
            owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = projectile.type;    

            projectile.localAI[0]++;
            if (projectile.localAI[0] == 1)
            {
                Projectile.NewProjectile(owner.Center, projectile.velocity, ModContent.ProjectileType<AllasHeld1>(), projectile.damage, projectile.knockBack, projectile.owner);
            }

            if (projectile.localAI[0] == 4)
            {
                vel = 45;
            }
            if (vel > 1)
            {
                owner.GetModPlayer<RolandPlayer>().Ram = true;
                owner.velocity = projectile.rotation.ToRotationVector2() * vel;
                vel *= 0.75f;
            }
            else
            {
                owner.GetModPlayer<RolandPlayer>().Ram = false;
                if (projectile.ai[1] > 0)
                {
                    FuriosoUtils.GivePlayerImmune(owner, 60);
                }
            }
            projectile.Center = owner.Center;

            if (projectile.localAI[0] >= 55)
            {
                owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = -1;
                vel = 0;
                FuriosoUtils.GivePlayerImmune(owner, 60);
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


        public override void CutTiles()
        {
            Player owner = Main.player[projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center - projectile.rotation.ToRotationVector2() * 125, projectile.Center + projectile.rotation.ToRotationVector2() * 125, (20 + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
        
    }

   
}