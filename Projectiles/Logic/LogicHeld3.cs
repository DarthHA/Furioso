using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Logic
{
    public class LogicHeld3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ateller Logic");
            //DisplayName.AddTranslation(7, "?????");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 70;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<LogicItem>()))
            {
                Projectile.Kill();
                return;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.Center = owner.Center;
            owner.heldProj = Projectile.whoAmI;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Projectiles/Logic/LogicGun2").Value;
            float rot = Projectile.rotation;
            if (Projectile.velocity.X < 0)
            {
                rot = MathHelper.Pi + Projectile.rotation;
            }
            SpriteEffects SP = Projectile.velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(tex, Projectile.Center + Projectile.rotation.ToRotationVector2() * 40 - Main.screenPosition, null, lightColor, rot, tex.Size() / 2, Projectile.scale * 0.65f, SP, 0);
            return false;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}