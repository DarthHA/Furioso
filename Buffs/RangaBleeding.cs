using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class RangaBleeding : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bleeding(Ranga Workshop)");
            DisplayName.AddTranslation(GameCulture.Chinese, "流血(琅琊工坊)");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }
    }
}