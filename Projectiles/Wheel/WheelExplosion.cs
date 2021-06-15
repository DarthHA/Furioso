using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Projectiles.Wheel
{
    public class WheelExplosion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wheel Industry");
            DisplayName.AddTranslation(GameCulture.Chinese, "轮盘重工");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 2;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                projectile.ai[0]++;
                for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.Pi / 8)
                {
                    Projectile.NewProjectile(projectile.Center, (i + MathHelper.Pi / 20).ToRotationVector2(), ModContent.ProjectileType<WheelExplosion1>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<WheelExplosion2>(), projectile.damage, projectile.knockBack,projectile.owner);
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
