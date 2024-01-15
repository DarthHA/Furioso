using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class FuriosoBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Furioso");
            // Description.SetDefault("You are immune to damage, and all Black Silence weapons will stun enemies.");
            //DisplayName.AddTranslation(7, "Furioso");
            //Description.AddTranslation(7, "你免疫所有伤害，且所有漆黑噤默武器均会击晕敌人。");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            //canBeCleared = false;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            rare = ItemRarityID.Expert;
        }
    }
}
