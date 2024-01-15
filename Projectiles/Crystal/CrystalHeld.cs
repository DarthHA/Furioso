using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Crystal
{
    public class CrystalHeld : ModProjectile
    {
        public float vel = 0;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Atelier");
            //DisplayName.AddTranslation(7, "卡莉斯塔工作室");
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<CrystalItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.Center = owner.Center;
            Projectile.direction = dir;
            Projectile.rotation = dir > 0 ? 0 : MathHelper.Pi;
            owner.direction = dir;
            SomeUtils.SetItemTime(owner, 5);
            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);
            owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = Projectile.type;
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] == 1)
            {
                vel = 200;
                owner.GetModPlayer<RolandPlayer>().Ram = true;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(dir, 0), ModContent.ProjectileType<CrystalHeld1>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(dir, 0), ModContent.ProjectileType<CrystalHeld2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CrystalSlash1>(), Projectile.damage, Projectile.knockBack, Projectile.owner, dir);
                int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(dir * 9f, 24f), ModContent.ProjectileType<CrystalSlash1>(), Projectile.damage, Projectile.knockBack, Projectile.owner, dir);
                Main.projectile[protmp].rotation = -dir * 0.2f;
                //Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<CrystalSlash12>(), projectile.damage, projectile.knockBack, projectile.owner, dir);
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/Crystal"));
            }
            if (Projectile.localAI[0] == 4 && Projectile.localAI[1] == 1)
            {
                Vector2 ForePos = Projectile.Center;// + new Vector2(400, 0) * dir;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), ForePos, (MathHelper.Pi / 6).ToRotationVector2(), ModContent.ProjectileType<CrystalSlash2>(), Projectile.damage, 0, Projectile.owner, dir);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), ForePos, (-MathHelper.Pi / 6).ToRotationVector2(), ModContent.ProjectileType<CrystalSlash2>(), Projectile.damage, 0, Projectile.owner, dir);
            }

            if (Projectile.localAI[0] < 30)
            {
                vel *= 0.75f;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
            }
            if (Projectile.localAI[0] >= 30 && Projectile.localAI[0] < 50)
            {
                vel = 0;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
            }
            owner.velocity = new Vector2(owner.direction, 0) * vel;
            if (Projectile.localAI[0] >= 50)
            {
                Projectile.localAI[0] = 0;
                if (++Projectile.localAI[1] >= 2)
                {
                    owner.GetModPlayer<RolandPlayer>().Ram = false;
                    owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = -1;
                    SomeUtils.GivePlayerImmune(owner, 30);
                    Projectile.Kill();
                    return;
                }
                else
                {
                    owner.GetModPlayer<RolandPlayer>().CrystalDodge = false;
                    Projectile.velocity = new Vector2(Math.Sign(Projectile.ai[0] - owner.Center.X + 0.01f), 0);
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

    }


}