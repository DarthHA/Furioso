using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Furioso.Buffs
{
    public class DurandalPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Durandal - Power");
            // Description.SetDefault("Your Black Silence weapon damage increased by 50%");
            //DisplayName.AddTranslation(7, "杜兰达尔-力量");
            //Description.AddTranslation(7, "你的漆黑噤默武器威力增加50%");
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            //canBeCleared = true;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            rare = ItemRarityID.White;
        }
    }
}
