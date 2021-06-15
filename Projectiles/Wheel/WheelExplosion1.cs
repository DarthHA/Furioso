using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Furioso.VertexUtils;

namespace Furioso.Projectiles.Wheel
{
    public class WheelExplosion1 : ModProjectile
    {
        public float Height = 400;
        public float Width = 200;
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
            projectile.timeLeft = 60;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
            projectile.hide = true;
        }


        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindProjectiles.Add(index);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.ai[1]++;
            if (projectile.ai[1] > 60)
            {
                projectile.Kill();
            }
        }



        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 R = projectile.rotation.ToRotationVector2() * Width;
            R.Y *= 0.3f;
            float k, a;
            if (projectile.ai[1] < 10)
            {
                k = 1;
                a = projectile.ai[1] / 10;
            }
            else
            {
                a = 1;
                k = (60 - projectile.ai[1]) / 50f;
            }
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(0, -Height * k), Color.White,
        new Vector3(0, 0f, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(0, 0), Color.White,
        new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(0, -Height * k) + R, Color.White,
        new Vector3(1, 0f, 1)));

            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(0, -Height * k) + R, Color.White,
        new Vector3(1, 0f, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(0, 0), Color.White,
        new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(projectile.Center + new Vector2(0, 0) + R, Color.White,
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
            Furioso.WheelEffect.Parameters["alpha"].SetValue(0.45f * a);

            Furioso.WheelEffect.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = mod.GetTexture("Images/Wheel1");
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            Furioso.WheelEffect.CurrentTechnique.Passes[0].Apply();


            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            spriteBatch.End();
            spriteBatch.Begin();

        }

    }
}
