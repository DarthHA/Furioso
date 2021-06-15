using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class AllasBrokenArmor : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Armor Penetration(Allas Workshop)");
            DisplayName.AddTranslation(GameCulture.Chinese, "盔甲穿透(阿拉斯工坊)");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
            canBeCleared = false;
        }
    }
}