using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Gores
{
    public class WheelRock : ModGore
    {
        public override void OnSpawn(Gore gore, IEntitySource source)
        {
            gore.scale = 0.4f + 0.15f * Main.rand.NextFloat();
            gore.alpha = 0;
            gore.velocity = (MathHelper.Pi * Main.rand.NextFloat() - MathHelper.Pi).ToRotationVector2() * (8 * Main.rand.NextFloat() + 4);
            gore.numFrames = 4;
            gore.behindTiles = false;
            gore.rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
            gore.sticky = false;
            gore.timeLeft = Gore.goreTime * 3;
        }

        public override bool Update(Gore gore)
        {
            gore.velocity *= 0.99f;
            gore.rotation += Math.Sign(gore.velocity.X) * 0.075f;
            gore.position += gore.velocity;
            gore.scale -= 0.015f;
            if (gore.scale <= 0.04f)
            {
                gore.active = false;
            }
            return false;
        }
    }
}