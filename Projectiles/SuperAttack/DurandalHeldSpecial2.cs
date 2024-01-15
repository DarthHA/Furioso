using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalHeldSpecial2 : ModProjectile
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
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;
            Projectile.Center = owner.Center + new Vector2(0, 10);

            Projectile.rotation = dir >= 0 ? MathHelper.Pi / 16 * 13 : MathHelper.Pi / 16 * 3;

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 100)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/SuperAttack/DurandalHeldSpecial2").Value;
            Main.spriteBatch.Draw(tex, Projectile.Center + Projectile.rotation.ToRotationVector2() * 30 - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.None, 0);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

    }
}