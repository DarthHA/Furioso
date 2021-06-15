using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Furioso.Items
{
	public abstract class BaseRolandItem : ModItem
	{
		public virtual DamageValue.DiceValue DiceDamage => new DamageValue.DiceValue(0, 0);
		public virtual int Slot => 0;

		public override string Texture => "Furioso/Items/BaseRolandItem";

        public override void SetDefaults()
		{
			item.damage = 12;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 4;
			item.value = 0;
			item.shootSpeed = 10;
			item.rare = ItemRarityID.Gray;
			item.autoReuse = false;
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			return false;
        }

        public override bool CanPickup(Player player)
        {
			return false;
        }

        public override bool CanUseItem(Player player)
        {
			if (player.GetModPlayer<RolandPlayer>().IsUsingGlove)
			{
				if (player.GetModPlayer<RolandPlayer>().WeaponCD[Slot] == 0)
				{
					player.GetModPlayer<RolandPlayer>().WeaponCD[Slot] = FuriosoList.WeaponsCD[Slot];
					return true;
				}
			}
			return false;
        }

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!SearchGlove(Main.LocalPlayer) && !Main.gamePaused)
			{
				item.TurnToAir();
				return;
			}
			Vector2 Size = Main.itemTexture[item.type].Size() * scale;
			float k = Main.LocalPlayer.GetModPlayer<RolandPlayer>().WeaponCD[Slot] / (float)FuriosoList.WeaponsCD[Slot];
			Vector2 Position = position - Size * 0.2f;
			Size *= 1.4f;
			Rectangle rectangle = new Rectangle((int)Position.X, (int)Position.Y + (int)(Size.Y * (1 - k)), (int)Size.X + 2, (int)(Size.Y * k));
			spriteBatch.Draw(Main.magicPixel, rectangle, frame, Color.Black * 0.75f);

			if (!Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.IsUsed((BlackSilenceCounter.Attack)Slot) &&
				!Main.LocalPlayer.GetModPlayer<RolandPlayer>().FuriosoAttacking)
			{
				Texture2D tex = mod.GetTexture("Items/BaseRolandItemUnused");
				Size = tex.Size() * scale;
				spriteBatch.Draw(tex, position - Size * 0.2f, frame, drawColor, 0, origin, scale * 1.4f, SpriteEffects.None, 0);
			}
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D tex = mod.GetTexture("Items/Roland" + Slot.ToString());
			Vector2 Size = tex.Size() * scale;
			spriteBatch.Draw(tex, position - Size * 0.2f, frame, drawColor, 0, origin, scale * 1.4f, SpriteEffects.None, 0);
			return false;
		}

        public bool SearchGlove(Player player)
        {
			for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
			{
				if (player.armor[i].type == ModContent.ItemType<BlackGloveItem>())
				{
					return true;
				}
			}
			return false;
		}

		public override void PostUpdate()
        {
			item.active = false;
		}

        public override void GetWeaponCrit(Player player, ref int crit)
		{
			crit = Math.Max(player.meleeCrit, player.rangedCrit) + 4;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				if (Language.ActiveCulture == GameCulture.Chinese)
				{
					tt.text = DiceDamage.Min + "-" + DiceDamage.Max + " 漆黑噤默伤害";
				}
				else
				{
					tt.text = DiceDamage.Min + "-" + DiceDamage.Max + " Black Silence Damage";
				}
			}

		}

        public override sealed bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if(player.IsBlackSilence() && player.statLife <= player.statLifeMax2 / 2)
            {
                if (Main.rand.Next(5) == 0)
                {
					player.GetModPlayer<RolandPlayer>().MaskImmune = true;
                }
            }
			ShootAlt(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			return false;
        }

		public virtual void ShootAlt(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

        }

        public override bool CanBurnInLava()
		{
			return false;
		}

	}
}