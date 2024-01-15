using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using static Furioso.Systems.VertexUtils;

namespace Furioso.Projectiles.Crystal
{
    public class CrystalSlash2 : ModProjectile
    {
        public float Width = 0;
        public float Length = 700;
        Effect CrystalEffect2 = ModContent.Request<Effect>("Furioso/Effects/Content/CrystalEffect2").Value;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Atelier");
            //DisplayName.AddTranslation(7, "卡莉斯塔工作室");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.timeLeft = 35;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
            Projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] < 5)
            {
                Projectile.Opacity = Projectile.localAI[0] / 5;
                Width = 25;
            }
            else
            {
                Width = 25f * (35 - Projectile.localAI[0]) / 30f;
                if (Projectile.localAI[0] >= 35)
                {
                    Projectile.Kill();
                }
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Draw1(Main.spriteBatch);
            Draw2(Main.spriteBatch);
            Draw2(Main.spriteBatch);
            return false;
        }

        public void Draw1(SpriteBatch spriteBatch)
        {
            Vector2 normal = Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.Pi / 2);
            Vector2 vec = Projectile.rotation.ToRotationVector2();
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            triangleList.Add(new CustomVertexInfo(Projectile.Center - vec * Length / 2f * 1.1f - normal * Width / 2f * 1.1f, Color.White, new Vector3(0, 0, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center - vec * Length / 2f * 1.1f + normal * Width / 2f * 1.1f, Color.White, new Vector3(0, 1, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + vec * Length / 2f * 1.1f - normal * Width / 2f * 1.1f, Color.White, new Vector3(1, 0, 1)));

            triangleList.Add(new CustomVertexInfo(Projectile.Center + vec * Length / 2f * 1.1f + normal * Width / 2f * 1.1f, Color.White, new Vector3(1, 1, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + vec * Length / 2f * 1.1f - normal * Width / 2f * 1.1f, Color.White, new Vector3(1, 0, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center - vec * Length / 2f * 1.1f + normal * Width / 2f * 1.1f, Color.White, new Vector3(0, 1, 1)));

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
            Furioso.DurandalEffect.Parameters["alpha"].SetValue(Projectile.Opacity);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Furioso/Images/Extra_Crystal2").Value;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            Furioso.DurandalEffect.CurrentTechnique.Passes[0].Apply();


            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin();

        }

        public void Draw2(SpriteBatch spriteBatch)
        {
            Vector2 normal = Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.Pi / 2);
            Vector2 vec = Projectile.rotation.ToRotationVector2();
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            triangleList.Add(new CustomVertexInfo(Projectile.Center - vec * Length / 2f - normal * Width / 2f, Color.White, new Vector3(0, 0, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center - vec * Length / 2f + normal * Width / 2f, Color.White, new Vector3(0, 1, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + vec * Length / 2f - normal * Width / 2f, Color.White, new Vector3(1, 0, 1)));

            triangleList.Add(new CustomVertexInfo(Projectile.Center + vec * Length / 2f + normal * Width / 2f, Color.White, new Vector3(1, 1, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + vec * Length / 2f - normal * Width / 2f, Color.White, new Vector3(1, 0, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center - vec * Length / 2f + normal * Width / 2f, Color.White, new Vector3(0, 1, 1)));

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

            var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
            var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
            var screenPos = screenCenter - screenSize / 2f;
            var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

            // 把变换和所需信息丢给shader
            CrystalEffect2.Parameters["uTransform"].SetValue(model * projection);
            CrystalEffect2.Parameters["alpha"].SetValue(Projectile.Opacity);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Furioso/Images/Extra_Crystal2").Value;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            CrystalEffect2.CurrentTechnique.Passes[0].Apply();


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
            Vector2 vec = Projectile.rotation.ToRotationVector2();
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - vec * Length / 2, Projectile.Center + vec * Length / 2, Width, ref point);
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 vec = Projectile.rotation.ToRotationVector2();
            Terraria.Utils.PlotTileLine(Projectile.Center - vec * Length / 2, Projectile.Center + vec * Length / 2, (Width + 16) * Projectile.scale, DelegateMethods.CutTiles);
        }



        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Crystal);
            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}
