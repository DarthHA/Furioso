using Furioso.Items;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
namespace Furioso
{
	public static class FuriosoList
	{
		public static int[] WeaponItems = new int[10];
		public static List<int> SpecialProjs = new List<int>();
		public static int[] WeaponsCD = new int[10];
		public static string[] WeaponsNameEn = new string[10];
		public static string[] WeaponsNameCn = new string[10];

		public static void Load()
		{
			WeaponItems = new int[10] {
			ModContent.ItemType<RangaItem>(),
			ModContent.ItemType<OldBoysItem>(),
			ModContent.ItemType<ZelkovaItem>(),
			ModContent.ItemType<LogicItem>(),
			ModContent.ItemType<DurandalItem>(),
			ModContent.ItemType<MookItem>(),
			ModContent.ItemType<AllasItem>(),
			ModContent.ItemType<CrystalItem>(),
			ModContent.ItemType<WheelItem>(),
			ModContent.ItemType<FuriosoItem>(),
			};

			WeaponsCD = new int[10]
			{
				60,
				60,
				60,
				120,
				120,
				120,
				120,
				180,
				240,
				5,
			};
			WeaponsNameEn = new string[10]
			{
				"Ranga Workshop",
				"Old Boys Workshop",
				"Zelkova Workshop",
				"Ateller Logic",
				"Durandal",
				"Mook Workshop",
				"Allas Workshop",
				"Crystal Atelier",
				"Wheel Industry",
				"Furioso",
			};

			WeaponsNameCn = new string[10]
{
				"琅琊工坊",
				"老男孩工坊",
				"榉树工坊",
				"逻辑工作室",
				"杜兰达尔",
				"墨工坊",
				"阿拉斯工坊",
				"卡莉斯塔工坊",
				"轮盘重工",
				"Furioso",
};
		}


		public static void Unload()
		{
			SpecialProjs.Clear();
		}
	}
}