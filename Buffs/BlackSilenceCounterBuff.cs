using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
	public class BlackSilenceCounterBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Black Silence Counter");
			Description.SetDefault("X types of Black Silence weapons have hit the target");
			DisplayName.AddTranslation(GameCulture.Chinese, "漆黑噤默计数器");
			Description.AddTranslation(GameCulture.Chinese, "已有X种漆黑噤默武器击中目标");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = false;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			rare = ItemRarityID.White;
			if (Language.ActiveCulture == GameCulture.Chinese)
			{
				tip = "已有" + Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() + "种漆黑噤默武器击中目标";
			}
			else
            {
				bool s = Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() > 1;
				tip = Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() + " type" + (s ? "s " : " ") + "of Black Silence weapons " + (s ? "have" : "has") + " hit the target";
			}
            if (Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() >= 9)
            {
				if (Language.ActiveCulture == GameCulture.Chinese)
				{
					tip += "\n现在可以发动Furioso";
				}
				else
				{
					tip += "\nCan use Furioso now";
				}
			}
		}
    }
}
