using Furioso.Projectiles.Durandal;
using Furioso.Projectiles.Mook;
using Furioso.Projectiles.OldBoys;
using Furioso.Projectiles.Ranga;
using Furioso.Projectiles.Wheel;
using Furioso.Projectiles.Zelkova;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class SuperAttackHeld : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Furioso");
            //DisplayName.AddTranslation(7, "Furioso");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 99999;
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
            if (!owner.IsHoldingItemOrFurioso())
            {
                Projectile.Kill();
                return;
            }
            Projectile.Center = owner.Center;
            SomeUtils.SetItemTime(owner, 25);
            owner.GetModPlayer<RolandPlayer>().FuriosoAttacking = true;
            if (Projectile.localAI[0] == 0)
            {
                LaunchAttack((int)Projectile.localAI[1]);
            }

            Projectile.localAI[0]++;
            int EndTimer = 0;
            switch (Projectile.localAI[1])
            {
                case 0:     //??1(??)
                    EndTimer = 62;
                    break;
                case 1:     //???
                    EndTimer = 55;
                    break;
                case 2:    //???
                    EndTimer = 45;
                    break;
                case 3:   //?
                    EndTimer = 75;
                    break;
                case 4:   //??
                    EndTimer = 90;
                    break;
                case 5:   //??
                    EndTimer = 90;
                    break;
                case 6:  //??
                    EndTimer = 70;
                    break;
                case 7:   //????
                    EndTimer = 51;
                    break;
                case 8:   //??2(??)
                    EndTimer = 51;
                    break;
                case 9:   //????
                    EndTimer = 95;
                    break;
                case 10:  //??????
                    EndTimer = 80;
                    break;
                default:
                    break;
            }
            if (Projectile.localAI[0] > EndTimer)
            {
                Projectile.localAI[1]++;
                Projectile.localAI[0] = 0;
                owner.direction = Math.Sign(Main.MouseWorld.X - owner.Center.X);
                if (Projectile.localAI[1] > 10)
                {
                    owner.GetModPlayer<RolandPlayer>().FuriosoAttacking = false;
                    Projectile.Kill();
                    return;
                }
            }

        }


        public void LaunchAttack(int id)
        {
            Player owner = Main.player[Projectile.owner];
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - owner.Center);
            switch (id)
            {
                case 0:     //??1(??)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<LogicHeldSpecial1>(), owner.GetRolandDamage(DamageValue.Logic), 0, Projectile.owner);
                    break;
                case 1:     //???
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<AllasHeldSpecial>(), owner.GetRolandDamage(DamageValue.Allas), 0, Projectile.owner);
                    break;
                case 2:    //???
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<OldBoysHeld>(), owner.GetRolandDamage(DamageValue.OldBoy), 0, Projectile.owner);
                    break;
                case 3:   //?
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<MookHeld>(), owner.GetRolandDamage(DamageValue.Mook), 0, Projectile.owner);
                    break;
                case 4:   //??
                    float dist = Main.MouseWorld.X - owner.Center.X;
                    dist = MathHelper.Clamp(dist, -150, 150) + owner.Center.X;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<RangaHeld>(), owner.GetRolandDamage(DamageValue.Ranga), 0, Projectile.owner, dist, 0);
                    break;
                case 5:   //??
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<ZelkovaHeld>(), owner.GetRolandDamage(DamageValue.Zelkova), 0, Projectile.owner);
                    break;
                case 6:  //??
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<WheelHeld>(), owner.GetRolandDamage(DamageValue.Wheel), 0, Projectile.owner);
                    break;
                case 7:   //????
                    dist = Main.MouseWorld.X - owner.Center.X;
                    dist = MathHelper.Clamp(dist, -150, 150) + owner.Center.X;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<CrystalHeldSpecial>(), owner.GetRolandDamage(DamageValue.Crystal), 0, Projectile.owner, dist, 0);
                    break;
                case 8:   //??2(??)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<LogicHeldSpecial2>(), (int)(owner.GetRolandDamage(DamageValue.Logic) * 1.5f), 0, Projectile.owner);
                    break;
                case 9:   //????
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<DurandalHeld>(), owner.GetRolandDamage(DamageValue.Durandal), 0, Projectile.owner);
                    break;
                case 10:  //??????
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, ShootVel, ModContent.ProjectileType<DurandalHeldSpecial>(), owner.GetRolandDamage(DamageValue.Furioso), 0, Projectile.owner);
                    break;
                default:
                    break;
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