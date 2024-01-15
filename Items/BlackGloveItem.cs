using Furioso.Systems;
using Furioso.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class BlackGloveItem : ModItem
    {
        public override void Load()
        {
            EquipLoader.AddEquipTexture(Mod, "Furioso/Items/BlackSilenceHandsOff", EquipType.HandsOff, this, "BlackSilenceHandsOff", null);
            EquipLoader.AddEquipTexture(Mod, "Furioso/Items/BlackSilenceHandsOn", EquipType.HandsOn, this, "BlackSilenceHandsOn", null);
            EquipLoader.AddEquipTexture(Mod, "Furioso/Items/BlackSilenceMask", EquipType.Face, this, "BlackSilenceMask", null);
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.sellPrice(1, 0, 0, 0);
            Item.rare = ItemRarityID.Gray;
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.IsBlackSilence())
            {
                player.GetModPlayer<RolandPlayer>().IsUsingGlove = true;
            }
            if (!hideVisual)
            {
                player.GetModPlayer<RolandPlayer>().IsUsingGloveVanity = true;
            }
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<RolandPlayer>().IsUsingGloveVanity = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.IsBlackSilence())
            {
                tooltips.Add(new TooltipLine(Mod, "Tooltip", SomeUtils.GetTranslation("GloveRolandDesc")));
            }
            else
            {
                tooltips.Add(new TooltipLine(Mod, "Tooltip", SomeUtils.GetTranslation("GloveNormalDesc")));
            }
        }


    }
}