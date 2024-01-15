using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using static Furioso.Systems.VertexUtils;

namespace Furioso.Projectiles.Wheel
{
    public class WheelSwing : ModProjectile
    {
        public Vector2[] OldPos = new Vector2[30];
        public float[] OldRot = new float[30];
        Effect WheelEffect2 = ModContent.Request<Effect>("Furioso/Effects/Content/WheelEffect2").Value;
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
            Projectile.friendly = true;
            Projectile.timeLeft = 40;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
            Projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            //if (projectile.ai[0] == 0) projectile.ai[0] = Main.rand.Next(2) * 2 - 1;
            Player owner = Main.player[Projectile.owner];
            Projectile.localAI[0]++;
            float r;
            if (Projectile.localAI[0] < 10)
            {
                float t = Projectile.localAI[0] / 10f;
                if (Projectile.ai[0] == 1)
                {
                    r = -MathHelper.Pi + MathHelper.Pi / 8f * 9f * t;
                }
                else
                {
                    r = 0 - MathHelper.Pi / 8f * 9f * t;
                }
            }
            else
            {
                if (Projectile.ai[0] == 1)
                {
                    r = MathHelper.Pi / 8f;
                }
                else
                {
                    r = -MathHelper.Pi / 8f * 9f;
                }
                Projectile.Opacity = (30f - Projectile.localAI[0]) / 30f;
            }


            Projectile.Center = owner.Center + r.ToRotationVector2() * 200;
            Projectile.velocity = (r + MathHelper.Pi / 2 * Projectile.ai[0]).ToRotationVector2();

            if (Projectile.localAI[0] <= 10)
            {
                for (int i = 0; i < OldRot.Length; i++)
                {
                    if (Projectile.ai[0] == 1)
                    {
                        OldRot[i] = MathHelper.Lerp(r, -MathHelper.Pi, (float)i / OldRot.Length);
                        OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * 200;
                        OldRot[i] += MathHelper.Pi / 2;
                    }
                    else
                    {
                        OldRot[i] = MathHelper.Lerp(r, 0, (float)i / OldRot.Length);
                        OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * 200;
                        OldRot[i] -= MathHelper.Pi / 2;
                    }
                }
            }
            else
            {
                for (int i = 0; i < OldRot.Length; i++)
                {
                    if (Projectile.ai[0] == 1)
                    {
                        float MinR = MathHelper.Lerp(-MathHelper.Pi, r, 1 - Projectile.timeLeft / 30f);
                        OldRot[i] = MathHelper.Lerp(r, MinR, (float)i / OldRot.Length);
                        OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * 200;
                        OldRot[i] += MathHelper.Pi / 2;
                    }
                    else
                    {
                        float MinR = MathHelper.Lerp(0, r, 1 - Projectile.timeLeft / 30f);
                        OldRot[i] = MathHelper.Lerp(r, MinR, (float)i / OldRot.Length);
                        OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * 200;
                        OldRot[i] -= MathHelper.Pi / 2;
                    }
                }
            }
        }


        public override void PostDraw(Color lightColor)
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
                int width = (int)(80 * Projectile.scale);

                for (int j = interp.Count - 1; j > 1; j--)
                {
                    var curPos = interp[j];
                    var nextPos = interp[j - 1];
                    var normalDir = nextPos - curPos;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    if (Projectile.ai[0] == 1)
                    {
                        normalDir = -normalDir;
                    }
                    var factor = (i - j / (float)interp.Count) / OldPos.Length;
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
                triangleList.Add(bars[0]);
                var vertex = new CustomVertexInfo((bars[0].Position + bars[1].Position) / 2 + Vector2.Normalize(Projectile.velocity) * 30, Color.White,
                    new Vector3(0, 0.5f, 1));
                triangleList.Add(bars[1]);
                triangleList.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

                var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
                var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
                var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
                var screenPos = screenCenter - screenSize / 2f;
                var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

                // 把变换和所需信息丢给shader
                WheelEffect2.Parameters["uTransform"].SetValue(model * projection);
                WheelEffect2.Parameters["threshold"].SetValue(0.1f);
                WheelEffect2.Parameters["fadeout"].SetValue((float)Math.Sqrt(Projectile.Opacity));
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Furioso/Images/Extra_Durandal2").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Furioso/Images/Perlin").Value;
                Main.graphics.GraphicsDevice.Textures[2] = ModContent.Request<Texture2D>("Furioso/Images/Extra_Wheel").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[1] = mod.GetTexture("Images/Extra_Trail");
                //Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;

                WheelEffect2.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin();
            }
        }



        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player owner = Main.player[Projectile.owner];
            bool flag = false;
            float point = 0;
            for (int i = 0; i < OldRot.Length; i++)
            {
                flag = flag || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), owner.Center, owner.Center + OldRot[i].ToRotationVector2() * 200, 50, ref point);
            }
            return flag;
        }

        public override void CutTiles()
        {
            Player owner = Main.player[Projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            for (int i = 0; i < OldRot.Length; i++)
            {
                Terraria.Utils.PlotTileLine(owner.Center, owner.Center + OldRot[i].ToRotationVector2() * 200, (50 + 16) * Projectile.scale, DelegateMethods.CutTiles);
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<WheelStun>(), 120);

                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Wheel);
            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}
