using Furioso.Systems;
using Furioso.Utils;
using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class BlackSilenceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Black Silence");
            // Description.SetDefault("Every third Black Silence weapon's damage will be increased by 50%.");
            //DisplayName.AddTranslation(7, "漆黑噤默");
            //Description.AddTranslation(7, "每三次漆黑噤默武器伤害增加50%");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            //canBeCleared = false;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            if (Main.LocalPlayer.statLife <= Main.LocalPlayer.statLifeMax2 / 2)
            {
                tip += SomeUtils.GetTranslation("BSBuffExtraDesc");
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            drawParams.DrawColor = Color.White;
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            int value = DamageValue.BlackSilenceCardCounter + 1;
            Vector2 DrawPos = drawParams.Position + drawParams.MouseRectangle.Size() * 0.6f;
            Terraria.Utils.DrawBorderString(spriteBatch, value.ToString(), DrawPos, Color.White, 1f);
        }
    }
}
