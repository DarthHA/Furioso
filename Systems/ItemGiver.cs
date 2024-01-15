using Furioso.Items;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Furioso.Systems
{
    public class ItemGiver : ModPlayer
    {
        public bool GiveGlove = false;

        public List<Item> SavedItem2 = new();

        public override void OnEnterWorld()
        {
            if (!GiveGlove)
            {
                GiveGlove = true;
                if (Player.IsBlackSilence())
                {
                    Item.NewItem(null, Player.Hitbox, ModContent.ItemType<BlackGloveItem>());
                }
            }

        }

        public override void PostUpdateMiscEffects()
        {
            if (SavedItem2.Count > 0)
            {
                foreach (Item item in SavedItem2)
                {
                    Player.QuickSpawnClonedItemDirect(Player.GetSource_FromThis(), item);
                }
                SavedItem2.Clear();
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("GiveGlove", GiveGlove);
            tag.Add("SavedIem2", SavedItem2);
        }

        public override void LoadData(TagCompound tag)
        {
            GiveGlove = tag.GetBool("GiveGlove");
            SavedItem2 = tag.Get<List<Item>>("SavedItem2");
        }


        public override bool CanSellItem(NPC vendor, Item[] shopInventory, Item item)
        {
            if (Main.LocalPlayer.IsBlackSilence() && item.type == ModContent.ItemType<BlackGloveItem>())
            {
                string str = Language.ActiveCulture.Name == "zh-Hans" ? "无价之宝" : "That's priceless";
                Main.NewText(str, Color.Red);
                return false;
            }
            return true;
        }

        //这个功能先放一放
        /*
        public override void PostUpdateMiscEffects()
        {

            for(int i = 0; i < 10; i++)
            {
                if (Player.GetModPlayer<RolandPlayer>().WeaponCD[i] > 0)
                {
                    string str = " - in cooldown";
                    ChangeItemName(FuriosoData.WeaponItems[i], Lang.GetItemNameValue(Player.HeldItem.type) + str);
                }
                else
                {
                    ChangeItemName(FuriosoData.WeaponItems[i], Lang.GetItemNameValue(Player.HeldItem.type));
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
                */
    }
}