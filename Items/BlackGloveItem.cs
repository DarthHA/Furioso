using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public class BlackGloveItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Black Glove");
			DisplayName.AddTranslation(GameCulture.Chinese, "漆黑手套");
            Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.accessory = true;
			item.width = 40;
			item.height = 40;
			item.value = Item.sellPrice(1, 0, 0, 0);
			item.rare = ItemRarityID.Gray;
		}


		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RolandPlayer>().IsUsingGlove = true;
			if (!hideVisual)
			{
				player.GetModPlayer<RolandPlayer>().IsUsingGloveVanity = true;
			}
		}

        public override void UpdateVanity(Player player, EquipType type)
        {
			player.GetModPlayer<RolandPlayer>().IsUsingGloveVanity = true;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Main.LocalPlayer.IsBlackSilence())
			{
				if (Language.ActiveCulture == GameCulture.Chinese)
				{
					tooltips.Add(new TooltipLine(mod, "Tooltip#0", "来自挚爱之人的遗物"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#1", "它承载着对过去的思念"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#2", "获得九种漆黑噤默武器"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#3", "拥有漆黑噤默效果"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#4", "生命低于50%时可以使用认知阻碍面具"));
				}
                else
                {
					tooltips.Add(new TooltipLine(mod, "Tooltip#0", "Relics from loved ones"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#1", "It carries the thoughts of the past"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#2", "Obtain nine Black Silence weapons"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#3", "Effects of the Black Silence"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#4", "Can use Perception-blocking Mask when below 50% health"));
				}
			}
			else
			{
				if (Language.ActiveCulture == GameCulture.Chinese)
				{
					tooltips.Add(new TooltipLine(mod, "Tooltip#0", "一副普通的黑手套"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#1", "真的吗？"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#2", "Obtain nine Black Silence weapons"));
				}
				else
				{
					tooltips.Add(new TooltipLine(mod, "Tooltip#0", "A pair of normal gloves"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#1", "Really?"));
					tooltips.Add(new TooltipLine(mod, "Tooltip#2", "Obtain nine Black Silence weapons"));
				}
			}
		}

        public override bool CanBurnInLava()
        {
			return false;
        }

    }
}