using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
	public class DurandalPower : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Durandal - Power");
			Description.SetDefault("Your Black Silence weapon damage increased by 50%");
			DisplayName.AddTranslation(GameCulture.Chinese, "杜兰达尔-力量");
			Description.AddTranslation(GameCulture.Chinese, "你的漆黑噤默武器威力增加50%");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
			canBeCleared = true;
		}

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
			rare = ItemRarityID.White;
        }
    }
}
