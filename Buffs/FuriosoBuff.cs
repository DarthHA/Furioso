using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
	public class FuriosoBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Furioso");
			Description.SetDefault("You are immune to damage, and all Black Silence weapons will stun enemies.");
			DisplayName.AddTranslation(GameCulture.Chinese, "Furioso");
			Description.AddTranslation(GameCulture.Chinese, "你免疫所有伤害，且所有漆黑噤默武器均会击晕敌人。");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = false;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			rare = ItemRarityID.Expert;
		}
	}
}
