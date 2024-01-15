using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Furioso.Systems.VertexUtils;

namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalSlashSpecial2 : ModProjectile
    {
        public Vector2[] OldPos = new Vector2[30];
        public float[] OldRot = new float[30];
        float r;
        public static float Radius = 115;
        Effect DurandalEffect2 = ModContent.Request<Effect>("Furioso/Effects/Content/DurandalEffect2").Value;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Furioso");
            //DisplayName.AddTranslation(7, "Furioso");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = 100;
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
            if (Projectile.ai[0] == 0) Projectile.ai[0] = Main.rand.Next(2) * 2 - 1;
            Player owner = Main.player[Projectile.owner];
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] < 35)
            {
                float t = (float)Math.Sqrt(Projectile.localAI[0] / 35f);
                if (Projectile.ai[0] > 0)
                {
                    r = -MathHelper.Pi / 2 + MathHelper.Pi / 8 * 27 * t;
                }
                else
                {
                    r = -MathHelper.Pi / 2 - MathHelper.Pi / 8 * 27 * t;
                }
                Projectile.Opacity = 1;
            }
            else
            {
                if (Projectile.ai[0] > 0)
                {
                    r = MathHelper.Pi / 8 * 7;
                }
                else
                {
                    r = -MathHelper.Pi / 8 * 15;
                }
                Projectile.Opacity = (100f - Projectile.localAI[0]) / 65f;
            }

            Projectile.Center = owner.Center + r.ToRotationVector2() * Radius;
            //projectile.velocity = (r + MathHelper.Pi / 2 * projectile.ai[0]).ToRotationVector2();
            //projectile.rotation = projectile.velocity.ToRotation();
            if (Projectile.localAI[0] <= 35)
            {
                for (int i = 0; i < OldRot.Length; i++)
                {
                    OldRot[i] = r - (float)i / OldRot.Length * MathHelper.Pi / 2 * 3f * Projectile.ai[0];
                    OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * Radius;
                    OldRot[i] = OldRot[i] + MathHelper.Pi / 2 * Projectile.ai[0];
                }

            }
            else
            {
                float k = (100 - Projectile.localAI[0]) / 65 * 0.6f + 0.4f;
                for (int i = 0; i < OldRot.Length; i++)
                {
                    OldRot[i] = r - (float)i / OldRot.Length * MathHelper.Pi / 2 * 3f * Projectile.ai[0] * k;
                    //OldRot[i] = r - MathHelper.Pi / 2 * projectile.ai[0] + (float)(i - OldRot.Length / 2f) / OldRot.Length * MathHelper.Pi * projectile.ai[0] * k;
                    OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * Radius;
                    OldRot[i] = OldRot[i] + MathHelper.Pi / 2 * Projectile.ai[0];
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
                int width = (int)(33 * Projectile.scale);

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


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

                var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
                var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
                var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
                var screenPos = screenCenter - screenSize / 2f;
                var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

                // 把变换和所需信息丢给shader
                DurandalEffect2.Parameters["uTransform"].SetValue(model * projection);
                DurandalEffect2.Parameters["fadeout"].SetValue(Projectile.Opacity);
                DurandalEffect2.Parameters["threshold"].SetValue(0.25f);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Furioso/Images/Extra_Durandal2").Value;
                Main.graphics.GraphicsDevice.Textures[1] = ModContent.Request<Texture2D>("Furioso/Images/Perlin").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;

                DurandalEffect2.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin();
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
        }

    }
}
