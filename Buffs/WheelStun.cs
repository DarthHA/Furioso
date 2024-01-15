using Terraria;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class WheelStun : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stunned(Wheel Industry)");
            //DisplayName.AddTranslation(7, "击晕(轮盘重工)");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = false;
            //canBeCleared = false;
        }
    }
}