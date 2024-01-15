using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Durandal
{
    public class DurandalHeld : ModProjectile
    {
        public float vel = 0;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<DurandalItem>()))
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[0] == 45)
            {
                Projectile.velocity = new Vector2(Math.Sign(Main.MouseWorld.X - owner.Center.X), 0);
            }

            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;
            Projectile.Center = owner.Center;
            owner.direction = dir;
            SomeUtils.SetItemTime(owner, 5);
            Projectile.rotation = dir >= 0 ? MathHelper.Pi / 4 * 3 : MathHelper.Pi / 4;
            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);


            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Projectile.velocity, ModContent.ProjectileType<DurandalHeld1>(), 0, 0, owner.whoAmI, 0);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Projectile.velocity, ModContent.ProjectileType<DurandalHeld2>(), 0, 0, Projectile.owner);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlash12>(), Projectile.damage, Projectile.knockBack, owner.whoAmI, dir);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlash1>(), Projectile.damage, Projectile.knockBack, owner.whoAmI, dir);
                float dist = owner.Distance(Main.MouseWorld) > 140 ? 140 : owner.Distance(Main.MouseWorld);
                Vector2 Pos = owner.Center + Vector2.Normalize(Main.MouseWorld - owner.Center) * dist;
                int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Pos, Vector2.Zero, ModContent.ProjectileType<DurandalSlash3>(), Projectile.damage, 0, Projectile.owner);
                Main.projectile[protmp].rotation = MathHelper.Pi / 2 + (Main.rand.NextFloat() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/DurandalAtk1"));
            }

            if (Projectile.ai[0] == 46)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Projectile.velocity, ModContent.ProjectileType<DurandalHeld1>(), 0, 0, owner.whoAmI, 1);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlash22>(), Projectile.damage, Projectile.knockBack, owner.whoAmI, dir);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Vector2.Zero, ModContent.ProjectileType<DurandalSlash2>(), Projectile.damage, Projectile.knockBack, owner.whoAmI, dir);
                float dist = owner.Distance(Main.MouseWorld) > 140 ? 140 : owner.Distance(Main.MouseWorld);
                Vector2 Pos = owner.Center + Vector2.Normalize(Main.MouseWorld - owner.Center) * dist;
                int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Pos, Vector2.Zero, ModContent.ProjectileType<DurandalSlash3>(), Projectile.damage, 0, Projectile.owner);
                Main.projectile[protmp].rotation = -MathHelper.Pi / 2 + (Main.rand.NextFloat() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/DurandalAtk2"));
            }
            if (owner.GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = ModContent.ProjectileType<DurandalHeld>();
                owner.GetModPlayer<RolandPlayer>().Ram = true;
                if (Projectile.ai[0] < 35)
                {
                    vel = MathHelper.Lerp(15, 0, Projectile.ai[0] / 35f);
                }
                else if (Projectile.ai[0] < 45)
                {
                    vel = 0;
                }
                else if (Projectile.ai[0] < 80)
                {
                    vel = MathHelper.Lerp(15, 0, (Projectile.ai[0] - 45) / 35f);
                }
                else
                {
                    vel = 0;
                }
            }
            if (Projectile.ai[0] >= 95)
            {
                owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = -1;
                Projectile.Kill();
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

    }
}