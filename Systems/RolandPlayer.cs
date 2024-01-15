using Furioso.Buffs;
using Furioso.Projectiles.Allas;
using Furioso.Projectiles.Crystal;
using Furioso.Projectiles.Durandal;
using Furioso.Projectiles.OldBoys;
using Furioso.Projectiles.Ranga;
using Furioso.Projectiles.SuperAttack;
using Furioso.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Furioso.Systems
{
    public class RolandPlayer : ModPlayer
    {
        public bool IsUsingGlove = false;
        public bool IsUsingGloveVanity = false;
        public bool ItemSaved = false;
        public Item[] SavedItem = new Item[10];
        public int[] WeaponCD = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public bool MaskImmune = false;

        public bool Ram = false;
        public int WhatUAreUsing = -1;
        public bool FuriosoAttacking = false;

        public BlackSilenceCounter BlackSilenceCounter = new BlackSilenceCounter();

        public bool CrystalDodge = false;

        public override void PreUpdateMovement()
        {
            if (IsUsingGlove)
            {
                if (Ram && WhatUAreUsing != -1)
                {
                    if (WhatUAreUsing == ModContent.ProjectileType<RangaHeld>())
                    {
                        Projectile ranga = Main.projectile[AnyProj(ModContent.ProjectileType<RangaHeld>())];
                        Player.velocity.X = Player.direction * (ranga.ModProjectile as RangaHeld).vel;
                        Player.velocity.Y = -0.01f;
                    }
                    else if (WhatUAreUsing == ModContent.ProjectileType<CrystalHeld>())
                    {
                        Projectile crystal = Main.projectile[AnyProj(ModContent.ProjectileType<CrystalHeld>())];
                        Player.velocity.X = Player.direction * (crystal.ModProjectile as CrystalHeld).vel;
                        Player.velocity.Y = -0.01f;
                    }
                    else if (WhatUAreUsing == ModContent.ProjectileType<CrystalHeldSpecial>())
                    {
                        Projectile crystal = Main.projectile[AnyProj(ModContent.ProjectileType<CrystalHeldSpecial>())];
                        Player.velocity.X = Player.direction * (crystal.ModProjectile as CrystalHeldSpecial).vel;
                        Player.velocity.Y = -0.01f;
                    }
                    else if (WhatUAreUsing == ModContent.ProjectileType<AllasHeld>())
                    {
                        Projectile allas = Main.projectile[AnyProj(ModContent.ProjectileType<AllasHeld>())];
                        Player.velocity = allas.rotation.ToRotationVector2() * (allas.ModProjectile as AllasHeld).vel;
                    }
                    else if (WhatUAreUsing == ModContent.ProjectileType<DurandalHeld>())
                    {
                        Projectile durandal = Main.projectile[AnyProj(ModContent.ProjectileType<DurandalHeld>())];
                        Player.velocity.X = Player.direction * (durandal.ModProjectile as DurandalHeld).vel;
                        Player.velocity.Y = 0;
                    }
                    else if (WhatUAreUsing == ModContent.ProjectileType<DurandalHeldSpecial>())
                    {
                        Player.velocity.X = 0;
                        Player.velocity.Y = 0;
                    }
                }
            }
        }

        public override void SetControls()
        {
            if (IsUsingGlove)
            {
                if (Ram)
                {
                    Player.controlUp = false;
                    Player.controlDown = false;
                    Player.controlHook = false;
                    Player.controlJump = false;
                    Player.controlLeft = false;
                    Player.controlMount = false;
                    Player.controlRight = false;
                    Player.mount.Dismount(Player);
                    Player.grappling[0] = -1;
                    Player.grapCount = 0;
                    Player.noFallDmg = true;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].aiStyle == 7)
                        {
                            Main.projectile[i].Kill();
                        }
                    }
                }
                else if (FuriosoAttacking)
                {
                    Player.controlHook = false;
                    Player.controlMount = false;
                    Player.mount.Dismount(Player);
                    Player.grappling[0] = -1;
                    Player.grapCount = 0;
                    Player.noFallDmg = true;
                    for (int i = 0; i < 1000; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].aiStyle == 7)
                        {
                            Main.projectile[i].Kill();
                        }
                    }
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (IsUsingGlove)
            {
                Player.AddBuff(ModContent.BuffType<BlackSilenceBuff>(), 2);
                if (!Player.GetModPlayer<RolandPlayer>().FuriosoAttacking)
                {
                    Player.AddBuff(ModContent.BuffType<BlackSilenceCounterBuff>(), 2);
                }

                if (Player.itemAnimation == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (WeaponCD[i] > 0)
                        {
                            WeaponCD[i]--;
                        }
                    }
                    MaskImmune = false;
                }

                if (Ram || FuriosoAttacking)
                {
                    Player.gravControl = false;
                    Player.gravControl2 = false;
                    Player.gravDir = 1;
                    Player.noFallDmg = true;
                }
                if (FuriosoAttacking)
                {
                    Player.AddBuff(ModContent.BuffType<FuriosoBuff>(), 2);
                    InfiniteMove();
                }
                if (WhatUAreUsing != ModContent.ProjectileType<CrystalHeld>())
                {
                    CrystalDodge = false;
                }

                if (!ItemSaved)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        SavedItem[i] = Player.inventory[i].Clone();
                        Player.inventory[i].SetDefaults(SkillData.WeaponItems[i]);
                        Player.inventory[i].TurnToAir();
                    }
                    ItemSaved = true;
                }

                for (int i = 0; i < 10; i++)
                {
                    if (Player.inventory[i].IsAir)
                    {
                        if (!Player.HasItem(SkillData.WeaponItems[i]))
                        {
                            Player.inventory[i].SetDefaults(SkillData.WeaponItems[i]);
                        }
                    }
                }
            }
            else
            {
                if (ItemSaved)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (SavedItem[i].IsAir) continue;
                        if (Player.inventory[i].IsAir || SkillData.IsThisWeaponRoland(Player.inventory[i]) != -1)
                        {
                            Player.inventory[i] = SavedItem[i].Clone();
                        }
                        else
                        {
                            ReturnItem(Player, SavedItem[i]);
                        }
                        SavedItem[i].TurnToAir();
                    }
                    ItemSaved = false;
                }
                QuickStop();
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (IsUsingGlove && FuriosoAttacking)
            {
                return false;
            }

            return true;
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (IsUsingGlove && FuriosoAttacking)
            {
                return false;
            }
            return true;
        }

        public override void PreSavePlayer()
        {
            if (ItemSaved)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (SavedItem[i].IsAir) continue;
                    if (Player.inventory[i].IsAir || SkillData.IsThisWeaponRoland(Player.inventory[i]) != -1)
                    {
                        Player.inventory[i] = SavedItem[i].Clone();
                    }
                    else
                    {
                        ReturnItem(Player, SavedItem[i]);
                    }
                    SavedItem[i].TurnToAir();
                }
                ItemSaved = false;
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (IsUsingGlove)
            {
                if (!Player.GetModPlayer<RolandPlayer>().FuriosoAttacking)
                {
                    if (AnyProj(ModContent.ProjectileType<OldBoysSwing>()) != -1)
                    {
                        Projectile swing = Main.projectile[AnyProj(ModContent.ProjectileType<OldBoysSwing>())];
                        modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
                        {
                            info.Damage -= (int)(swing.damage * DamageValue.OldBoysBlock);
                            if (info.Damage <= 1) info.Damage = 1;
                            info.DustDisabled = true;
                            info.SoundDisabled = true;
                            info.Knockback = 0;
                        };
                        BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.OldBoys);
                        if (swing.localAI[1] == 0)
                        {
                            swing.localAI[1] = 1;
                            Projectile.NewProjectile(Player.GetSource_ItemUse_WithPotentialAmmo(Player.HeldItem, 0), (npc.Center + Player.Center) / 2, Vector2.Zero, ModContent.ProjectileType<OldBoysGuard>(), 0, 0, 0);
                            SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/OldBoysGuard"));
                        }
                    }
                }
            }
        }



        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (!IsUsingGlove) return false;

            if (FuriosoAttacking)
            {
                return true;
            }
            if (WhatUAreUsing != -1)
            {
                if (!CrystalDodge && WhatUAreUsing == ModContent.ProjectileType<CrystalHeld>())
                {
                    CrystalDodge = true;
                    BlackSilenceCounter.AddCounter(BlackSilenceCounter.Attack.Crystal);
                    Player.statLife += DamageValue.CrystalHealLife;
                    if (Player.statLife > Player.statLifeMax2)
                    {
                        Player.statLife = Player.statLifeMax2;
                    }
                    Player.HealEffect(DamageValue.CrystalHealLife);
                    SoundEngine.PlaySound(new SoundStyle("Furioso/Sounds/CrystalEvasion"));
                }
                return true;
            }
            if (MaskImmune)
            {
                return true;
            }
            return false;
        }



        public override void ResetEffects()
        {
            if (IsUsingGlove)
            {
                if (WhatUAreUsing != -1)
                {
                    if (AnyProj(WhatUAreUsing) == -1)
                    {
                        Ram = false;
                        WhatUAreUsing = -1;
                    }
                }
                else
                {
                    Ram = false;
                }
                if (FuriosoAttacking)
                {
                    if (AnyProj(ModContent.ProjectileType<SuperAttackHeld>()) == -1)
                    {
                        FuriosoAttacking = false;
                    }
                }
            }
            IsUsingGlove = false;
            IsUsingGloveVanity = false;
        }
        public override void UpdateDead()
        {
            IsUsingGloveVanity = false;
            IsUsingGlove = false;
            QuickStop(true);
        }


        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (IsUsingGlove)
            {
                if (WhatUAreUsing == ModContent.ProjectileType<RangaHeld>())
                {
                    drawInfo.heldProjOverHand = true;
                    //drawInfo.drawHeldProjInFrontOfHeldItemAndBody = true;
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (IsUsingGlove)
            {
                damageSource = PlayerDeathReason.ByCustomReason(SomeUtils.GetTranslation("BSDeathText"));
            }
            return true;
        }

        public override void FrameEffects()
        {
            if (IsUsingGloveVanity)
            {
                Player.handon = EquipLoader.GetEquipSlot(Mod, "BlackSilenceHandsOn", EquipType.HandsOn);
                Player.handoff = EquipLoader.GetEquipSlot(Mod, "BlackSilenceHandsOff", EquipType.HandsOff);
                if (Player.IsBlackSilence())
                {
                    if (Player.statLife <= Player.statLifeMax2 / 2)
                    {
                        Player.face = EquipLoader.GetEquipSlot(Mod, "BlackSilenceMask", EquipType.Face);
                    }
                    Player.body = 10;
                    Player.legs = 10;
                }
            }
        }


        public void InfiniteMove()
        {
            Player.wingTime = Player.wingTimeMax;
            Player.rocketTime = Player.rocketTimeMax;
            Player.RefreshExtraJumps();
        }

        public void QuickStop(bool ResetCD = false)
        {
            WhatUAreUsing = -1;
            MaskImmune = false;
            CrystalDodge = false;
            FuriosoAttacking = false;
            Ram = false;
            BlackSilenceCounter.Clear();
            if (ResetCD)
            {
                for (int i = 0; i < 10; i++)
                {
                    WeaponCD[i] = 0;
                }
            }
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active)
                {
                    if (SkillData.SpecialProjs.Contains(proj.type))
                    {
                        proj.active = false;
                    }
                }
            }
        }



        private int AnyProj(int type)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == type && proj.owner == Player.whoAmI)
                {
                    return proj.whoAmI;
                }
            }
            return -1;
        }

        private void ReturnItem(Player player, Item item)
        {
            bool success = false;
            for (int i = 10; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].IsAir)
                {
                    player.inventory[i] = item.Clone();
                    success = true;
                    break;
                }
            }
            if (!success)
            {
                Player.GetModPlayer<ItemGiver>().SavedItem2.Add(item.Clone());
            }
        }
    }
}