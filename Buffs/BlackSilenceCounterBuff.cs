using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class BlackSilenceCounterBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Black Silence Counter");
            // Description.SetDefault("X types of Black Silence weapons have hit the target");
            //DisplayName.AddTranslation(7, "漆黑噤默计数器");
            //Description.AddTranslation(7, "已有X种漆黑噤默武器击中目标");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            //canBeCleared = false;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            rare = ItemRarityID.White;
            tip = string.Format(SomeUtils.GetTranslation("BSCBuffHitCounter"), Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter());

            tip += "\n";
            if (Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() >= 9)
            {
                tip += SomeUtils.GetTranslation("BSCBuffCanUseFurioso");
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
        {
            drawParams.DrawColor = Color.White;
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            if (Main.LocalPlayer.GetModPlayer<RolandPlayer>().IsUsingGlove)
            {
                int value = Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter();
                Vector2 DrawPos = drawParams.Position + drawParams.MouseRectangle.Size() * 0.6f;
                Terraria.Utils.DrawBorderString(spriteBatch, value.ToString(), DrawPos, Color.White, 1f);
            }
        }
    }
}
