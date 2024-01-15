using Furioso.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Furioso.Projectiles.Wheel
{
    public class WheelExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wheel Industry");
            //DisplayName.AddTranslation(7, "轮盘重工");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0]++;
                for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.Pi / 8)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (i + MathHelper.Pi / 20).ToRotationVector2(), ModContent.ProjectileType<WheelExplosion1>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WheelExplosion2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
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
