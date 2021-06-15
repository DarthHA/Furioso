using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class FuriosoStun : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stunned(Furioso)");
            DisplayName.AddTranslation(GameCulture.Chinese, "击晕(Furioso)");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }
    }
}