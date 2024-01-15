using Furioso.Systems;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Furioso.Items
{
    public abstract class BaseRolandItem : ModItem
    {
        public virtual DamageValue.DiceValue DiceDamage => new(0, 0);
        public virtual int Slot => 0;

        public virtual int WeaponCD => 60;

        public override string Texture => "Furioso/Items/BaseRolandItem";

        public sealed override void SetStaticDefaults()
        {
            SkillData.WeaponItems[Slot] = Item.type;
            SkillData.WeaponsCD[Slot] = WeaponCD;

        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4;
            Item.value = 0;
            Item.shootSpeed = 10;
            Item.rare = ItemRarityID.Gray;
            Item.autoReuse = false;
        }
        public override bool AltFunctionUse(Player player) => true;

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
                if (player.altFunctionUse == 2)
                {
                    return true;
                }
                if (player.GetModPlayer<RolandPlayer>().WeaponCD[Slot] == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!SearchGlove(Main.LocalPlayer) && !Main.gamePaused)
            {
                Item.TurnToAir();
                return;
            }
            Vector2 Size = ModContent.Request<Texture2D>("Furioso/Items/BaseRolandItem").Value.Size() * scale;
            float k = Main.LocalPlayer.GetModPlayer<RolandPlayer>().WeaponCD[Slot] / (float)SkillData.WeaponsCD[Slot];
            Vector2 Position = position - Size * 0.7f;
            Size *= 1.4f;
            Rectangle rectangle = new((int)Position.X, (int)Position.Y + (int)(Size.Y * (1 - k)), (int)Size.X + 2, (int)(Size.Y * k));
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, frame, Color.Black * 0.75f);

            if (!Main.LocalPlayer.GetModPlayer<RolandPlayer>().BlackSilenceCounter.IsUsed((BlackSilenceCounter.Attack)Slot) &&
                !Main.LocalPlayer.GetModPlayer<RolandPlayer>().FuriosoAttacking)
            {
                Texture2D tex = ModContent.Request<Texture2D>("Furioso/Items/BaseRolandItemUnused").Value;
                Size = tex.Size() * scale;
                spriteBatch.Draw(tex, position - Size * 0.2f, frame, drawColor, 0, origin, scale * 1.4f, SpriteEffects.None, 0);
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D tex = ModContent.Request<Texture2D>("Furioso/Items/Roland" + Slot.ToString()).Value;
            spriteBatch.Draw(tex, position, frame, drawColor, 0, origin, scale * 1.4f, SpriteEffects.None, 0);
            return false;
        }

        public bool SearchGlove(Player player)
        {
            return player.GetModPlayer<RolandPlayer>().IsUsingGlove;
        }

        public override void PostUpdate()
        {
            Item.active = false;
        }


        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            crit = player.GetCritChance(DamageClass.Melee) + player.GetCritChance(DamageClass.Ranged) + player.GetCritChance(DamageClass.Generic) + 4;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (tt != null)
            {
                tt.Text = DiceDamage.Min + "-" + DiceDamage.Max + SomeUtils.GetTranslation("BSDamage");
            }
            tooltips.Add(new TooltipLine(Mod, "ExtraData", SomeUtils.GetTranslation("WeaponExtraDesc")));

        }

        public override sealed bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                List<int> AllWeapon = new();
                List<int> UnUsedWeapon = new();
                if (player.GetModPlayer<RolandPlayer>().BlackSilenceCounter.GetCounter() >= 9)       //优先使用Furioso
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int weapon = SkillData.IsThisWeaponRoland(player.inventory[i]);
                        if (weapon == 9)
                        {
                            player.selectedItem = i;
                            return false;
                        }
                    }

                }

                for (int i = 0; i < 10; i++)
                {
                    int weapon = SkillData.IsThisWeaponRoland(player.inventory[i]);
                    if (weapon >= 0 && weapon <= 8)
                    {
                        if (!player.GetModPlayer<RolandPlayer>().BlackSilenceCounter.IsUsed(weapon))
                        {
                            UnUsedWeapon.Add(i);
                        }
                        if (player.selectedItem != i)
                        {
                            AllWeapon.Add(i);
                        }
                    }

                }
                if (UnUsedWeapon.Count > 0)       //优先使用未使用武器
                {
                    player.selectedItem = UnUsedWeapon[Main.rand.Next(UnUsedWeapon.Count)];
                }
                else if (AllWeapon.Count > 0)
                {
                    player.selectedItem = AllWeapon[Main.rand.Next(AllWeapon.Count)];
                }

            }
            else
            {
                if (player.statLife <= player.statLifeMax2 / 2)
                {
                    if (Main.rand.NextBool(5))
                    {
                        player.GetModPlayer<RolandPlayer>().MaskImmune = true;
                    }
                }
                SafeShoot(player, source, position, velocity, type, damage, knockback);
                player.GetModPlayer<RolandPlayer>().WeaponCD[Slot] = SkillData.WeaponsCD[Slot];
            }
            return false;
        }

        public virtual void SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

        }


    }
}