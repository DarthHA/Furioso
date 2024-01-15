using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Zelkova
{
    public class ZelkovaHeld : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zelkova Workshop");
            //DisplayName.AddTranslation(7, "榉树工坊");
            SkillData.SpecialProjs.Add(Projectile.type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 999;
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<ZelkovaItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.direction = dir;
            Projectile.Center = owner.Center;
            SomeUtils.SetItemTime(owner, 5);

            Projectile.localAI[0]++;
            if (Projectile.localAI[0] == 1)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<ZelkovaHeldAxe>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.localAI[1]);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<ZelkovaHeldMace>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.localAI[1]);
                if (Projectile.localAI[1] == 0)
                {
                    Vector2 RelaPos = new Vector2(dir, 1) * -30 + new Vector2(0, -55);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, RelaPos, ModContent.ProjectileType<AxeSwing>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/ZelkovaAxe"));
                }
                else
                {
                    Vector2 RelaPos = new Vector2(dir, 1) * 55 + new Vector2(0, -55);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, RelaPos, ModContent.ProjectileType<MaceSwing>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/ZelkovaMace"));
                }
            }
            if (Projectile.localAI[0] >= 45)
            {
                Projectile.localAI[0] = 0;
                owner.direction = Math.Sign(Main.MouseWorld.X - owner.Center.X + 0.01f);
                Projectile.velocity = new Vector2(owner.direction, 0);
                if (++Projectile.localAI[1] >= 2)
                {
                    Projectile.Kill();
                    return;
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