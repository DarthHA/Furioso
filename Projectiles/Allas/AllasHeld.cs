using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Allas
{
    public class AllasHeld : ModProjectile
    {
        public float vel = 0;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Allas WorkShop");
            //DisplayName.AddTranslation(7, "阿拉斯工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.ghost)
            {
                Projectile.Kill();
                return;
            }
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<AllasItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;

            owner.direction = dir;
            SomeUtils.SetItemTime(owner, 5);
            Projectile.rotation = Projectile.velocity.ToRotation();
            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);
            owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = Projectile.type;

            Projectile.localAI[0]++;
            if (Projectile.localAI[0] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Projectile.velocity, ModContent.ProjectileType<AllasHeld1>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

            if (Projectile.localAI[0] == 4)
            {
                vel = 45;
            }
            if (vel > 1)
            {
                owner.GetModPlayer<RolandPlayer>().Ram = true;
                owner.velocity = Projectile.rotation.ToRotationVector2() * vel;
                vel *= 0.75f;
            }
            else
            {
                owner.GetModPlayer<RolandPlayer>().Ram = false;
                if (Projectile.ai[1] > 0)
                {
                    SomeUtils.GivePlayerImmune(owner, 30);
                }
            }
            Projectile.Center = owner.Center;

            if (Projectile.localAI[0] >= 55)
            {
                vel = 45;
                Projectile.ai[1]++;
                Projectile.localAI[0] = 0;
                Projectile.velocity = Vector2.Normalize(Main.MouseWorld - owner.Center);
                if (Projectile.ai[1] > 1)
                {
                    owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = -1;
                    vel = 0;
                    SomeUtils.GivePlayerImmune(owner, 30);
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            Player owner = Main.player[Projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Terraria.Utils.PlotTileLine(Projectile.Center - Projectile.rotation.ToRotationVector2() * 125, Projectile.Center + Projectile.rotation.ToRotationVector2() * 125, (20 + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }

    }


}