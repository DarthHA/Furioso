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
    public class CrystalSlash1 : ModProjectile
    {
        public Vector2[] OldPos = new Vector2[30];
        public float[] OldRot = new float[30];
        float r;
        public static float Radius = 120;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crystal Atelier");
            DisplayName.AddTranslation(GameCulture.Chinese, "卡莉斯塔工作室");
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.friendly = true;
            projectile.timeLeft = 60;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 200;
            projectile.extraUpdates = 1;
            projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 0) projectile.ai[0] = Main.rand.Next(2) * 2 - 1;
            Player owner = Main.player[projectile.owner];
            projectile.localAI[0]++;
            float StartR = projectile.ai[0] > 0 ? -MathHelper.Pi / 2 : MathHelper.Pi / 2 * 3;
            float EndR = projectile.ai[0] > 0 ? MathHelper.Pi / 24 * 23 : MathHelper.Pi / 24;
            if (projectile.localAI[0] < 15)
            {
                float t = (float)Math.Sqrt(projectile.localAI[0] / 15f);
                r = StartR + (EndR - StartR) * t;
                projectile.Opacity = 1;
            }
            else
            {
                r = EndR;
                projectile.Opacity = (60f - projectile.localAI[0]) / 45f;
            }
            r += projectile.rotation;

            float ModifiedRadius = 1;
            if (projectile.localAI[0] < 15)
            {
                ModifiedRadius = AO(projectile.localAI[0] / 15) * 0.5f + 0.5f;
            }
            projectile.Center = owner.Center + r.ToRotationVector2() * Radius * ModifiedRadius + projectile.velocity;

            if (projectile.localAI[0] <= 15)
            {
                for (int i = 0; i < OldRot.Length; i++)
                {
                    OldRot[i] = r - (float)i / OldRot.Length * (r - StartR);
                    ModifiedRadius = AO(GetK(StartR, EndR, OldRot[i])) * 0.5f + 0.5f;
                    OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * Radius * ModifiedRadius + projectile.velocity;
                    OldRot[i] = OldRot[i] + MathHelper.Pi / 2 * projectile.ai[0];
                }
                
            }
            else
            {
                float k = (60 - projectile.localAI[0]) / 45;
                for (int i = 0; i < OldRot.Length; i++)
                {
                    OldRot[i] = r - (float)i / OldRot.Length * (r - StartR) * k;
                    ModifiedRadius = AO(GetK(StartR, EndR, OldRot[i])) * 0.5f + 0.5f;
                    OldPos[i] = owner.Center + OldRot[i].ToRotationVector2() * Radius * ModifiedRadius + projectile.velocity;
                    OldRot[i] = OldRot[i] + MathHelper.Pi / 2 * projectile.ai[0];
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player owner = Main.player[projectile.owner];
            bool flag = false;
            float point = 0;
            for (int i = 0; i < OldRot.Length; i++)
            {
                flag = flag || Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), owner.Center, owner.Center + OldRot[i].ToRotationVector2() * (Radius + 15), 20, ref point);
            }
            return flag;
        }


        public override void CutTiles()
        {
            Player owner = Main.player[projectile.owner];
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            for (int i = 0; i < OldRot.Length; i++)
            {
                Utils.PlotTileLine(owner.Center, owner.Center + OldRot[i].ToRotationVector2() * (Radius + 25), (20 + 16) * projectile.scale, DelegateMethods.CutTiles);
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
                int width = (int)(13 * projectile.scale);

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
                Main.graphics.GraphicsDevice.Textures[0] = mod.GetTexture("Images/Extra_Durandal1");
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                //Main.graphics.GraphicsDevice.Textures[1] = mod.GetTexture("Images/Extra_Trail");
                //Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;

                Furioso.DurandalEffect.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                spriteBatch.End();
                spriteBatch.Begin();
            }
        }

        private float GetK(float begin, float end, float current)
        {
            return (current - begin) / (end - begin);
        }


        private float AO(float i)
        {
            return (float)Math.Sin(i * MathHelper.Pi + MathHelper.Pi) + 1;
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
