using Furioso.Items;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
namespace Furioso.Projectiles.Ranga
{
    public class RangaHeld : ModProjectile
    {
        public float vel = 0;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ranga WorkShop");
            //DisplayName.AddTranslation(7, "琅琊工坊");
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
            if (!owner.IsHoldingItemOrFurioso(ModContent.ItemType<RangaItem>()))
            {
                Projectile.Kill();
                return;
            }
            int dir = Math.Sign(Projectile.velocity.X + 0.01f);
            Projectile.Center = owner.Center;
            Projectile.direction = dir;
            Projectile.rotation = dir > 0 ? 0 : MathHelper.Pi;
            owner.heldProj = Projectile.whoAmI;
            owner.direction = dir;
            SomeUtils.SetItemTime(owner, 5);
            owner.itemRotation = (float)Math.Atan2(Projectile.rotation.ToRotationVector2().Y * dir, Projectile.rotation.ToRotationVector2().X * dir);
            owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = Projectile.type;
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] == 1)
            {
                vel = 125;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
                if (Projectile.localAI[1] < 2)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, new Vector2(dir, 0), ModContent.ProjectileType<RangaHeld1>(), Projectile.damage, 0, owner.whoAmI);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, new Vector2(dir, 0), ModContent.ProjectileType<RangaSlash1>(), Projectile.damage, 0, owner.whoAmI);
                }
                else
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, new Vector2(dir, 0), ModContent.ProjectileType<RangaHeld2>(), Projectile.damage, 0, owner.whoAmI);
                    int protmp = Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center + new Vector2(dir * 160, 20), new Vector2(dir, 0), ModContent.ProjectileType<RangaSlash2>(), Projectile.damage, 0, owner.whoAmI);
                    Main.projectile[protmp].rotation = dir > 0 ? 0 : MathHelper.Pi;
                }
                SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/Ranga" + ((int)Projectile.localAI[1] + 1).ToString()));
            }

            if (Projectile.localAI[0] < 20)
            {
                vel *= 0.75f;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
            }
            if (Projectile.localAI[0] >= 20 && Projectile.localAI[0] < 30)
            {
                vel = 0;
                owner.GetModPlayer<RolandPlayer>().Ram = true;
            }
            owner.velocity = new Vector2(owner.direction, 0) * vel;
            if (Projectile.localAI[0] >= 30)
            {
                Projectile.localAI[0] = 0;
                if (++Projectile.localAI[1] >= 3)
                {
                    owner.GetModPlayer<RolandPlayer>().WhatUAreUsing = -1;
                    owner.GetModPlayer<RolandPlayer>().Ram = false;
                    SomeUtils.GivePlayerImmune(owner, 30);
                    Projectile.Kill();
                    return;
                }
                else
                {
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