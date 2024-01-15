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
    public class LogicHeldSpecial1 : ModProjectile
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


            if (Projectile.ai[1] == 0)
            {
                float rot = Projectile.velocity.ToRotation();

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, rot.ToRotationVector2(), ModContent.ProjectileType<LogicHeld2>(), 0, 0, owner.whoAmI);
                if (Projectile.velocity.X < 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, (MathHelper.Pi / 4).ToRotationVector2(), ModContent.ProjectileType<LogicHeld1>(), 0, 0, owner.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, (MathHelper.Pi / 4 * 3).ToRotationVector2(), ModContent.ProjectileType<LogicHeld1>(), 0, 0, owner.whoAmI);
                }

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center + new Vector2(0, -5) + rot.ToRotationVector2() * 80, rot.ToRotationVector2(), ModContent.ProjectileType<LogicGunExplosion1>(), Projectile.damage, Projectile.knockBack, owner.whoAmI);
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/LogicAtk1"));
            }
            if (Projectile.ai[1] <= 30)
            {
                float r = MathHelper.Pi / 2;
                int dir = owner.direction;
                owner.itemRotation = (float)Math.Atan2(r.ToRotationVector2().Y * dir, r.ToRotationVector2().X * dir);
            }
            else
            {
                int dir = owner.direction;
                owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);
            }
            if (Projectile.ai[1] == 31)
            {
                Projectile.velocity = Vector2.Normalize(Main.MouseWorld - owner.Center) * 10;
                float rot = Projectile.velocity.ToRotation();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, rot.ToRotationVector2(), ModContent.ProjectileType<LogicHeld1>(), Projectile.damage, Projectile.knockBack, owner.whoAmI);
                if (Projectile.velocity.X < 0)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, (MathHelper.Pi / 8 * 3).ToRotationVector2(), ModContent.ProjectileType<LogicHeld2>(), 0, 0, owner.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, (MathHelper.Pi / 8 * 5).ToRotationVector2(), ModContent.ProjectileType<LogicHeld2>(), 0, 0, owner.whoAmI);
                }

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center + new Vector2(0, -5) + rot.ToRotationVector2() * 80, rot.ToRotationVector2(), ModContent.ProjectileType<LogicGunExplosion1>(), Projectile.damage, Projectile.knockBack, owner.whoAmI);
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/LogicAtk1"));
            }
            if (Projectile.ai[1] >= 62)
            {
                Projectile.Kill();
                return;
            }
            Projectile.ai[1]++;
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