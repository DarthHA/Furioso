using Terraria;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class RangaBleeding : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bleeding(Ranga Workshop)");
            //DisplayName.AddTranslation(7, "流血(琅琊工坊)");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            //longerExpertDebuff = false;
            //canBeCleared = false;
        }
    }
}