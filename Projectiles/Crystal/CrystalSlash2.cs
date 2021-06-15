using Furioso.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using static Furioso.VertexUtils;

namespace Furioso.Projectiles.Crystal
{
    public class CrystalSlash2 : ModProjectile
    {
        public float Width = 0;
        public float Length = 700;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Atelier");
            DisplayName.AddTranslation(GameCulture.Chinese, "卡莉斯塔工作室");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.timeLeft = 35;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 40;
            projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.localAI[0]++;
            if (projectile.localAI[0] < 5)
            {
                projectile.Opacity = projectile.localAI[0] / 5;
                Width = 25;
            }
            else
            {
                Width = 25f * (35 - projectile.localAI[0]) / 30f;
                if (projectile.localAI[0] >= 35)
                {
                    projectile.Kill();
                }
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Draw1(spriteBatch);
            Draw2(spriteBatch);
            Draw2(spriteBatch);
            return false;
        }

        public void Draw1(SpriteBatch spriteBatch)
        {
            Vector2 normal = projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.Pi / 2);
            Vector2 vec = projectile.rotation.ToRotationVector2();
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            triangleList.Add(new CustomVertexInfo(projectile.Center - vec * Length / 2f * 1.1f - normal * Width / 2f * 1.1f, Color.White, new Vector3(0, 0, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center - vec * Length / 2f * 1.1f + normal * Width / 2f * 1.1f, Color.White, new Vector3(0, 1, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + vec * Length / 2f * 1.1f - normal * Width / 2f * 1.1f, Color.White, new Vector3(1, 0, 1)));

            triangleList.Add(new CustomVertexInfo(projectile.Center + vec * Length / 2f * 1.1f + normal * Width / 2f * 1.1f, Color.White, new Vector3(1, 1, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + vec * Length / 2f * 1.1f - normal * Width / 2f * 1.1f, Color.White, new Vector3(1, 0, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center - vec * Length / 2f * 1.1f + normal * Width / 2f * 1.1f, Color.White, new Vector3(0, 1, 1)));

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

            var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
            var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
            var screenPos = screenCenter - screenSize / 2f;
            var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

            // 把变换和所需信息丢给shader
            Furioso.DurandalEffect.Parameters["uTransform"].SetValue(model * projection);
            Furioso.DurandalEffect.Parameters["alpha"].SetValue(projectile.Opacity);
            Main.graphics.GraphicsDevice.Textures[0] = mod.GetTexture("Images/Extra_Crystal2");
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            Furioso.DurandalEffect.CurrentTechnique.Passes[0].Apply();


            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin();

        }

        public void Draw2(SpriteBatch spriteBatch)
        {
            Vector2 normal = projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.Pi / 2);
            Vector2 vec = projectile.rotation.ToRotationVector2();
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            triangleList.Add(new CustomVertexInfo(projectile.Center - vec * Length / 2f - normal * Width / 2f, Color.White, new Vector3(0, 0, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center - vec * Length / 2f + normal * Width / 2f, Color.White, new Vector3(0, 1, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + vec * Length / 2f - normal * Width / 2f, Color.White, new Vector3(1, 0, 1)));

            triangleList.Add(new CustomVertexInfo(projectile.Center + vec * Length / 2f + normal * Width / 2f, Color.White, new Vector3(1, 1, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + vec * Length / 2f - normal * Width / 2f, Color.White, new Vector3(1, 0, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center - vec * Length / 2f + normal * Width / 2f, Color.White, new Vector3(0, 1, 1)));

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

            var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
            var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
            var screenPos = screenCenter - screenSize / 2f;
            var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

            // 把变换和所需信息丢给shader
            Furioso.CrystalEffect2.Parameters["uTransform"].SetValue(model * projection);
            Furioso.CrystalEffect2.Parameters["alpha"].SetValue(projectile.Opacity);
            Main.graphics.GraphicsDevice.Textures[0] = mod.GetTexture("Images/Extra_Crystal2");
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            Furioso.CrystalEffect2.CurrentTechnique.Passes[0].Apply();


            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin();

        }


        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 vec = projectile.rotation.ToRotationVector2();
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center - vec * Length / 2, projectile.Center + vec * Length / 2, Width, ref point);
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 vec = projectile.rotation.ToRotationVector2();
            Utils.PlotTileLine(projectile.Center - vec * Length / 2, projectile.Center + vec * Length / 2, (Width + 16) * projectile.scale, DelegateMethods.CutTiles);
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
                Main.player[projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Crystal);
            }
            else
            {
                FuriosoUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}
