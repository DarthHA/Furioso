using Terraria;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class AllasBrokenArmor : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Armor Penetration(Allas Workshop)");
            //DisplayName.AddTranslation(7, "盔甲穿透(阿拉斯工坊)");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = false;
            //canBeCleared = false;
        }
    }
}