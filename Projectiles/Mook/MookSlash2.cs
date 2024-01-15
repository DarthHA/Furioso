using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Furioso.Systems.VertexUtils;

namespace Furioso.Projectiles.Mook
{
    public class MookSlash2 : ModProjectile
    {
        public Vector2 ShootPos = Vector2.Zero;
        Effect MookEffect = ModContent.Request<Effect>("Furioso/Effects/Content/MookEffect").Value;
        private int damage;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mook Workshop");
            //DisplayName.AddTranslation(7, "墨工坊");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.timeLeft = 100000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            Projectile.localAI[0]++;

            if (ShootPos == Vector2.Zero)
            {
                ShootPos = Projectile.Center;
            }
            float rot = Projectile.localAI[0] / 35 * MathHelper.TwoPi * Projectile.ai[0];
            Vector2 unit = rot.ToRotationVector2() * 200;
            unit.X *= Projectile.ai[1];
            unit = unit.RotatedBy(Projectile.rotation) * Projectile.scale;
            Projectile.Center = ShootPos + unit;
            Projectile.velocity = (rot + MathHelper.Pi / 2).ToRotationVector2() * Projectile.ai[0];
            if (Projectile.localAI[0] < 10)
            {
                Projectile.Opacity = Projectile.localAI[0] / 10;
            }
            else
            {
                Projectile.Opacity = (30 - Projectile.localAI[0]) / 20;
                if (Projectile.localAI[0] >= 30)
                {
                    Projectile.Kill();
                }
            }
            for (int i = Projectile.oldPos.Length - 1; i > 0; --i)
                Projectile.oldRot[i] = Projectile.oldRot[i - 1];
            Projectile.oldRot[0] = Projectile.rotation;
        }


        public override void PostDraw(Color lightColor)
        {

            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero) continue;
                var vCur = Projectile.oldRot[i].ToRotationVector2();
                var vNext = Projectile.oldRot[i - 1].ToRotationVector2();

                float dist = MathHelper.WrapAngle(Projectile.oldRot[i - 1]) - MathHelper.WrapAngle(Projectile.oldRot[i]);
                dist = Math.Abs(MathHelper.WrapAngle(dist));
                var middleDistance = (Projectile.oldPos[i] - Projectile.oldPos[i - 1]).Length() / 2;
                var controlPoint1 = Projectile.oldPos[i] - vCur * middleDistance * 2;
                var controlPoint2 = Projectile.oldPos[i - 1] + vNext * middleDistance * 2;
                List<Vector2> interp = new List<Vector2>();
                interp.Add(Projectile.oldPos[i] - vCur);
                for (float t = 0; t <= dist; t += 0.05f)
                {
                    float it = t / dist;
                    if (dist == 0) it = 0;
                    var pos = Vector2.CatmullRom(controlPoint1, Projectile.oldPos[i], Projectile.oldPos[i - 1], controlPoint2, it);
                    interp.Add(pos);
                }
                interp.Add(Projectile.oldPos[i - 1]);
                int width = (int)(10 * Projectile.scale);

                for (int j = interp.Count - 1; j > 1; j--)
                {
                    var curPos = interp[j];
                    var nextPos = interp[j - 1];
                    var normalDir = nextPos - curPos;
                    normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                    if (Projectile.ai[0] >= 0)
                    {
                        normalDir = -normalDir;
                    }
                    var factor = (i - j / (float)interp.Count) / Projectile.oldPos.Length;
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
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;

                var screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
                var screenSize = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
                var projection = Matrix.CreateOrthographicOffCenter(0, screenSize.X, screenSize.Y, 0, 0, 1);
                var screenPos = screenCenter - screenSize / 2f;
                var model = Matrix.CreateTranslation(new Vector3(-screenPos.X, -screenPos.Y, 0));

                // 把变换和所需信息丢给shader
                MookEffect.Parameters["uTransform"].SetValue(model * projection);
                MookEffect.Parameters["alpha"].SetValue(Projectile.Opacity);
                Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Furioso/Images/Extra_Mook").Value;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

                MookEffect.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return targetHitbox.Distance(ShootPos) <= 200 * Projectile.scale;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                damage = 100;
                int trueDamage = (int)(damage * DamageValue.MookTrueDamage);
                if (trueDamage < DamageValue.MookTrueDamageCap) trueDamage = DamageValue.MookTrueDamageCap;
                SomeUtils.DealTrueDamage(target, trueDamage);
                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Mook);

            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}
