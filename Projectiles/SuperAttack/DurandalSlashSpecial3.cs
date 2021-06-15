using Furioso.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Furioso.VertexUtils;

namespace Furioso.Projectiles.SuperAttack
{
    public class DurandalSlashSpecial3 : ModProjectile
    {
        public Vector2 ShootPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Furioso");
            DisplayName.AddTranslation(GameCulture.Chinese, "Furioso");
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 40;
            FuriosoList.SpecialProjs.Add(projectile.type);
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.timeLeft = 75;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 120;
            projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            projectile.localAI[0]++;

            if (ShootPos == Vector2.Zero)
            {
                ShootPos = projectile.Center;
            }
            float t = (float)Math.Sqrt(projectile.localAI[0] / 60f);
            float len = (t - 0.5f) * 800f * projectile.scale;
            projectile.Center = ShootPos + projectile.rotation.ToRotationVector2() * len;
            projectile.velocity = projectile.rotation.ToRotationVector2();
            if (projectile.localAI[0] < 10)
            {
                projectile.Opacity = projectile.localAI[0] / 10;
            }
            else
            {
                projectile.Opacity = (40 - projectile.localAI[0]) / 30;
                if (projectile.localAI[0] >= 40)
                {
                    projectile.Kill();
                }
            }
            for (int i = projectile.oldPos.Length - 1; i > 0; --i)
                projectile.oldRot[i] = projectile.oldRot[i - 1];
            projectile.oldRot[0] = projectile.rotation;

        }


        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
            for (int i = 1; i < projectile.oldPos.Length; ++i)
            {
                if (projectile.oldPos[i] == Vector2.Zero) continue;
                var vCur = projectile.oldRot[i].ToRotationVector2();
                var vNext = projectile.oldRot[i - 1].ToRotationVector2();

                float dist = MathHelper.WrapAngle(projectile.oldRot[i - 1]) - MathHelper.WrapAngle(projectile.oldRot[i]);
                dist = Math.Abs(MathHelper.WrapAngle(dist));
                var middleDistance = (projectile.oldPos[i] - projectile.oldPos[i - 1]).Length() / 2;
                var controlPoint1 = projectile.oldPos[i] - vCur * middleDistance * 2;
                var controlPoint2 = projectile.oldPos[i - 1] + vNext * middleDistance * 2;
                List<Vector2> interp = new List<Vector2>();
                interp.Add(projectile.oldPos[i] - vCur);
                for (float t = 0; t <= dist; t += 0.05f)
                {
                    float it = t / dist;
                    if (dist == 0) it = 0;
                    var pos = Vector2.CatmullRom(controlPoint1, projectile.oldPos[i], projectile.oldPos[i - 1], controlPoint2, it);
                    interp.Add(pos);
                }
                interp.Add(projectile.oldPos[i - 1]);
                int width = (int)(15 * projectile.scale);

                for (int j = interp.Count - 1; j > 1; j--)
                {
                    var curPos = interp[j];
                    var nextPos = interp[j - 1];
                    var normalDir = nextPos - curPos;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    if (projectile.ai[0] >= 0)
                    {
                        normalDir = -normalDir;
                    }
                    var factor = (i - j / (float)interp.Count) / projectile.oldPos.Length;
                    var color = Color.Lerp(Color.White, Color.Red, factor);
                    var w = MathHelper.Lerp(1f, 0.05f, factor);
                    bars.Add(new CustomVertexInfo(curPos + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                    bars.Add(new CustomVertexInfo(curPos + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
                }
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形
                triangleList.Add(bars[0]);
                var vertex = new CustomVertexInfo((bars[0].Position + bars[1].Position) / 2 + Vector2.Normalize(projectile.velocity) * 30, Color.White,
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
                Main.graphics.GraphicsDevice.Textures[0] = mod.GetTexture("Images/Extra_Durandal3");
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

                Furioso.DurandalEffect.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                spriteBatch.End();
                spriteBatch.Begin();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return targetHitbox.Distance(projectile.Center) <= 20 * projectile.scale;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            FuriosoUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
        }
    }
}
