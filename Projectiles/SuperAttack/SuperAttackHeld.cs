using Furioso.Items;
using Furioso.Projectiles.Durandal;
using Furioso.Projectiles.Mook;
using Furioso.Projectiles.OldBoys;
using Furioso.Projectiles.Ranga;
using Furioso.Projectiles.Wheel;
using Furioso.Projectiles.Zelkova;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class SuperAttackHeld : ModProjectile
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
            projectile.timeLeft = 99999;
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
            projectile.Center = owner.Center;
            owner.SetItemTime(25);
            owner.GetModPlayer<RolandPlayer>().FuriosoAttacking = true;
            if (projectile.localAI[0] == 0)
            {
                LaunchAttack((int)projectile.localAI[1]);
            }

            projectile.localAI[0]++;
            int EndTimer = 0;
            switch (projectile.localAI[1])
            {
                case 0:     //Âß¼­1£¨ÊÖÇ¹£©
                    EndTimer = 62;
                    break;
                case 1:     //°¢À­Ë¹
                    EndTimer = 55;
                    break;
                case 2:    //ÀÏÄÐº¢
                    EndTimer = 45;
                    break;
                case 3:   //Ä«
                    EndTimer = 75;
                    break;
                case 4:   //ÀÅçð
                    EndTimer = 90;
                    break;
                case 5:   //é·Ê÷
                    EndTimer = 90;
                    break;
                case 6:  //ÂÖÅÌ
                    EndTimer = 70;
                    break;
                case 7:   //¿¨ÀòË¹Ëþ
                    EndTimer = 51;
                    break;
                case 8:   //Âß¼­2£¨²½Ç¹£©
                    EndTimer = 51;
                    break;
                case 9:   //¶ÅÀ¼´ï¶û
                    EndTimer = 95;
                    break;
                case 10:  //¶ÅÀ¼´ï¶ûÊÕÎ²
                    EndTimer = 80;
                    break;
                default:
                    break;
            }
            if (projectile.localAI[0] > EndTimer)
            {
                projectile.localAI[1]++;
                projectile.localAI[0] = 0;
                owner.direction = Math.Sign(Main.MouseWorld.X - owner.Center.X);
                if (projectile.localAI[1] > 10)
                {
                    owner.GetModPlayer<RolandPlayer>().FuriosoAttacking = false;
                    projectile.Kill();
                    return;
                }
            }

        }


        public void LaunchAttack(int id)
        {
            Player owner = Main.player[projectile.owner];
            Vector2 ShootVel = Vector2.Normalize(Main.MouseWorld - owner.Center);
            switch (id)
            {
                case 0:     //Âß¼­1£¨ÊÖÇ¹£©
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<LogicHeldSpecial1>(), owner.GetRolandDamage(DamageValue.Logic), 0, projectile.owner);
                    break;
                case 1:     //°¢À­Ë¹
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<AllasHeldSpecial>(), owner.GetRolandDamage(DamageValue.Allas), 0, projectile.owner);
                    break;
                case 2:    //ÀÏÄÐº¢
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<OldBoysHeld>(), owner.GetRolandDamage(DamageValue.OldBoy), 0, projectile.owner);
                    break;
                case 3:   //Ä«
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<MookHeld>(), owner.GetRolandDamage(DamageValue.Mook), 0, projectile.owner);
                    break;
                case 4:   //ÀÅçð
                    float dist = Main.MouseWorld.X - owner.Center.X;
                    dist = MathHelper.Clamp(dist, -150, 150) + owner.Center.X;
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<RangaHeld>(), owner.GetRolandDamage(DamageValue.Ranga), 0, projectile.owner, dist, 0);
                    break;
                case 5:   //é·Ê÷
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<ZelkovaHeld>(), owner.GetRolandDamage(DamageValue.Zelkova), 0, projectile.owner);
                    break;
                case 6:  //ÂÖÅÌ
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<WheelHeld>(), owner.GetRolandDamage(DamageValue.Wheel), 0, projectile.owner);
                    break;
                case 7:   //¿¨ÀòË¹Ëþ
                    dist = Main.MouseWorld.X - owner.Center.X;
                    dist = MathHelper.Clamp(dist, -150, 150) + owner.Center.X;
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<CrystalHeldSpecial>(), owner.GetRolandDamage(DamageValue.Crystal), 0, projectile.owner, dist, 0);
                    break;
                case 8:   //Âß¼­2£¨²½Ç¹£©
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<LogicHeldSpecial2>(), (int)(owner.GetRolandDamage(DamageValue.Logic) * 1.5f), 0, projectile.owner);
                    break;
                case 9:   //¶ÅÀ¼´ï¶û
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<DurandalHeld>(), owner.GetRolandDamage(DamageValue.Durandal), 0, projectile.owner);
                    break;
                case 10:  //¶ÅÀ¼´ï¶ûÊÕÎ²
                    Projectile.NewProjectile(owner.Center, ShootVel, ModContent.ProjectileType<DurandalHeldSpecial>(), owner.GetRolandDamage(DamageValue.Furioso), 0, projectile.owner);
                    break;
                default:
                    break;
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