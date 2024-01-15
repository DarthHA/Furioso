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

namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalSlashSpecial1 : ModProjectile
    {
        public Vector2[] OldPos = new Vector2[30];
        public float[] OldRot = new float[30];
        float r;
        public static float Radius = 150;
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
            Projectile.friendly = true;
            Projectile.timeLeft = 100;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 200;
            Projectile.extraUpdates = 1;
            Projectile.ownerHitCheck = true;
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
                    OldRot[i] = r - (float)i / OldRot.Length * MathHelper.TwoPi * Projectile.ai[0];
                    OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * Radius;
                    OldRot[i] = OldRot[i] + MathHelper.Pi / 2 * Projectile.ai[0];
                }

            }
            else
            {
                float k = (100 - Projectile.localAI[0]) / 65f;
                for (int i = 0; i < OldRot.Length; i++)
                {
                    OldRot[i] = r - (float)i / OldRot.Length * MathHelper.TwoPi * Projectile.ai[0] * k;
                    OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * Radius;
                    OldRot[i] = OldRot[i] + MathHelper.Pi / 2 * Projectile.ai[0];
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player owner = Main.player[Projectile.owner];
            bool flag = false;
            float point = 0;
            for (int i = 0; i < OldRot.Length; i++)
            {
                flag = flag || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), owner.Center, owner.Center + OldRot[i].ToRotationVector2() * (Radius + 25), 50, ref point);
            }
            return flag;
        }



        public override void CutTiles()
        {
            Player owner = Main.player[Projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            for (int i = 0; i < OldRot.Length; i++)
            {
                Terraria.Utils.PlotTileLine(owner.Center, owner.Center + OldRot[i].ToRotationVector2() * (Radius + 25), (50 + 16) * Projectile.scale, DelegateMethods.CutTiles);
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
                int width = (int)(10 * Projectile.scale);

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
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

                var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
                var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
                var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
                var screenPos = screenCenter - screenSize / 2f;
                var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

                // 把变换和所需信息丢给shader
                Furioso.DurandalEffect.Parameters["uTransform"].SetValue(model * projection);
                Furioso.DurandalEffect.Parameters["alpha"].SetValue(Projectile.Opacity);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Furioso/Images/Extra_Durandal1").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[1] = mod.GetTexture("Images/Extra_Trail");
                //Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;

                Furioso.DurandalEffect.CurrentTechnique.Passes[0].Apply();


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
