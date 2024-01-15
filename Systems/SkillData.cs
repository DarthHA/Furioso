using System.Collections.Generic;
using Terraria;
namespace Furioso.Systems
{
    public static class SkillData
    {
        public static int[] WeaponItems = new int[10];
        public static List<int> SpecialProjs = new();
        public static int[] WeaponsCD = new int[10];


        public static int IsThisWeaponRoland(Item item)
        {
            if (item.IsAir) return -1;
            for (int i = 0; i < WeaponItems.Length; i++)
            {
                if (item.type == WeaponItems[i]) return i;
            }
            return -1;
        }

        public static void Unload()
        {
            SpecialProjs.Clear();
        }
    }
}