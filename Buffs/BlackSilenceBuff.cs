using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
	public class BlackSilenceBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Black Silence");
			Description.SetDefault("Every third Black Silence weapon's damage will be increased by 50%.");
			DisplayName.AddTranslation(GameCulture.Chinese, "漆黑噤默");
			Description.AddTranslation(GameCulture.Chinese, "每三次漆黑噤默武器伤害增加50%");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = false;
		}

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (Main.LocalPlayer.statLife <= Main.LocalPlayer.statLifeMax2 / 2)
            {
                if (Language.ActiveCulture == GameCulture.Chinese)
                {
					tip += "\n认知阻碍面具效果：\n" +
						"使用任意漆黑噤默武器时有20%的几率处于无敌状态";
                }
                else
                {
					tip += "\nEffect of Perception-blocking Mask:\n" +
						"You have 20% chance of being immune when using any Black Silence weapons";
				}
            }
        }
    }
}
