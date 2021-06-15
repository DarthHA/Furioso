using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Furioso.Items;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace Furioso
{
    public class MiscRolandPlayer : ModPlayer
    {
        public bool GiveGlove = false;

        public override void OnEnterWorld(Player player)
        {
            if (!GiveGlove)
            {
                GiveGlove = true;
                if (player.IsBlackSilence())
                {
                    Item.NewItem(player.Hitbox, ModContent.ItemType<BlackGloveItem>());
                }
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"GiveGlove",GiveGlove},
            };
        }

        public override void Load(TagCompound tag)
        {
            GiveGlove = tag.GetBool("GiveGlove");
        }



        public override bool CanSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (Main.LocalPlayer.IsBlackSilence() && item.type == ModContent.ItemType<BlackGloveItem>())
            {
                string str = Language.ActiveCulture == GameCulture.Chinese ? "无价之宝" : "That's priceless";
                Main.NewText(str, Color.Red);
                return false;
            }
            return true;
        }


        public override void PostUpdateMiscEffects()
        {
            for (int i = 0; i < 10; i++)
            {
                if (player.GetModPlayer<RolandPlayer>().WeaponCD[i] > 0)
                {
                    string str1 = Language.ActiveCulture == GameCulture.Chinese ? FuriosoList.WeaponsNameCn[i] : FuriosoList.WeaponsNameEn[i];
                    string str2 = Language.ActiveCulture == GameCulture.Chinese ? "-冷却中" : " - in cooldown";
                    ChangeItemName(FuriosoList.WeaponItems[i], str1 + str2);
                }
                else
                {
                    string str = Language.ActiveCulture == GameCulture.Chinese ? FuriosoList.WeaponsNameCn[i] : FuriosoList.WeaponsNameEn[i];
                    ChangeItemName(FuriosoList.WeaponItems[i], str);
                }
            }
        }
        public void ChangeItemName(int type, string name)
        {
            FieldInfo field = typeof(Lang).GetField("_itemNameCache", BindingFlags.NonPublic | BindingFlags.Static);
            if (field != null)
            {
                LocalizedText[] _itemNameCache = (LocalizedText[])field.GetValue(new Lang());
                field = typeof(LocalizedText).GetField("value", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(_itemNameCache[type], name);
                }
            }
        }

    }
}