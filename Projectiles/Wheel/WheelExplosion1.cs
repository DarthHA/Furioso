using Furioso.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Furioso.Systems.VertexUtils;

namespace Furioso.Projectiles.Wheel
{
    public class WheelExplosion1 : ModProjectile
    {
        public float Height = 400;
        public float Width = 200;
        Effect WheelEffect = ModContent.Request<Effect>("Furioso/Effects/Content/WheelEffect").Value;
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
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 60)
            {
                Projectile.Kill();
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Vector2 R = Projectile.rotation.ToRotationVector2() * Width;
            R.Y *= 0.3f;
            float k, a;
            if (Projectile.ai[1] < 10)
            {
                k = 1;
                a = Projectile.ai[1] / 10;
            }
            else
            {
                a = 1;
                k = (60 - Projectile.ai[1]) / 50f;
            }
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(0, -Height * k), Color.White,
        new Vector3(0, 0f, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(0, 0), Color.White,
        new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(0, -Height * k) + R, Color.White,
        new Vector3(1, 0f, 1)));

            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(0, -Height * k) + R, Color.White,
        new Vector3(1, 0f, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(0, 0), Color.White,
        new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(0, 0) + R, Color.White,
        new Vector3(1, 1f, 1)));


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

            var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
            var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
            var screenPos = screenCenter - screenSize / 2f;
            var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

            // 把变换和所需信息丢给shader
            WheelEffect.Parameters["alpha"].SetValue(0.45f * a);

            WheelEffect.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Furioso/Images/Wheel1").Value;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            WheelEffect.CurrentTechnique.Passes[0].Apply();


            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();

        }

    }
}
