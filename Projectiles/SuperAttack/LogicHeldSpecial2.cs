using Furioso.Projectiles.Logic;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class LogicHeldSpecial2 : ModProjectile
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
            if (!owner.IsHoldingItemOrFurioso())
            {
                Projectile.Kill();
                return;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.Center = owner.Center;
            owner.heldProj = Projectile.whoAmI;
            owner.direction = Math.Sign(Projectile.velocity.X);
            SomeUtils.SetItemTime(owner, 5);
            int dir = owner.direction;

            Projectile.ai[1]++;

            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);
            if (Projectile.ai[1] == 1)
            {
                int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Projectile.velocity, ModContent.ProjectileType<LogicHeld3>(), 0, 0, owner.whoAmI);
                Main.projectile[protmp].timeLeft = 50;
                float rot = Projectile.velocity.ToRotation();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center + new Vector2(0, -5) + rot.ToRotationVector2() * 150, rot.ToRotationVector2(), ModContent.ProjectileType<LogicGunExplosion2>(), Projectile.damage * 2, Projectile.knockBack, owner.whoAmI);
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/LogicAtk2"));
            }
            if (Projectile.ai[1] > 50)
            {
                Projectile.Kill();
                return;
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