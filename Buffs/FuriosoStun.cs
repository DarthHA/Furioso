using Terraria;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class FuriosoStun : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stunned(Furioso)");
            //DisplayName.AddTranslation(7, "击晕(Furioso)");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = false;
            //canBeCleared = false;
        }
    }
}