using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class WheelStun : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stunned(Wheel Industry)");
            DisplayName.AddTranslation(GameCulture.Chinese, "击晕(轮盘重工)");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }
    }
}