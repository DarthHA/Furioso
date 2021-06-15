using Furioso.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Furioso.VertexUtils;

namespace Furioso.Projectiles.Wheel
{
    public class WheelExplosion2 : ModProjectile
    {
        public float Radius = 300;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wheel Industry");
            DisplayName.AddTranslation(GameCulture.Chinese, "轮盘重工");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height = 100;
            projectile.timeLeft = 120;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
            projectile.friendly = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 120;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }

        public override void AI()
        {
            projectile.ai[1]++;
            if (projectile.ai[1] == 1)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 Pos = projectile.Center;
                    Pos.X += (Main.rand.NextFloat() * 2 - 1) * Radius * 0.5f;
                    Pos.Y += (Main.rand.NextFloat() * 2 - 1) * Radius / 10f;
                    Gore gore = Gore.NewGoreDirect(Pos, Vector2.Zero, mod.GetGoreSlot("Gores/WheelRock"));
                    gore.frame = (byte)Main.rand.Next(4);
                }
            }
            if (projectile.ai[1] > 120)
            {
                projectile.Kill();
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle((int)projectile.Center.X - 150, (int)projectile.Center.Y - 300, 300, 350);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float k;
            if (projectile.ai[1] < 5)
            {
                k = projectile.ai[1] / 5f;
            }
            else
            {
                k = (120 - projectile.ai[1]) / 115f;
            }
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(-Radius, -Radius / 3), Color.White,
        new Vector3(0, 0f, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(-Radius, Radius / 3), Color.White,
        new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(Radius, -Radius / 3), Color.White,
        new Vector3(1, 0f, 1)));

            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(Radius, -Radius / 3), Color.White,
        new Vector3(1, 0f, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(-Radius, Radius / 3), Color.White,
        new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(Radius, Radius / 3), Color.White,
        new Vector3(1, 1f, 1)));


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

            var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
            var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
            var screenPos = screenCenter - screenSize / 2f;
            var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

            // 把变换和所需信息丢给shader
            Furioso.WheelEffect.Parameters["alpha"].SetValue(0.75f * k);

            Furioso.WheelEffect.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = mod.GetTexture("Images/Wheel2");
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            Furioso.WheelEffect.CurrentTechnique.Passes[0].Apply();


            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin();

        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            int CritRate = Math.Max(Main.player[projectile.owner].meleeCrit, Main.player[projectile.owner].rangedCrit);
            crit = Main.rand.Next(100) <= CritRate;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!Main.player[projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                FuriosoUtils.DeepAddBuff(target, ModContent.BuffType<WheelStun>(), 160);

                Main.player[projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Wheel);
            }
            else
            {
                FuriosoUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}
