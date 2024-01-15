using Furioso.Buffs;
using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Furioso.Systems.VertexUtils;

namespace Furioso.Projectiles.Wheel
{
    public class WheelExplosion2 : ModProjectile
    {
        public float Radius = 300;
        Effect WheelEffect = ModContent.Request<Effect>("Furioso/Effects/Content/WheelEffect").Value;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wheel Industry");
            //DisplayName.AddTranslation(7, "轮盘重工");
            SkillData.SpecialProjs.Add(Projectile.type);
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.timeLeft = 120;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 120;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }

        public override void AI()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] == 1)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 Pos = Projectile.Center;
                    Pos.X += (Main.rand.NextFloat() * 2 - 1) * Radius * 0.5f;
                    Pos.Y += (Main.rand.NextFloat() * 2 - 1) * Radius / 10f;
                    //Gore gore = Gore.NewGoreDirect(null, Pos, Vector2.Zero, ModContent.Request<Gore>("Furioso/Gores/WheelRock").Value);
                    //gore.frame = (byte)Main.rand.Next(4);
                }
            }
            if (Projectile.ai[1] > 120)
            {
                Projectile.Kill();
            }
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle((int)Projectile.Center.X - 150, (int)Projectile.Center.Y - 300, 300, 350);
        }

        public override void PostDraw(Color lightColor)
        {
            float k;
            if (Projectile.ai[1] < 5)
            {
                k = Projectile.ai[1] / 5f;
            }
            else
            {
                k = (120 - Projectile.ai[1]) / 115f;
            }
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(-Radius, -Radius / 3), Color.White,
        new Vector3(0, 0f, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(-Radius, Radius / 3), Color.White,
        new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(Radius, -Radius / 3), Color.White,
        new Vector3(1, 0f, 1)));

            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(Radius, -Radius / 3), Color.White,
        new Vector3(1, 0f, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(-Radius, Radius / 3), Color.White,
        new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(Projectile.Center + new Vector2(Radius, Radius / 3), Color.White,
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
            WheelEffect.Parameters["alpha"].SetValue(0.75f * k);

            WheelEffect.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Furioso/Images/Wheel2").Value;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            WheelEffect.CurrentTechnique.Passes[0].Apply();


            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);

            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();

        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<WheelStun>(), 160);

                Main.player[Projectile.owner].GetModPlayer<RolandPlayer>().BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Wheel);
            }
            else
            {
                SomeUtils.DeepAddBuff(target, ModContent.BuffType<FuriosoStun>(), 180);
            }
        }
    }
}
