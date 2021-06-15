using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Furioso.VertexUtils;

namespace Furioso.Projectiles.Durandal
{
    public class DurandalSlash12 : ModProjectile
    {
        public Vector2[] OldPos = new Vector2[30];
        public float[] OldRot = new float[30];
        float r;
        public static float Radius = 115;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Durandal");
            DisplayName.AddTranslation(GameCulture.Chinese, "杜兰达尔");
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
            //if (projectile.ai[0] == 0) projectile.ai[0] = Main.rand.Next(2) * 2 - 1;
            Player owner = Main.player[projectile.owner];
            projectile.localAI[0]++;
            if (projectile.localAI[0] < 15)
            {
                float t = (float)Math.Sqrt(projectile.localAI[0] / 15f);
                if (projectile.ai[0] > 0)
                {
                    r = -MathHelper.Pi / 8 * 4 + MathHelper.Pi / 8 * 11 * t;
                }
                else
                {
                    r = -MathHelper.Pi / 8 * 4 - MathHelper.Pi / 8 * 11 * t;
                }
                projectile.Opacity = 1;
            }
            else
            {
                if (projectile.ai[0] > 0)
                {
                    r = MathHelper.Pi / 8 * 7;
                }
                else
                {
                    r = -MathHelper.Pi / 8 * 15;
                }
                projectile.Opacity = (60f - projectile.localAI[0]) / 45f;
            }

            projectile.Center = owner.Center + r.ToRotationVector2() * Radius;
            //projectile.velocity = (r + MathHelper.Pi / 2 * projectile.ai[0]).ToRotationVector2();
            //projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.localAI[0] <= 15)
            {
                for (int i = 0; i < OldRot.Length; i++)
                {
                    OldRot[i] = r - (float)i / OldRot.Length * MathHelper.Pi * projectile.ai[0];
                    OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * Radius;
                    OldRot[i] = OldRot[i] + MathHelper.Pi / 2 * projectile.ai[0];
                }

            }
            else
            {
                float k = (60 - projectile.localAI[0]) / 45 * 0.6f + 0.4f;
                for (int i = 0; i < OldRot.Length; i++)
                {
                    OldRot[i] = r - (float)i / OldRot.Length * MathHelper.Pi * projectile.ai[0] * k;
                    //OldRot[i] = r - MathHelper.Pi / 2 * projectile.ai[0] + (float)(i - OldRot.Length / 2f) / OldRot.Length * MathHelper.Pi * projectile.ai[0] * k;
                    OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * Radius;
                    OldRot[i] = OldRot[i] + MathHelper.Pi / 2 * projectile.ai[0];
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
            for (int i = 1; i < OldPos.Length; ++i)
            {
                if (OldPos[i] == Vector2.Zero) continue;
                if (OldPos[i] == OldPos[i - 1]) continue;
                var vCur = OldRot[i].ToRotationVector2();
                var vNext = OldRot[i - 1].ToRotationVector2();

                float dist = MathHelper.WrapAngle(OldRot[i - 1]) - MathHelper.WrapAngle(OldRot[i]);
                dist = Math.Abs(MathHelper.WrapAngle(dist));
                var middleDistance = (OldPos[i] - OldPos[i - 1]).Length() / 2;
                var controlPoint1 = OldPos[i] - vCur * middleDistance * 2;
                var controlPoint2 = OldPos[i - 1] + vNext * middleDistance * 2;
                List<Vector2> interp = new List<Vector2>();
                interp.Add(OldPos[i] - vCur);
                for (float t = 0; t <= dist; t += 0.05f)
                {
                    float it = t / dist;
                    if (dist == 0) it = 0;
                    var pos = Vector2.CatmullRom(controlPoint1, OldPos[i], OldPos[i - 1], controlPoint2, it);
                    interp.Add(pos);
                }
                interp.Add(OldPos[i - 1]);
                int width = (int)(33 * projectile.scale);

                for (int j = interp.Count - 1; j > 1; j--)
                {
                    var curPos = interp[j];
                    var nextPos = interp[j - 1];
                    var normalDir = nextPos - curPos;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                    var factor = (i - j / (float)interp.Count) / OldPos.Length;
                    //if (projectile.ai[0] > 0) factor = 1 - factor;
                    var color = Color.White;
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    bars.Add(new CustomVertexInfo(curPos + normalDir * -width, color, new Vector3(factor, 1, w)));
                    bars.Add(new CustomVertexInfo(curPos + normalDir * width, color, new Vector3(factor, 0, w)));
                }
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }


                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

                var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
                var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
                var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
                var screenPos = screenCenter - screenSize / 2f;
                var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

                // 把变换和所需信息丢给shader
                Furioso.DurandalEffect2.Parameters["uTransform"].SetValue(model * projection);
                Furioso.DurandalEffect2.Parameters["fadeout"].SetValue(projectile.Opacity);
                Furioso.DurandalEffect2.Parameters["threshold"].SetValue(0.25f);
                Main.graphics.GraphicsDevice.Textures[0] = mod.GetTexture("Images/Extra_Durandal2");
                Main.graphics.GraphicsDevice.Textures[1] = mod.GetTexture("Images/Perlin");
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;

                Furioso.DurandalEffect2.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                spriteBatch.End();
                spriteBatch.Begin();
            }
        }


    }
}
